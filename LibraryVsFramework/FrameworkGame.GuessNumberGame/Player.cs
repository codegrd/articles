namespace FrameworkGame.GuessNumberGame;

public sealed record Player
{
    public string Name { get; }

    private readonly INumberGuesser _guesser;

    internal Player(string name, INumberGuesser guesser)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Player name cannot be null or empty", nameof(name));
        }

        Name = name;
        _guesser = guesser ?? throw new ArgumentNullException(nameof(guesser));
    }

    public int GuessNumber((int left, int right) range)
    {
        return _guesser.Guess(range);
    }

    public bool Equals(Player? other)
    {
        return other != null && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
