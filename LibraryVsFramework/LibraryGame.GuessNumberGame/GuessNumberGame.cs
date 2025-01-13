namespace LibraryGame.GuessNumberGame;

public sealed class GuessNumberGame
{
    private int _roundsLeft;

    private readonly Dictionary<string, int> _playerScores;

    // Можно скрыть за интерфейсом и дать возможность переопределить снаружи
    private readonly Random _random;

    public (int left, int right) Range { get; }

    internal GuessNumberGame(int rounds, (int left, int right) range, IReadOnlyCollection<string> playerNames)
    {
        ArgumentNullException.ThrowIfNull(playerNames);

        if (rounds <= 0)
        {
            throw new ArgumentException("Rounds must be positive", nameof(rounds));
        }

        if (range.right <= range.left)
        {
            throw new ArgumentException("Right number must be greater than left", nameof(range));
        }

        if (playerNames.Count < 2)
        {
            throw new ArgumentException("There must be at least 2 players", nameof(playerNames));
        }

        _roundsLeft = rounds;
        Range = range;
        _playerScores = playerNames.ToDictionary(x => x, _ => 0);
        _random = new Random();
    }

    public RoundResult NextRound(ICollection<PlayerAnswer> playerAnswers)
    {
        if (IsFinished())
        {
            return new RoundResult(0, 0, []);
        }

        if (playerAnswers.ToHashSet().Count != _playerScores.Count)
        {
            throw new ArgumentException("There must be the same number of players");
        }

        if (playerAnswers.Any(x => !_playerScores.ContainsKey(x.PlayerName)))
        {
            throw new ArgumentException("One of players does not exist");
        }

        var expectedNumber = _random.Next(Range.left, Range.right + 1);

        var roundResult = DetermineRoundWinners(expectedNumber, playerAnswers);

        Array.ForEach(roundResult.Winners.ToArray(), x => _playerScores[x.PlayerName]++);

        NextRound();

        return roundResult;
    }

    public GameResult GetResult()
    {
        return IsFinished() ? GameResult.Finished(_playerScores) : GameResult.NotFinished(_playerScores);
    }

    public bool IsFinished()
    {
        return _roundsLeft == 0;
    }

    private void NextRound()
    {
        if (!IsFinished())
        {
            _roundsLeft--;
        }
    }

    // Можно гипотетически вынести в интерфейс, чтобы была возможность переопределить логику игры и написать юнит-тесты
    // PlayerAnswer можно сделать интерфейсом
    private RoundResult DetermineRoundWinners(int expectedNumber, ICollection<PlayerAnswer> playerAnswers)
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

    // Для потенциального переопределения логики и юнит-тестирования можно вынести в интерфейс
    private static int CalculateClosest(int expectedNumber, int number1, int number2)
    {
        var number1Diff = Math.Abs(expectedNumber - number1);
        var number2Diff = Math.Abs(expectedNumber - number2);

        return number1Diff < number2Diff ? number1 : number2;
    }
}
