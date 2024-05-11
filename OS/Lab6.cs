using System.Text;

namespace OS;

public class MemoryManagement
{
    public MemoryManagement(List<int> blockSize, List<int> processesSize)
    {
        BlockSize = blockSize;
        ProcessesSize = processesSize;
    }

    public List<int> BlockSize { get; private set; } = new();
    public List<int> ProcessesSize { get; private set; } = new();

    public void FirstFit()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("manage memory with first-fit: ");
        stringBuilder.AppendLine("---------------");
        stringBuilder.AppendLine("P.N | P.S | B.N");
        stringBuilder.AppendLine("---------------");
        ProcessesSize.ForEach(processSize =>
        {
            int targetBlock = BlockSize.FirstOrDefault(x => x >= processSize);
            if (targetBlock != 0 && targetBlock >= processSize)
            {
                int targetBlockIndex = BlockSize.IndexOf(targetBlock);
                BlockSize[targetBlockIndex] -= processSize;
                stringBuilder.AppendLine(
                    $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | {targetBlockIndex + 1}"
                );
            }
            else
            {
                stringBuilder.AppendLine(
                    $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | Not Allocated"
                );
            }
        });

        Console.WriteLine(stringBuilder);
    }

    public void BestFit()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("manage memory with best-fit: ");
        stringBuilder.AppendLine("---------------");
        stringBuilder.AppendLine("P.N | P.S | B.N");
        stringBuilder.AppendLine("---------------");
        ProcessesSize.ForEach(processSize =>
        {
            var avilableBlocks = BlockSize.Where(x => x >= processSize);

            if (avilableBlocks != null && avilableBlocks.Any())
            {
                int targetBlock = avilableBlocks.Min();
                if (targetBlock != 0 && targetBlock >= processSize)
                {
                    int targetBlockIndex = BlockSize.IndexOf(targetBlock);
                    BlockSize[targetBlockIndex] -= processSize;
                    stringBuilder.AppendLine(
                        $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | {targetBlockIndex + 1}"
                    );
                }
                else
                {
                    stringBuilder.AppendLine(
                        $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | Not Allocated"
                    );
                }
            }
            else
            {
                stringBuilder.AppendLine(
                    $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | Not Allocated"
                );
            }
        });
        Console.WriteLine(stringBuilder);
    }

    public void WorstFit()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("manage memory with worst-fit: ");
        stringBuilder.AppendLine("---------------");
        stringBuilder.AppendLine("P.N | P.S | B.N");
        stringBuilder.AppendLine("---------------");
        ProcessesSize.ForEach(processSize =>
        {
            var avilableBlocks = BlockSize.Where(x => x >= processSize);

            if (BlockSize.Where(x => x >= processSize) != null && avilableBlocks.Any())
            {
                int targetBlock = avilableBlocks.Max();
                if (targetBlock != 0 && targetBlock >= processSize)
                {
                    int targetBlockIndex = BlockSize.IndexOf(targetBlock);
                    BlockSize[targetBlockIndex] -= processSize;
                    stringBuilder.AppendLine(
                        $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | {targetBlockIndex + 1}"
                    );
                }
                else
                {
                    stringBuilder.AppendLine(
                        $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | Not Allocated"
                    );
                }
            }
            else
            {
                stringBuilder.AppendLine(
                    $"  {ProcessesSize.IndexOf(processSize) + 1} | {processSize} | Not Allocated"
                );
            }
        });

        Console.WriteLine(stringBuilder);
    }
}
