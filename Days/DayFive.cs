using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Days;

internal class DayFive : IDay
{
    public int Day => 5;

    public string ComputeFirst(string input)
    {
        var lines = InputReader.GetLines(input);
        Dictionary<int, HashSet<int>> rulesBefore = ParseRules(lines).GroupBy(r => r.After, r => r.Before)
                                                                     .ToDictionary(g => g.Key, g => g.ToHashSet());

        int result = 0;
        foreach (var update in ParseUpdates(lines))
        {
            if (UpdateIsValid(update, rulesBefore))
            {
                result += update[update.Length / 2];
            }
        }

        return result.ToString();
    }

    private static IEnumerable<Rule> ParseRules(string[] lines)
    {
        foreach (var line in lines)
        {
            var separatorPos = line.IndexOf('|');
            if (separatorPos < 0)
                yield break;

            int before = int.Parse(line[..separatorPos]);
            int after = int.Parse(line[(separatorPos + 1)..]);
            yield return new(before, after);
        }
    }

    private static IEnumerable<int[]> ParseUpdates(string[] lines)
    {
        foreach (var line in lines)
        {
            if (!line.Contains(','))
                continue;

            yield return line.Split(',').Select(int.Parse).ToArray();
        }
    }

    private static bool UpdateIsValid(int[] update, Dictionary<int, HashSet<int>> rulesBefore)
    {
        for (int i = 0; i < update.Length; i++)
        {
            if (!rulesBefore.TryGetValue(update[i], out var beforeSet))
                continue;

            for (int j = i; j < update.Length; j++)
            {
                if (beforeSet.Contains(update[j]))
                    return false;
            }
        }

        return true;
    }

    public string ComputeSecond(string input)
    {
        var lines = InputReader.GetLines(input);
        Dictionary<int, HashSet<int>> rulesBefore = ParseRules(lines).GroupBy(r => r.After, r => r.Before)
                                                                     .ToDictionary(g => g.Key, g => g.ToHashSet());

        int result = 0;
        foreach (var update in ParseUpdates(lines))
        {
            if (!UpdateIsValid(update, rulesBefore))
            {
                ReorderUpdate(update, rulesBefore);
                result += update[update.Length / 2];
            }
        }

        return result.ToString();
    }

    private static void ReorderUpdate(int[] update, Dictionary<int, HashSet<int>> rulesBefore)
    {
        bool hasPermuted;

        do
        {
            hasPermuted = false;
            for (int i = 0; i < (update.Length - 1); i++)
            {
                if (!rulesBefore.TryGetValue(update[i], out var beforeSet))
                    continue;

                for (int j = i; j < update.Length; j++)
                {
                    if (beforeSet.Contains(update[j]))
                    {
                        var temp = update[i + 1];
                        update[i + 1] = update[i];
                        update[i] = temp;
                        hasPermuted = true;
                        break;
                    }
                }

            }

        } while (hasPermuted);
    }


    private record Rule(int Before, int After);
}
