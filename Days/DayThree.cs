using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

internal class DayThree : IDay
{
    public int Day => 3;

    public string ComputeFirst(string input)
    {
        int result = 0;
        foreach (var (left, right) in GetMulInstructions(input, false))
            result += left * right;

        return result.ToString();
    }

    public string ComputeSecond(string input)
    {
        int result = 0;
        foreach (var (left, right) in GetMulInstructions(input, true))
            result += left * right;

        return result.ToString();
    }

    private IEnumerable<(int left, int right)> GetMulInstructions(string input, bool conditional)
    {
        //mul(123,4)
        Regex mulRegex = new(@"^mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);
        const string doInstruction = "do()";
        const string dontInstruction = "don't()";

        bool isEnabled = true;
        for (int i = 0; i < input.Length - 8; i++)
        {
            if (conditional)
            {
                string dontsubs = input.Substring(i, dontInstruction.Length);
                if (doInstruction == input.Substring(i, doInstruction.Length))
                    isEnabled = true;

                if (dontInstruction == dontsubs)
                    isEnabled = false;

                if (!isEnabled)
                    continue;
            }

            var length = Math.Min(input.Length - i, 12);
            var matchResult = mulRegex.Match(input, i, length);
            if (matchResult.Success)
                yield return (int.Parse(matchResult.Groups[1].ValueSpan), int.Parse(matchResult.Groups[2].ValueSpan));
        }
    }
}
