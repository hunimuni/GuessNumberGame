using System;

public interface IGameSettings
{
    int MinNum { get; }
    int MaxNum { get; }
    int MaxAttempts { get; }
}

public interface INumberGenerator
{
    int GenerateNumber();
}

public interface IGame
{
    int NumberToGuess { get; }
    int MaxAttempts { get; }
    int Attempts { get; }
    bool GuessNumber(int number);
}

public class GameSettings : IGameSettings
{
    public int MinNum { get; }
    public int MaxNum { get; }
    public int MaxAttempts { get; }

    public GameSettings(int minNum, int maxNum, int maxAttempts)
    {
        MinNum = minNum;
        MaxNum = maxNum;
        MaxAttempts = maxAttempts;
    }
}

public class NumberGenerator : INumberGenerator
{
    private readonly Random random = new Random();
    private readonly int minNum;
    private readonly int maxNum;

    public NumberGenerator(int minNum, int maxNum)
    {
        this.minNum = minNum;
        this.maxNum = maxNum;
    }

    public int GenerateNumber()
    {
        return random.Next(minNum, maxNum + 1);
    }
}

public class Game : IGame
{
    public int NumberToGuess { get; }
    public int MaxAttempts { get; }
    public int Attempts { get; private set; }

    public Game(int numberToGuess, int maxAttempts)
    {
        NumberToGuess = numberToGuess;
        MaxAttempts = maxAttempts;
    }

    public bool GuessNumber(int number)
    {
        Attempts++;
        if (number == NumberToGuess)
        {
            return true;
        }
        else if (number < NumberToGuess)
        {
            Console.WriteLine("Загаданное число больше");
        }
        else
        {
            Console.WriteLine("Загаданное число меньше");
        }
        return false;
    }
}

public class GameUI
{
    private readonly IGame game;

    public GameUI(IGame game)
    {
        this.game = game;
    }

    public void Play()
    {
        while (game.Attempts < game.MaxAttempts)
        {
            Console.Write("Ваш вариант: ");
            int number = Convert.ToInt32(Console.ReadLine());
            if (game.GuessNumber(number))
            {
                Console.WriteLine("Поздравляем! Вы угадали число.");
                return;
            }
        }
        Console.WriteLine("Вы не угадали. Было загадано число: " + game.NumberToGuess);
    }
}

class Program
{
    static void Main()
    {
        IGameSettings gameSettings = new GameSettings(1, 100, 10);
        Console.WriteLine($"Угадайте число от {gameSettings.MinNum} до {gameSettings.MaxNum} за {gameSettings.MaxAttempts} попыток.");

        INumberGenerator numberGenerator = new NumberGenerator(gameSettings.MinNum, gameSettings.MaxNum);
        int numberToGuess = numberGenerator.GenerateNumber();

        IGame game = new Game(numberToGuess, gameSettings.MaxAttempts);
        GameUI gameUI = new GameUI(game);
        gameUI.Play();
    }
}