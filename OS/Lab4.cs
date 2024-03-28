namespace OS;

public class Fork
{
    bool[] fork = new bool[5];

    public void Get(int left, int right)
    {
        lock (this)
        {
            while (fork[left] || fork[right])
                Monitor.Wait(this);
            fork[left] = true;
            fork[right] = true;
        }
    }

    public void Put(int left, int right)
    {
        lock (this)
        {
            fork[left] = false;
            fork[right] = false;
            Monitor.PulseAll(this);
        }
    }
}

public class Philosopher
{
    int n;
    int thinkDelay;
    int eatDelay;
    int left,
        right;

    Fork fork;

    public Philosopher(int n, int thinkDelay, int eatDelay, Fork fork)
    {
        this.n = n;
        this.thinkDelay = thinkDelay;
        this.eatDelay = eatDelay;
        this.fork = fork;
        this.left = n == 0 ? 4 : (n - 1) % 5;
        this.right = (n) % 5;
        new Thread(new ThreadStart(Run)).Start();
    }

    public void Run()
    {
        while (true)
        {
            try
            {
                Thread.Sleep(thinkDelay);
                fork.Get(left, right);
                Console.WriteLine("Philosopher " + n + " is eating...");
                Console.ReadLine();
                Thread.Sleep(eatDelay);
                fork.Put(left, right);
            }
            catch
            {
                return;
            }
        }
    }
}
