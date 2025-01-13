using System.Collections.Immutable;

namespace LibraryGame.GuessNumberGame;

public sealed record GameResult
{
    private string[]? _winners;

    public string[] Winners => _winners ??= DetermineWinners();
    public IReadOnlyDictionary<string, int> PlayerScores { get; }

    public bool IsFinished { get; private set; }

    private GameResult(bool isFinished, IReadOnlyDictionary<string, int> playerScores)
    {
        IsFinished = isFinished;
        PlayerScores = playerScores;
    }

    internal static GameResult Finished(IReadOnlyDictionary<string, int> playerScores)
    {
        return new GameResult(true, playerScores);
    }

    internal static GameResult NotFinished(IReadOnlyDictionary<string, int> playerScores)
    {
        return new GameResult(false, playerScores);
    }

    private string[] DetermineWinners()
    {
        return PlayerScores.Any()
            ? PlayerScores
                .GroupBy(x => x.Value)
                .OrderByDescending(x => x.Key)
                .First()
                .Select(x => x.Key)
                .ToArray()
            : [];
    }
}
