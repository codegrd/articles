using System.Collections.Immutable;

namespace FrameworkGame.GuessNumberGame;

public sealed record GameResult
{
    public IReadOnlyCollection<Player> Winners { get; }

    public IReadOnlyDictionary<Player, int> PlayerScores { get; }

    internal GameResult(IReadOnlyDictionary<Player, int> playerScores)
    {
        PlayerScores = playerScores;
        Winners = DetermineWinners();
    }

    private Player[] DetermineWinners()
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
