namespace LibraryGame.GuessNumberGame;

public sealed record PlayerAnswer(string PlayerName, int GuessedNumber)
{
    public bool Equals(PlayerAnswer? other)
    {
        return other is not null && PlayerName == other.PlayerName;
    }

    public override int GetHashCode()
    {
        return PlayerName.GetHashCode();
    }
}
