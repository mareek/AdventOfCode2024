namespace AdventOfCode2024.Helpers;

internal static class InputReader
{
    public static string ReadTestInput(int day) => ReadInput("TestInput", day);

    public static string ReadRealInput(int day) => ReadInput("RealInput", day);

    private static string ReadInput(string folder, int day)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, folder, $"{day}.txt");
        return File.ReadAllText(filePath);
    }

    public static string[] GetLines(string input)
        => input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public static int[] GetNumbers(string line)
        => line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
               .Select(int.Parse)
               .ToArray();
}
