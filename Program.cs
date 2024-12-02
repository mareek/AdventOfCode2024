﻿using AdventOfCode2024.Days;
using AdventOfCode2024.Helpers;

const bool isTest = false;

IDay[] days =
[
    new DayOne(),
    new DayTwo(),
];

foreach (var day in days)
{
    var input = isTest ? InputReader.ReadTestInput(day.Day) : InputReader.ReadRealInput(day.Day);

    var chrono = System.Diagnostics.Stopwatch.StartNew();
    var firstSolution = day.ComputeFirst(input);
    var secondSolution = day.ComputeSecond(input);
    chrono.Stop();
    Console.WriteLine($" {day.Day:00} :  {firstSolution} | {secondSolution} in {chrono.ElapsedMilliseconds} ms");
}