namespace FrameworkGame.GuessNumberGame;

public sealed class GuessNumberGame
{
    private int _roundsLeft;

    private readonly Dictionary<Player, int> _playerScores;

    private readonly HashSet<Player> _players;

    // Можно скрыть за интерфейсом и дать возможность переопределить снаружи
    private readonly Random _random;

    private readonly (int left, int right) _range;

    private readonly INotifier _notifier;

    internal GuessNumberGame(INotifier notifier, int rounds, (int left, int right) range, HashSet<Player> players)
    {
        ArgumentNullException.ThrowIfNull(notifier);
        ArgumentNullException.ThrowIfNull(players);

        if (rounds <= 0)
        {
            throw new ArgumentException("Rounds must be positive", nameof(rounds));
        }

        if (range.right <= range.left)
        {
            throw new ArgumentException("Right number must be greater than left", nameof(range));
        }

        if (players.Count < 2)
        {
            throw new ArgumentException("There must be at least 2 players", nameof(players));
        }

        _notifier = notifier;
        _roundsLeft = rounds;
        _range = range;
        _players = players;
        _playerScores = players.ToDictionary(x => x, _ => 0);
        _random = new Random();
    }


    // В случае с фреймворком, он сам выполняет всю логику.
    // Возвращаемое значение можно добавить, например, чтобы снаружи отследить корректность завершения.
    public void Start()
    {
        while (!IsFinished())
        {
            var playerAnswers = _players.Select(x => new PlayerAnswer(x, x.GuessNumber(_range))).ToArray();

            NotifyAboutPlayerAnswers(playerAnswers);

            var expectedNumber = _random.Next(_range.left, _range.right + 1);

            var roundResult = DetermineRoundResult(expectedNumber, playerAnswers);

            Array.ForEach(roundResult.Winners.ToArray(), x => _playerScores[x.Player]++);

            NotifyAboutRoundResult(roundResult);

            NextRound();
        }

        var gameResult = GetResult();

        NotifyAboutGameResult(gameResult);
    }

    // Можно гипотетически вынести в интерфейс, чтобы была возможность переопределить логику игры и написать юнит-тесты
    // PlayerAnswer можно сделать интерфейсом
    private static RoundResult DetermineRoundResult(int expectedNumber, PlayerAnswer[] playerAnswers)
    {
        var bestGuessedNumber = playerAnswers.Aggregate(
            playerAnswers.First().GuessedNumber,
            (x, y) => CalculateClosest(expectedNumber, x, y.GuessedNumber)
        );

        var winners = playerAnswers
            .Where(x => x.GuessedNumber == bestGuessedNumber)
            .ToArray();

        return new RoundResult(bestGuessedNumber, expectedNumber, winners);
    }

    private bool IsFinished() => _roundsLeft == 0;

    private void NextRound()
    {
        if (!IsFinished())
        {
            _roundsLeft--;
        }
    }

    private GameResult GetResult() => new(_playerScores);

    private void NotifyAboutPlayerAnswers(PlayerAnswer[] playerAnswers) =>
        Array.ForEach(playerAnswers, x => _notifier.Notify($"Ответ игрока {x.Player.Name}: {x.GuessedNumber}"));

    private void NotifyAboutRoundResult(RoundResult roundResult) =>
        _notifier.Notify($"""

                          Загаданное число: {roundResult.ExpectedNumber}
                          Лучший ответ: {roundResult.BestGuessedNumber}
                          Победители раунда:
                          {string.Join(Environment.NewLine, roundResult.Winners.Select(x => x.Player.Name))}

                          """);

    private void NotifyAboutGameResult(GameResult gameResult) =>
        _notifier.Notify($"""
                          Победители игры:
                          {string.Join(Environment.NewLine, gameResult.Winners.Select(x => x.Name))}

                          {string.Join(Environment.NewLine,
                              gameResult.PlayerScores.Select(x => $"Игрок {x.Key.Name} имеет результат {x.Value}."))}
                          """);

    // Для потенциального переопределения логики и юнит-тестирования можно вынести в интерфейс
    private static int CalculateClosest(int expectedNumber, int number1, int number2)
    {
        var number1Diff = Math.Abs(expectedNumber - number1);
        var number2Diff = Math.Abs(expectedNumber - number2);

        return number1Diff < number2Diff ? number1 : number2;
    }
}
