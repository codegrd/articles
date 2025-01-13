namespace FrameworkGame.GuessNumberGame;

// Удобное апи для создания нотификаторов
public static class Notifiers
{
    public static INotifier Console() => new ConsoleNotifier();
}
