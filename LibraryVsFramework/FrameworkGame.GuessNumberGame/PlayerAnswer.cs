namespace FrameworkGame.GuessNumberGame;

internal sealed record PlayerAnswer(Player Player, int GuessedNumber)
{
    public bool Equals(PlayerAnswer? other)
    {
        return other is not null && Equals(Player, other.Player);
    }

    public override int GetHashCode()
    {
        return Player.GetHashCode();
    }
}
