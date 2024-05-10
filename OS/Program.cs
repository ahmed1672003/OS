internal class Program
{
    static void Main(string[] args)
    {
        List<VirtualProcess> processes = new() { new(0, 10) };
        // FCFSScheduler.CalcFinishTime(processes);
        RoundRobinScheduler.ExecuteRoundRobin(processes, 2);
        foreach (var item in processes)
        {
            Console.WriteLine(item.FinishTime);
        }
    }
}

public class FCFSScheduler
{
    public static void CalcFinishTime(List<VirtualProcess> processes)
    {
        processes.ForEach(process =>
        {
            process.FinishTime =
                processes.Where(x => x.ArrivalTime < process.ArrivalTime).Sum(x => x.BurstTime)
                + process.BurstTime;
        });
    }

    public static void CalcTurnAroundTime(List<VirtualProcess> processes)
    {
        processes.ForEach(process =>
        {
            process.ArrivalTime = process.FinishTime - process.ArrivalTime;
        });
    }

    public static void CalcWaitingTime(List<VirtualProcess> processes)
    {
        processes.ForEach(process =>
        {
            process.WaitingTime = process.TurnAroundTime - process.BurstTime;
        });
    }
}

public class RoundRobinScheduler
{
    public static void ExecuteRoundRobin(List<VirtualProcess> processes, int timeQuantum)
    {
        int currentTime = 0;
        int totalWaitingTime = 0;
        int totalTurnaroundTime = 0;

        Queue<VirtualProcess> processQueue = new Queue<VirtualProcess>(processes);

        while (processQueue.Count > 0)
        {
            VirtualProcess currentProcess = processQueue.Dequeue();

            // Execute current process for the time quantum or remaining burst time, whichever is smaller
            int executionTime = Math.Min(timeQuantum, currentProcess.BurstTime);
            currentProcess.BurstTime -= executionTime;
            currentTime += executionTime;

            // Update waiting time for all processes in the queue
            foreach (var process in processQueue)
            {
                process.WaitingTime += executionTime;
            }

            // If current process still has burst time left, enqueue it back to the end of the queue
            if (currentProcess.BurstTime > 0)
            {
                processQueue.Enqueue(currentProcess);
            }
            else
            {
                // Calculate turnaround time for the completed process
                currentProcess.TurnAroundTime = currentTime - currentProcess.ArrivalTime;
                totalWaitingTime += currentProcess.WaitingTime;
                totalTurnaroundTime += currentProcess.TurnAroundTime;
            }
        }

        // Calculate average time
        double averageWaitingTime = (double)totalWaitingTime / processes.Count;
        double averageTurnaroundTime = (double)totalTurnaroundTime / processes.Count;

        // Assign average time to each process (for display purpose)
        foreach (var process in processes)
        {
            process.AverageTime = averageTurnaroundTime;
        }
    }
}

public class VirtualProcess
{
    public VirtualProcess(int arrivalTime, int burstTime)
    {
        ArrivalTime = arrivalTime;
        BurstTime = burstTime;
    }

    public int ArrivalTime { get; set; }
    public int BurstTime { get; set; }
    public int WaitingTime { get; set; }
    public int TurnAroundTime { get; set; }
    public int FinishTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public double AverageTime { get; set; }
}
