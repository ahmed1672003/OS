namespace OS;

public class Lab5
{
    public static void CalcWatingTime(int[] processes, int n, int[] bt, int[] wt)
    {
        wt[0] = 0;
        for (int i = 1; i < n; i++)
        {
            wt[i] = bt[i - 1] + wt[i - 1];
        }
    }

    public static void CalcTurnAroundTime(int[] processes, int n, int[] bt, int[] wt, int[] tat)
    {
        for (int i = 0; i < n; i++)
        {
            tat[i] = bt[i] + wt[i];
        }
    }

    public static void CalcAverageTime(int[] processes, int n, int[] bt)
    {
        int[] wt = new int[n];
        int[] tat = new int[n];
        int total_wt = 0,
            total_tat = 0;
        CalcWatingTime(processes, n, bt, wt);
        CalcTurnAroundTime(processes, n, bt, wt, tat);
        Console.Write("Processes Burst time Waiting" + " time Turn around time\n");
        for (int i = 0; i < n; i++)
        {
            total_wt = total_wt + wt[i];
            total_tat = total_tat + tat[i];
            Console.Write(" {0} ", (i + 1));
            Console.Write(" {0} ", bt[i]);
            Console.Write(" {0}", wt[i]);
            Console.Write(" {0}\n", tat[i]);
        }
        float s = (float)total_wt / (float)n;
        int t = total_tat / n;
        Console.Write("Average waiting time = {0}", s);
        Console.Write("\n");
        Console.Write("Average turn around time = {0} ", t);
    }
}
