namespace AdventOfCode2024.Days;

internal interface IDay
{
    int Day { get; }
    string ComputeFirst(string input) => "not yet implemented";
    string ComputeSecond(string input) => "not yet implemented";
    bool IsSlow() => false;
}
