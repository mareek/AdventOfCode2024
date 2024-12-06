using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Days;

internal class DayFour : IDay
{
    public int Day => 4;

    public string ComputeFirst(string input)
    {
        const string word = "XMAS";
        var lines = InputReader.GetLines(input);
        var result = CountHorizontal(input, word)
                     + CountVertical(lines, word)
                     + CountDiagonalNWSE(lines, word)
                     + CountDiagonalSWNE(lines, word);
        return result.ToString();
    }

    public int CountHorizontal(string input, string word)
    {
        string reversed = new(word.Reverse().ToArray());
        int result = 0;
        for (int i = 0; i <= (input.Length - word.Length); i++)
        {
            string target = input[i..(i + word.Length)];
            if (word == target)
                result++;
            if (reversed == target)
                result++;
        }

        return result;
    }

    public int CountVertical(string[] lines, string word)
    {
        string reversed = new(word.Reverse().ToArray());
        int result = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (!TryBuildTarget(i, j, out var target))
                    break;

                if (word == target)
                    result++;
                if (reversed == target)
                    result++;
            }
        }

        return result;

        bool TryBuildTarget(int row, int col, out string? target)
        {
            target = null;
            StringBuilder resultBuilder = new(word.Length);
            for (int i = 0; i < word.Length; i++)
            {
                int targetRow = row + i;
                if (lines.Length <= targetRow)
                    return false;

                if (lines[targetRow].Length <= col)
                    return false;

                resultBuilder.Append(lines[targetRow][col]);
            }

            target = resultBuilder.ToString();
            return true;
        }
    }

    public int CountDiagonalNWSE(string[] lines, string word)
    {
        string reversed = new(word.Reverse().ToArray());
        int result = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (!TryBuildTarget(i, j, out var target))
                    break;

                if (word == target)
                    result++;
                if (reversed == target)
                    result++;
            }
        }

        return result;

        bool TryBuildTarget(int row, int col, out string? target)
        {
            target = null;
            StringBuilder resultBuilder = new(word.Length);
            for (int i = 0; i < word.Length; i++)
            {
                int targetRow = row + i;
                int targetCol = col + i;
                if (lines.Length <= targetRow)
                    return false;

                if (lines[targetRow].Length <= targetCol)
                    return false;

                resultBuilder.Append(lines[targetRow][targetCol]);
            }

            target = resultBuilder.ToString();
            return true;
        }
    }

    public int CountDiagonalSWNE(string[] lines, string word)
    {
        string reversed = new(word.Reverse().ToArray());
        int result = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (!TryBuildTarget(i, j, out var target))
                    break;

                if (word == target)
                    result++;
                if (reversed == target)
                    result++;
            }
        }

        return result;

        bool TryBuildTarget(int row, int col, out string? target)
        {
            target = null;
            StringBuilder resultBuilder = new(word.Length);
            for (int i = 0; i < word.Length; i++)
            {
                int targetRow = row - i;
                int targetCol = col + i;
                if (targetRow < 0)
                    return false;

                if (lines[targetRow].Length <= targetCol)
                    return false;

                resultBuilder.Append(lines[targetRow][targetCol]);
            }

            target = resultBuilder.ToString();
            return true;
        }
    }

    public string ComputeSecond(string input)
    {
        var lines = InputReader.GetLines(input);
        Regex[] xmasRegexes =
        [
            new("M.S\n.A.\nM.S", RegexOptions.Compiled),
            new("M.M\n.A.\nS.S", RegexOptions.Compiled),
            new("S.M\n.A.\nS.M", RegexOptions.Compiled),
            new("S.S\n.A.\nM.M", RegexOptions.Compiled),
        ];
        int result = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (!TryBuildTarget(i, j, out var target))
                    break;

                if (xmasRegexes.Any(r => r.IsMatch(target)))
                    result++;
            }
        }

        return result.ToString();

        bool TryBuildTarget(int row, int col, [NotNullWhen(returnValue: true)] out string? target)
        {
            target = null;
            StringBuilder resultBuilder = new(12);
            for (int i = 0; i < 3; i++)
            {
                int targetRow = row + i;
                if (lines.Length <= targetRow)
                    return false;

                for (int j = 0; j < 3; j++)
                {
                    int targetCol = col + j;
                    if (lines[targetRow].Length <= targetCol)
                        return false;

                    resultBuilder.Append(lines[targetRow][targetCol]);
                }
                resultBuilder.Append("\n");
            }

            target = resultBuilder.ToString();
            return true;
        }

    }
}
