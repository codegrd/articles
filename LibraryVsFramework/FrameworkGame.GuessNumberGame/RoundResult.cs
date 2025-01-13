namespace FrameworkGame.GuessNumberGame;

internal sealed record RoundResult(int BestGuessedNumber, int ExpectedNumber, IReadOnlyCollection<PlayerAnswer> Winners);
