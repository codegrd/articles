namespace FrameworkGame.GuessNumberGame;

// При желании можно реализовать кастомный нотификатор
public interface INotifier
{
    // В идеале, нужно сделать несколько методов, которые принимают объекты, а строку формируют внутри себя.
    // Но для демонстрации сойдёт.
    public void Notify(string text);
}
