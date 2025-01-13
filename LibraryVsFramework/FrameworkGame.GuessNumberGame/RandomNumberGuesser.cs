namespace FrameworkGame.GuessNumberGame;

internal sealed class RandomNumberGuesser: INumberGuesser
{
    private readonly Random _random = new();

    public int Guess((int left, int right) range) => _random.Next(range.left, range.right);
}
