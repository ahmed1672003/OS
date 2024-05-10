internal class Program
{
    static void Main(string[] args)
    {
        int[] availableResources = new int[] { 3, 3, 2 };
        int[,] maximumResourcesCanBeAllocated = new int[,]
        {
            { 7, 5, 3 },
            { 3, 2, 2 },
            { 9, 0, 2 },
            { 2, 2, 2 },
            { 4, 3, 3 }
        };
        int[,] allocationResources = new int[,]
        {
            { 0, 1, 0 },
            { 2, 0, 0 },
            { 3, 0, 2 },
            { 2, 1, 1 },
            { 0, 0, 2 }
        };

        BankersAlgorithm bankersAlgorithm = new BankersAlgorithm(
            availableResources,
            maximumResourcesCanBeAllocated,
            allocationResources
        );

        if (bankersAlgorithm.IsSafeState(out List<int> safeSequence))
        {
            Console.WriteLine(
                $"system is save state with sequance: {string.Join(" => ", safeSequence)} "
            );
        }
        else
        {
            Console.WriteLine(
                $"system is not save with sequance and save sequance: {string.Join(" => ", safeSequence)} "
            );
        }
    }
}

class BankersAlgorithm
{
    // Available resources
    int[] _availableResources;

    // Maximum resources that can be allocated to processes
    int[,] _maximumResourcesCanBeAllocated;

    // Resources currently allocated to processes
    int[,] _allocationResources;

    // Remaining needs of processes
    int[,] _need;

    public BankersAlgorithm(
        int[] availableResources,
        int[,] maximumResourcesCanBeAllocated,
        int[,] allocationResources
    )
    {
        _availableResources = availableResources;
        _maximumResourcesCanBeAllocated = maximumResourcesCanBeAllocated;
        _allocationResources = allocationResources;
        _need = new int[allocationResources.GetLength(1), _availableResources.Length];

        // Calculate remaining needs of processes
        for (int i = 0; i < _allocationResources.GetLength(1); i++)
        {
            for (int j = 0; j < _availableResources.Length; j++)
            {
                _need[i, j] = maximumResourcesCanBeAllocated[i, j] - allocationResources[i, j];
            }
        }
    }

    public bool IsSafeState(out List<int> safeSequence)
    {
        safeSequence = new List<int>();
        int[] work = new int[_availableResources.Length];
        int[] finish = new int[_allocationResources.GetLength(1)];

        // Initialize work and finish arrays
        for (int i = 0; i < _availableResources.Length; i++)
        {
            work[i] = _availableResources[i];
        }

        for (int i = 0; i < _allocationResources.GetLength(1); i++)
        {
            finish[i] = 0;
        }

        int count = 0;
        while (count < _allocationResources.GetLength(1))
        {
            bool found = false;
            for (int i = 0; i < _allocationResources.GetLength(1); i++)
            {
                if (finish[i] == 0)
                {
                    int j;
                    for (j = 0; j < _availableResources.Length; j++)
                    {
                        if (_need[i, j] > work[j])
                        {
                            break;
                        }
                    }

                    if (j == _availableResources.Length)
                    {
                        // Process can be allocated resources
                        for (int k = 0; k < _availableResources.Length; k++)
                        {
                            work[k] += _allocationResources[i, k];
                        }

                        finish[i] = 1;
                        found = true;
                        count++;
                        safeSequence.Add(i);
                    }
                }
            }

            // If no process could be allocated resources, the system is in an unsafe state
            if (!found)
            {
                return false;
            }
        }

        return true;
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
