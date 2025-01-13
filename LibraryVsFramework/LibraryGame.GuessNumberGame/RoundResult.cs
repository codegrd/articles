namespace LibraryGame.GuessNumberGame;

public sealed record RoundResult(int BestGuessedNumber, int ExpectedNumber, IReadOnlyCollection<PlayerAnswer> Winners);
