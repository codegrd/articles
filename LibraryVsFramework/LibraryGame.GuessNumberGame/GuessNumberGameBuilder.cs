namespace LibraryGame.GuessNumberGame;

public sealed class GuessNumberGameBuilder
{
    private int _rounds = 1;

    private readonly HashSet<string> _playerNames = [];

    private (int left, int right) _range = (10, 100);

    private GuessNumberGameBuilder()
    {
    }

    public static GuessNumberGameBuilder Create() => new();

    public GuessNumberGameBuilder WithRounds(int rounds)
    {
        if (rounds <= 0)
        {
            throw new ArgumentException("Rounds must be positive", nameof(rounds));
        }

        _rounds = rounds;

        return this;
    }

    public GuessNumberGameBuilder AddPlayer(string playerName)
    {
        _playerNames.Add(playerName);

        return this;
    }

    public GuessNumberGameBuilder AddPlayers(params string[] playerNames)
    {
        if (playerNames == null || playerNames.Length == 0)
        {
            throw new ArgumentException("Player names collection cannot be null or empty", nameof(playerNames));
        }

        foreach (var name in playerNames)
        {
            AddPlayer(name);
        }

        return this;
    }

    public GuessNumberGameBuilder WithRange(int left, int right)
    {
        if (right <= left)
        {
            throw new ArgumentException("Right number must be greater than left", nameof(right));
        }

        _range = (left, right);

        return this;
    }

    public GuessNumberGame StartGame()
    {
        if (_rounds <= 0)
        {
            throw new InvalidOperationException("Rounds must be set and greater than 0");
        }

        if (_playerNames.Count < 2)
        {
            throw new InvalidOperationException("There must be at least 2 players");
        }

        return new GuessNumberGame(_rounds, _range, _playerNames);
    }
}
