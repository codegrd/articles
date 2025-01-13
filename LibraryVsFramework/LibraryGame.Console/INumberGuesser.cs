namespace LibraryGame.Console;

// Интерфейс определён в вызывающем коде. Можно было и без него, но ввели для удобства
internal interface INumberGuesser
{
    public int Guess((int left, int right) range);
}
