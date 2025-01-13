using LibraryGame.Console;
using LibraryGame.GuessNumberGame;

// Инициализируем определённое количество игроков
var humanPlayer = "Человек";
var computerPlayer = "Компьютер";

// Определяем, кто игрок, кто компьютер
var numberGuessers = new Dictionary<string, INumberGuesser>
{
    { humanPlayer, new ConsoleNumberGuesser(humanPlayer) },
    { computerPlayer, new RandomNumberGuesser() },
};

// Создаём игру, используя апи библиотеки
var game = GuessNumberGameBuilder
    .Create()
    .WithRange(0, 100)
    .WithRounds(3)
    .AddPlayers(humanPlayer, computerPlayer) // Добавляем лишь имена игроков для подсчёта, саму логику не регистриуем
    .StartGame();

RoundResult roundResult;

while (!game.IsFinished())
{
    roundResult = game.NextRound( // Сами управляем выполнением каждого раунда
        numberGuessers.Select(x => // Сами передаём в игру ответы игроков
        {
            var guessed = x.Value.Guess(game.Range); // Руками снаружи вызываем угадывания и выводим в консоль
            Console.WriteLine($"Ответ игрока {x.Key}: {guessed}");

            return new PlayerAnswer(x.Key, guessed);
        }).ToArray()
    );

    WriteRoundResult(roundResult); // Выводим результат раунда
}

WriteGameResult(game.GetResult()); // Выводим результат игры

return;

void WriteRoundResult(RoundResult roundResult)
{
    Console.WriteLine($"Загаданное число: {roundResult.ExpectedNumber}");
    Console.WriteLine($"Лучший ответ: {roundResult.BestGuessedNumber}");
    Console.WriteLine("Победители раунда:");
    Console.WriteLine(string.Join(Environment.NewLine, roundResult.Winners.Select(x => x.PlayerName)));
    Console.WriteLine();
}

void WriteGameResult(GameResult gameResult)
{
    Console.WriteLine($"Победители игры:{Environment.NewLine}{string.Join(Environment.NewLine, gameResult.Winners)}");
    Console.WriteLine();
    Console.WriteLine(string.Join(Environment.NewLine,
        gameResult.PlayerScores.Select(x => $"Игрок {x.Key} имеет результат {x.Value}.")));
}
