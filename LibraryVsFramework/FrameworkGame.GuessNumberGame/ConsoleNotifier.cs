namespace FrameworkGame.GuessNumberGame;

internal sealed class ConsoleNotifier : INotifier
{
    public void Notify(string text)
    {
        Console.WriteLine(text);
    }
}
