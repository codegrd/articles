# Библиотека vs Фреймворк

**Библиотека** и **фреймворк** — это два разных подхода к организации кода и инструментов в программировании. 
Основное различие между ними — в том, _кто управляет исполнением кода_.

При использовании библиотеки именно **клиент** управляет потоком программы, просто вызывая библиотечные функции в определённом порядке.

Фреймворк же берёт управление на себя. Это называется [Inversion of Control (IoC)](https://ru.wikipedia.org/wiki/%D0%98%D0%BD%D0%B2%D0%B5%D1%80%D1%81%D0%B8%D1%8F_%D1%83%D0%BF%D1%80%D0%B0%D0%B2%D0%BB%D0%B5%D0%BD%D0%B8%D1%8F).
Именно он определяет всю последовательность вызовов и даёт клиенту лишь возможность встроить свои методы (функции, коллбэки) на специально подготовленные места.

_Ты вызываешь код библиотеки, фреймворк вызывает твой код._

Данный репозиторий на примере игры с угадыванием числа из диапазона демонстрирует разницу межу библиотекой и фреймворком.

Игра поддерживает настройку количества игроков (с интерфейсом через консоль и компьютерных с автогенерацией),
диапазона чисел для угадывания, а также количества раундов.

В решении 4 проекта:

| Проект                        |                                                    |
|-------------------------------|----------------------------------------------------|
| FrameworkGame.GuessNumberGame | Фреймворк, реализующий игру "Угадай число".        |
| FrameworkGame.Console         | Клиент фреймворка, запускающий игру.               |
|||
| LibraryGame.GuessNumberGame   | Библиотека для создания игры "Угадай число".       |
| LibraryGame.Console           | Клиент, использующий библиотеку для создания игры. |

## Библиотека

Библиотека предоставляет классы и методы для инициализации (`GuessNumberGameBuilder`) и запуска раунда (`GuessNumberGame.NextRound`), а также внутри себя подсчитывает результаты.

### Библиотечный код раунда (проверки удалены):
```csharp
public RoundResult NextRound(ICollection<PlayerAnswer> playerAnswers)
{
    var expectedNumber = _random.Next(Range.left, Range.right + 1);

    var roundResult = DetermineRoundWinners(expectedNumber, playerAnswers);

    Array.ForEach(roundResult.Winners.ToArray(), x => _playerScores[x.PlayerName]++);

    NextRound();

    return roundResult;
}
```

Ход игры, а так же реакцию на результаты реализует клиент.

### Код клиента:
```csharp
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
```

## Фреймворк

Фреймворк сам исполняет всю игру (`GuessNumberGame.Start`), предоставляя возможность зарегистрировать обработчики событий (`GuessNumberGameBuilder`) или добавить кастомную функциональность игрока (`INotifier` и `INumberGuesser`).
Также фреймворк предоставляет свои классы для базовой функциональности (`GuessNumberGame.Notifiers` и `FrameworkGame.GuessNumberGame.Players`).

### Код игры во фреймворке:
```csharp
// В случае с фреймворком, он сам выполняет всю логику.
// Возвращаемое значение можно добавить, например, чтобы снаружи отследить корректность завершения.
public void Start()
{
    while (!IsFinished())
    {
        var playerAnswers = _players.Select(x => new PlayerAnswer(x, x.GuessNumber(_range))).ToArray();

        NotifyAboutPlayerAnswers(playerAnswers); // Внутри вызывается зарегистрированный нотификатор, пишущий в консоль

        var expectedNumber = _random.Next(_range.left, _range.right + 1);

        var roundResult = DetermineRoundResult(expectedNumber, playerAnswers);

        Array.ForEach(roundResult.Winners.ToArray(), x => _playerScores[x.Player]++);

        NotifyAboutRoundResult(roundResult);

        NextRound();
    }

    var gameResult = GetResult();

    NotifyAboutGameResult(gameResult);
}
```

Клиент занимается лишь регистрацией функциональности во фреймворке и никак не управляет исполнением кода.

### Код клиента:
```csharp
var game = GuessNumberGameBuilder
    .Create()
    .AddPlayers(Players.Console("Human"), Players.Computer("Computer")) // Регистрируем игроков
    .WithNotifier(Notifiers.Console()) // Регистрируем нотификатор о событиях игры
    .WithRounds(3)
    .Build();

// Вся логика происходит внутри
game.Start();
```

## Выводы

Фреймворк позволяет обеспечивать хорошую целостность игры, не давая неправильно использовать его код.

При использовании библиотеки клиент сам определяет логику, из-за чего она может быть реализована с ошибками, но зато он получает возможность гибко настраивать всю логику, поскольку сам управляет всем процессом.

Фреймворк ограничивает кастомизацию логики только теми "дырками", которые определил сам, что даёт минус к гибкости, но плюс к консистентности.

Код клиента фреймворка намного короче, чем код клиента библиотеки, так как весь флоу реализован в нём.

Но из-за того, что фреймворк берёт исполнение на себя, могут возникнуть трудности в его интеграции с другими фреймворками.
Например, в текущем примере достаточно сложно интегрировать фреймворк игры с контроллерами фреймворка ASP .Net (вероятно, пришлось бы использовать `System.Threading.Channels` для передачи информации из запросов в код игрока).
