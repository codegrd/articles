namespace FrameworkGame.GuessNumberGame;

// Удобное апи для создания игроков
public static class Players
{
    public static Player Console(string name) => new(name, new ConsoleNumberGuesser(name));

    public static Player Computer(string name) => new(name, new RandomNumberGuesser());

    public static Player Custom(string name, INumberGuesser numberGuesser) => new(name, numberGuesser);
}
