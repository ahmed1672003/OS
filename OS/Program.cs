using OS;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Fork fork = new();
        new Philosopher(0, 1, 1, fork);
        new Philosopher(1, 2, 2, fork);
        new Philosopher(2, 3, 3, fork);
        new Philosopher(3, 4, 4, fork);
        new Philosopher(4, 5, 5, fork);
        new Philosopher(5, 5, 5, fork);
        new Philosopher(6, 5, 5, fork);
        new Philosopher(7, 5, 5, fork);
        new Philosopher(8, 5, 5, fork);
        new Philosopher(9, 5, 5, fork);
        new Philosopher(10, 5, 5, fork);
    }
}
