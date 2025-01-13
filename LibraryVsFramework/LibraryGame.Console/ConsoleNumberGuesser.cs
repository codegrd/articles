namespace LibraryGame.Console;

internal sealed class ConsoleNumberGuesser(string playerName) : INumberGuesser
{
    public int Guess((int left, int right) range)
    {
        if (string.IsNullOrWhiteSpace(playerName))
        {
            throw new ArgumentException("Player name cannot be null or empty", nameof(playerName));
        }

        var (left, right) = range;

        while (true)
        {
            System.Console.Write($"{playerName}, введите число в диапазоне от {left} до {right}: ");
            var input = System.Console.ReadLine();

            if (!TryParseNumber(input, out int number))
            {
                continue;
            }

            if (!IsWithinRange(number))
            {
                continue;
            }

            return number;
        }

        bool TryParseNumber(string? input, out int number)
        {
            if (int.TryParse(input, out number))
            {
                return true;
            }

            System.Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
            return false;
        }

        bool IsWithinRange(int number)
        {
            if (number >= left && number <= right)
            {
                return true;
            }

            System.Console.WriteLine($"Число должно быть в диапазоне от {left} до {right}.");
            return false;
        }
    }
}
