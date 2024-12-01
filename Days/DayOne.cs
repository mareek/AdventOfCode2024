namespace AdventOfCode2024.Days;

internal class DayOne : IDay
{
    public int Day => 1;

    public string ComputeFirst(string input)
    {
        Parse(input, out var left, out var right);

        left.Sort();
        right.Sort();

        var result = left.Zip(right, (l, r) => Math.Abs(l - r)).Sum();
        return result.ToString();
    }

    public string ComputeSecond(string input)
    {
        Parse(input, out var left, out var right);

        int result = 0;
        for (int i = 0; i < left.Count; i++)
        {
            var lefty = left[i];
            for (var j = 0; j < right.Count; j++)
            {
                if (lefty == right[j])
                {
                    result += lefty;
                }
            }
        }

        return result.ToString();
    }

    private static void Parse(string input, out List<int> left, out List<int> right)
    {
        left = [];
        right = [];
        foreach (var line in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var splitedLine = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            left.Add(int.Parse(splitedLine[0]));
            right.Add(int.Parse(splitedLine[1]));
        }
    }
}
