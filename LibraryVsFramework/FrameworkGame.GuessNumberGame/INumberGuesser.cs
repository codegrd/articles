namespace FrameworkGame.GuessNumberGame;

// Интерфейс для угадашек предоставляется фреймворком (в проекте с ним)
// При желании можно реализовать кастомную угадашку
public interface INumberGuesser
{
    public int Guess((int left, int right) range);
}
