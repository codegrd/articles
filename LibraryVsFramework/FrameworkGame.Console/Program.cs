using FrameworkGame.GuessNumberGame;

var game = GuessNumberGameBuilder
    .Create()
    .AddPlayers(Players.Console("Human"), Players.Computer("Computer")) // Регистрируем игроков
    .WithNotifier(Notifiers.Console()) // Регистрируем нотификатор о событиях игры
    .WithRounds(3)
    .Build();

// Вся логика происходит внутри
game.Start();
