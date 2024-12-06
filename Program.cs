using AdventOfCode2024.Days;
using AdventOfCode2024.Helpers;

const bool isTest = false;

IDay[] days =
[
    new DayOne(),
    new DayTwo(),
    new DayThree(),
    new DayFour(),
    new DayFive(),
];

foreach (var day in days)
{
    var input = isTest ? InputReader.ReadTestInput(day.Day) : InputReader.ReadRealInput(day.Day);

    var chrono = System.Diagnostics.Stopwatch.StartNew();
    var firstSolution = day.ComputeFirst(input);
    var secondSolution = day.ComputeSecond(input);
    chrono.Stop();
    Console.WriteLine($" {day.Day:00} : {firstSolution} \t| {secondSolution} \tin {chrono.ElapsedMilliseconds} ms");
}