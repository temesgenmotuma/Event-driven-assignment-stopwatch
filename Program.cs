using System;
using System.Threading;

public delegate void StopwatchEventHandler(string message);

public class Stopwatch
{
    private DateTime _startTime;
    private TimeSpan _elapsedTime;
    private bool _isRunning;

    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    public TimeSpan TimeElapsed => _isRunning ? DateTime.Now - _startTime + _elapsedTime : _elapsedTime;
    public bool IsRunning => _isRunning;

    public void Start()
    {
        if (!_isRunning)
        {
            _startTime = DateTime.Now;
            _isRunning = true;
            OnStarted?.Invoke("Stopwatch Started!");
        }
    }

    public void Stop()
    {
        if (_isRunning)
        {
            _elapsedTime += DateTime.Now - _startTime;
            _isRunning = false;
            OnStopped?.Invoke("Stopwatch Stopped!");
        }
    }

    public void Reset()
    {
        _isRunning = false;
        _elapsedTime = TimeSpan.Zero;
        OnReset?.Invoke("Stopwatch Reset!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.OnStarted += message => DisplayMessage(message);
        stopwatch.OnStopped += message => DisplayMessage(message);
        stopwatch.OnReset += message => DisplayMessage(message);

        string lastMessage = string.Empty;

        void DisplayMessage(string message)
        {
            lastMessage = message;
        }

        Console.WriteLine("Console based Stopwatch Application\nCommands: S to Start, T to Stop, R to Reset, Q to Quit\n");

        bool quit = false;

        while (!quit)
        {
            Console.Clear();
            Console.WriteLine("Console based Stopwatch Application\nCommands: S to Start, T to Stop, R to Reset, Q to Quit\n");
            Console.WriteLine($"Time Elapsed: {stopwatch.TimeElapsed:hh\\:mm\\:ss}");

            if (!string.IsNullOrEmpty(lastMessage))
            {
                Console.WriteLine($"\n{lastMessage}");
            }

            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                switch (key)
                {
                    case ConsoleKey.S:
                        stopwatch.Start();
                        break;
                    case ConsoleKey.T:
                        stopwatch.Stop();
                        break;
                    case ConsoleKey.R:
                        stopwatch.Reset();
                        break;
                    case ConsoleKey.Q:
                        quit = true;
                        stopwatch.Stop();
                        Console.WriteLine("Exiting application");
                        break;
                    default:
                        Console.WriteLine("Incorrect command entered. Please use S, T, R, or Q.");
                        break;
                }
            }

            Thread.Sleep(100);
        }
    }
}
