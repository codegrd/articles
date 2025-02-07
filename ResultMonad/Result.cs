namespace ResultMonad;

public class Result<T>
{
    private readonly Func<(bool isSuccess, T? value, string? error)> _lazyEvaluator;
    private (bool isSuccess, T? value, string? error)? _evaluationResult;

    private (bool isSuccess, T? value, string? error) Evaluate() => _evaluationResult ??= _lazyEvaluator();

    public bool IsSuccess => Evaluate().isSuccess;
    public T? Value => Evaluate().value;
    public string? Error => !IsSuccess ? Evaluate().error : null;

    private Result(Func<(bool, T?, string?)> lazyEvaluator) => _lazyEvaluator = lazyEvaluator;

    public static Result<T> New(Func<Empty, Result<T>> value) => Result<Empty>.Success().Bind(value);

    public static Result<T> Success(T value) => new(() => (true, value, null));
    public static Result<T> Failure(string error) => new(() => (false, default, error));

    public static Result<Empty> Success() => Result<Empty>.Success(Empty.Default);

    public Result<U> Bind<U>(Func<T, Result<U>> func) => new(() => IsSuccess
        ? func(Value!).Evaluate()
        : (false, default, Error));
}

public class AsyncResult<T>
{
    private readonly Func<Task<(bool isSuccess, T? value, string? error)>> _lazyEvaluator;
    private (bool isSuccess, T? value, string? error)? _evaluationResult;

    private async Task<(bool isSuccess, T? value, string? error)> EvaluateAsync()
    {
        _evaluationResult ??= await _lazyEvaluator().ConfigureAwait(false);
        return _evaluationResult.Value;
    }

    private AsyncResult(Func<Task<(bool, T?, string?)>> lazyEvaluator)
    {
        _lazyEvaluator = lazyEvaluator;
    }

    public static AsyncResult<T> New(Func<Empty, AsyncResult<T>> value) => AsyncResult<Empty>.Success().Bind(value);

    public static AsyncResult<T> FromResult(Result<T> result)
        => new(() => Task.FromResult((result.IsSuccess, result.Value, result.Error)));

    public static AsyncResult<T> Success(T value)
        => new(() => Task.FromResult((true, value, (string?)null))!);

    public static AsyncResult<T> Failure(string error)
        => new(() => Task.FromResult((false, default(T?), error))!);

    public static AsyncResult<Empty> Success() => AsyncResult<Empty>.Success(Empty.Default);

    public async Task<bool> GetIsSuccessAsync()
    {
        var (isSuccess, _, _) = await EvaluateAsync().ConfigureAwait(false);
        return isSuccess;
    }

    public async Task<T?> GetValueAsync()
    {
        var (_, value, _) = await EvaluateAsync().ConfigureAwait(false);
        return value;
    }

    public async Task<string?> GetErrorAsync()
    {
        if (await GetIsSuccessAsync().ConfigureAwait(false))
            return null;
        var (_, _, error) = await EvaluateAsync().ConfigureAwait(false);
        return error;
    }

    // Асинхронный Bind к другому асинхронному результату
    public AsyncResult<U> Bind<U>(Func<T, AsyncResult<U>> func)
        => new(async () =>
        {
            var (isSuccess, val, err) = await EvaluateAsync().ConfigureAwait(false);
            if (!isSuccess)
                return (false, default, err);
            return await func(val!).EvaluateAsync().ConfigureAwait(false);
        });

    // Асинхронный Bind к синхронному Result (на случай смешанных вычислений)
    public AsyncResult<U> Bind<U>(Func<T, Result<U>> func)
        => new(async () =>
        {
            var (isSuccess, val, err) = await EvaluateAsync().ConfigureAwait(false);
            if (!isSuccess)
                return (false, default, err);
            var next = func(val!);
            return (next.IsSuccess, next.Value, next.Error);
        });

    // Асинхронный Bind к ф-ции возвращающей Task<Result<U>>
    // ВАЖНО: Мы не выполняем task сейчас. Мы оборачиваем вызов func в ленивую лямбду,
    // которая будет вызвана при первом обращении к EvaluateAsync().
    public AsyncResult<U> BindAsync<U>(Func<T, Task<Result<U>>> func)
        => new(async () =>
        {
            var (isSuccess, val, err) = await EvaluateAsync().ConfigureAwait(false);
            if (!isSuccess)
                return (false, default, err);
            var next = await func(val!).ConfigureAwait(false);
            return (next.IsSuccess, next.Value, next.Error);
        });

    public async Task<Result<T>> RunAsync()
    {
        var (isSuccess, value, error) = await EvaluateAsync().ConfigureAwait(false);
        return isSuccess ? Result<T>.Success(value!) : Result<T>.Failure(error!);
    }
}

public readonly struct Empty
{
    public static readonly Empty Default = new();
}

public static class ResultExtensions
{
    public static AsyncResult<T> AsAsync<T>(this Result<T> result)
        => AsyncResult<T>.FromResult(result);

    public static Result<Empty> Handle<T>(this Result<T> result, Action<Result<T>> action)
    {
        action(result);
        return Result<Empty>.Success();
    }

    public static AsyncResult<Empty> HandleAsync<T>(this AsyncResult<T> result, Func<AsyncResult<T>, Task> action)
    {
        action(result);
        return AsyncResult<Empty>.Success();
    }
}
