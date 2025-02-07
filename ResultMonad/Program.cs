namespace ResultMonad;

internal static class Program
{
    private static Result<int> ParseInt(string strNumber) =>
        int.TryParse(strNumber, out var number)
            ? Result<int>.Success(number)
            : Result<int>.Failure("Parsing error");

    private static Result<int> Multiply(int number1, int number2)
    {
        try
        {
            return Result<int>.Success(checked(number1 * number2));
        }
        catch (OverflowException)
        {
            return Result<int>.Failure("Overflow");
        }
    }

    private static Result<Empty> WriteNumberToFile(string filePath, int number)
    {
        try
        {
            File.WriteAllText(filePath, number.ToString());
            return Result<Empty>.Success();
        }
        catch (Exception ex)
        {
            return Result<Empty>.Failure($"Ошибка записи файла: {ex.Message}");
        }
    }

    private static async Task<Result<Empty>> WriteNumberToFileAsync(string filePath, int number)
    {
        try
        {
            await File.WriteAllTextAsync(filePath, number.ToString());
            return Result<Empty>.Success();
        }
        catch (Exception ex)
        {
            return Result<Empty>.Failure($"Ошибка записи файла: {ex.Message}");
        }
    }

    public static void Main() =>
        Result<int>.New(_ => ParseInt("123"))
            .Bind(number => Multiply(number, 2))
            .Bind(number => Multiply(number, 3))
            .Bind(number => WriteNumberToFile("output.txt", number))
            .Handle(result => Console.WriteLine(result.IsSuccess ? "Тут ОК" : $"Ошибка: {result.Error}"))
            .Bind(_ => ParseInt("a123"))
            .Bind(number => Multiply(number, 2))
            .Bind(number => Multiply(number, 3))
            .Bind(number => WriteNumberToFile("output.txt", number))
            .Handle(result => Console.WriteLine(result.IsSuccess ? "ОК" : $"А тут ошибка парсинга: {result.Error}"))
            .Bind(_ => ParseInt("123"))
            .Bind(number => Multiply(number, 2))
            .Bind(number => Multiply(number, 300000000))
            .AsAsync()
            .BindAsync(number => WriteNumberToFileAsync("output.txt", number))
            .HandleAsync(async result => Console.WriteLine(await result.GetIsSuccessAsync()
                ? "ОК"
                : $"А тут ошибка переполнения: {await result.GetErrorAsync()}"))
            .Bind(_ => ParseInt("123"))
            .Bind(number => Multiply(number, 2))
            .Bind(number => Multiply(number, 4))
            .Bind(number => WriteNumberToFile("output2.txt", number))
            .HandleAsync(async result => Console.WriteLine(await result.GetIsSuccessAsync()
                ? "Тут снова ОК"
                : $"Ошибка: {await result.GetErrorAsync()}"));
}
