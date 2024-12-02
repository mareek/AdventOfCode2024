using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Days;

internal class DayTwo : IDay
{
    public int Day => 2;

    public string ComputeFirst(string input)
    {
        var result = InputReader.GetLines(input)
                                .Select(InputReader.GetNumbers)
                                .Count(ReportIsSafe);
        return result.ToString();
    }

    public string ComputeSecond(string input)
    {
        var result = InputReader.GetLines(input)
                                .Select(InputReader.GetNumbers)
                                .Count(ReportIsAlmostSafe);
        return result.ToString();
    }

    private bool ReportIsSafe(int[] report)
    {
        bool reportIsIncreasing = (report[0] - report[1]) < 0;
        for (int i = 1; i < report.Length; i++)
        {
            var diff = report[i - 1] - report[i];
            if (reportIsIncreasing ^ (diff < 0))
                return false;

            var absDiff = Math.Abs(diff);
            if (absDiff < 1 || 3 < absDiff)
                return false;
        }
        return true;
    }

    private bool ReportIsAlmostSafe(int[] report)
    {
        if (ReportIsSafe(report))
            return true;

        for (int i = 0; i < report.Length; i++)
        {
            var shortReport = GetShortenedReport(i).ToArray();
            if (ReportIsSafe(shortReport))
                return true;
        }

        return false;

        IEnumerable<int> GetShortenedReport(int itemToSkip)
        {
            for (int i = 0; i < report.Length; i++)
            {
                if (i != itemToSkip)
                    yield return report[i];
            }
        }
    }
}
