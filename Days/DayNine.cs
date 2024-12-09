using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Days;

internal class DayNine : IDay
{
    public int Day => 9;

    public string ComputeFirst(string input)
    {
        var memoryBlocks = ParseInput(input).ToArray();
        var memoryLayout = GetMemoryLayout(memoryBlocks).ToArray();
        Compact(memoryLayout);
        var result = ComputeChecksum(memoryLayout);
        return result.ToString();
    }

    public string ComputeSecond(string input)
    {
        var memoryBlocks = ParseInput(input).ToArray();
        var memoryLayout = GetMemoryLayout(memoryBlocks).ToArray();
        Defrag(memoryLayout, memoryBlocks);
        var result = ComputeChecksum(memoryLayout);
        return result.ToString();
    }

    private void Print(long?[] memoryLayout)
    {
        Console.WriteLine();
        foreach (var memoryBlock in memoryLayout)
        {
            if (memoryBlock is long fileId)
                Console.Write(fileId);
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }

    public static long ComputeChecksum(long?[] memoryLayout)
    {
        long result = 0;
        for (int i = 0; i < memoryLayout.Length; i++)
            if (memoryLayout[i] is long memValue)
                result += i * memValue;

        return result;
    }

    private void Defrag(long?[] memoryLayout, MemoryBlock[] memoryBlocks)
    {
        foreach (var memoryBlock in GetMemoryBlockReverse())
        {
            if (FindFirstFreeMemoryBlock(memoryBlock) is MemoryBlock freeBlock)
                Copy(memoryBlock, freeBlock);
        }

        void Copy(MemoryBlock origin, MemoryBlock destination)
        {
            for (int i = 0; i < origin.Length; i++)
            {
                memoryLayout[destination.Start + i] = origin.Id;
                memoryLayout[origin.Start + i] = null;
            }

            destination.Start += origin.Length;
            destination.Length -= origin.Length;
        }

        MemoryBlock? FindFirstFreeMemoryBlock(MemoryBlock memoryBlock)
            => memoryBlocks.TakeWhile(b => b.Start < memoryBlock.Start)
                           .Where(b => b.IsFree)
                           .FirstOrDefault(b => memoryBlock.Length <= b.Length);

        IEnumerable<MemoryBlock> GetMemoryBlockReverse()
        {
            for (int i = memoryBlocks.Length - 1; i >= 0; i--)
            {
                var memoryBlock = memoryBlocks[i];
                if (!memoryBlock.IsFree)
                    yield return memoryBlock;
            }
        }
    }

    private static void Compact(long?[] memoryLayout)
    {
        long freeSpaceIndex = GetNextFreeSpace(-1);
        long fileSectorIndex = GetNextFileSector(memoryLayout.Length);

        while (freeSpaceIndex < fileSectorIndex)
        {
            memoryLayout[freeSpaceIndex] = memoryLayout[fileSectorIndex];
            memoryLayout[fileSectorIndex] = null;
            freeSpaceIndex = GetNextFreeSpace(freeSpaceIndex);
            fileSectorIndex = GetNextFileSector(fileSectorIndex);
        }

        long GetNextFreeSpace(long curIndex)
        {
            for (long i = curIndex + 1; i < memoryLayout.Length; i++)
                if (memoryLayout[i] is null)
                    return i;

            return memoryLayout.Length;
        }

        long GetNextFileSector(long curIndex)
        {
            for (long i = curIndex - 1; i >= 0; i--)
                if (memoryLayout[i] is not null)
                    return i;

            return -1;
        }
    }

    private static IEnumerable<long?> GetMemoryLayout(IEnumerable<MemoryBlock> memoryBlocks)
    {
        foreach (var memoryBlock in memoryBlocks)
            for (long i = 0; i < memoryBlock.Length; i++)
                yield return memoryBlock.Id;
    }

    private static IEnumerable<MemoryBlock> ParseInput(string input)
    {
        bool isFile = true;
        long fileIndex = 0;
        long cursor = 0;
        foreach (var chr in input)
        {
            long length = long.Parse(chr.ToString());
            if (isFile)
                yield return new MemoryBlock(fileIndex++, cursor, length);
            else
                yield return new MemoryBlock(null, cursor, length);

            isFile = !isFile;
            cursor += length;
        }
    }

    private class MemoryBlock(long? id, long strart, long length)
    {
        public long? Id => id;
        public long Start { get; set; } = strart;
        public long Length { get; set; } = length;
        public bool IsFree => !Id.HasValue;
    }

}
