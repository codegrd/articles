namespace FrameworkGame.GuessNumberGame;

public sealed class GuessNumberGameBuilder
{
    private int _rounds = 1;

    private readonly HashSet<Player> _players = [];

    private (int left, int right) _range = (0, 100);

    private INotifier _notifier = new EmptyNotifier();

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

    public GuessNumberGameBuilder AddPlayer(Player player)
    {
        _players.Add(player);

        return this;
    }

    public GuessNumberGameBuilder AddPlayers(params Player[] players)
    {
        if (players == null || players.Length == 0)
        {
            throw new ArgumentException("Player collection cannot be null or empty", nameof(players));
        }

        foreach (var name in players)
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

    public GuessNumberGameBuilder WithNotifier(INotifier notifier)
    {
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

        return this;
    }

    public GuessNumberGame Build()
    {
        if (_rounds <= 0)
        {
            throw new InvalidOperationException("Rounds must be set and greater than 0");
        }

        if (_players.Count < 2)
        {
            throw new InvalidOperationException("There must be at least 2 players");
        }

        return new GuessNumberGame(_notifier, _rounds, _range, _players);
    }
}
