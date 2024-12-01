using AdventOfCode2024.Days;
using AdventOfCode2024.Helpers;

const bool isTest = false;

IDay[] days =
[
    new DayOne(),
];

foreach (var day in days)
{
    var input = isTest? InputReader.ReadTestInput(day.Day) :InputReader.ReadRealInput(day.Day);

    var firstSolution = day.ComputeFirst(input);
    var secondSolution = day.ComputeSecond(input);
    Console.WriteLine($" {day.Day:00} :  {firstSolution} | {secondSolution}");
}