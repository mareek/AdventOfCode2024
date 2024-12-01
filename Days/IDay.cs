namespace AdventOfCode2024.Days;

internal interface IDay
{
    int Day { get; }
    string ComputeFirst(string input);
    string ComputeSecond(string input);
}
