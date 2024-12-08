using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Days;

internal class DaySeven : IDay
{
    public int Day => 7;
    
    public bool IsSlow() => true;

    public string ComputeFirst(string input)
    {
        var equations = InputReader.GetLines(input)
                                   .Select(l => new Equation(l))
                                   .ToArray();

        var result = equations.Where(e => IsSolvable(e, Operator.GetFirstOperators()))
                              .Sum(e => e.Result);

        return result.ToString();
    }

    public string ComputeSecond(string input)
    {
        var equations = InputReader.GetLines(input)
                                   .Select(l => new Equation(l))
                                   .ToArray();

        var result = equations.Where(e => IsSolvable(e, Operator.GetSecondOperators()))
                              .Sum(e => e.Result);

        return result.ToString();
    }

    private bool IsSolvable(Equation equation, Operator[] operators)
    {
        var compinations = GetAllPossibleCombinations(operators, equation.Numbers.Length - 1);
        return compinations.Any(equation.IsSolution);
    }

    private IEnumerable<T[]> GetAllPossibleCombinations<T>(T[] possibleValues, int length)
    {
        var results = new List<T[]>();
        Recurse(possibleValues, length, [], results);
        return results;

        static void Recurse(T[] possibleValues, int length, IEnumerable<T> inConstruction, List<T[]> results)
        {
            if (length == 0)
                results.Add(inConstruction.ToArray());
            else
                foreach (var value in possibleValues)
                    Recurse(possibleValues, length - 1, inConstruction.Append(value), results);
        }
    }

    private class Equation
    {
        public long Result { get; }
        public long[] Numbers { get; }

        public Equation(string line)
        {
            var splitedInput = line.Split(':');
            Result = long.Parse(splitedInput[0]);
            Numbers = InputReader.GetLongs(splitedInput[1]);
        }

        public bool IsSolution(Operator[] operators)
        {
            if (operators.Length != (Numbers.Length - 1))
                throw new ArgumentException($"Expected {(Numbers.Length - 1)} oeprators, but got {operators.Length}", nameof(operators));

            long result = Numbers[0];
            for (int i = 0; i < operators.Length; i++)
            {
                result = operators[i].Compute(result, Numbers[i + 1]);
            }

            return result == Result;
        }
    }

    private class Operator
    {
        private readonly Func<long, long, long> _operation;

        public Operator(Func<long, long, long> operation) => _operation = operation;

        public long Compute(long left, long right) => _operation(left, right);

        public static Operator[] GetFirstOperators()
            => [new((a, b) => a + b), new((a, b) => a * b)];

        public static Operator[] GetSecondOperators()
            => [new((a, b) => a + b), new((a, b) => a * b), new(Concatenate)];

        private static long Concatenate(long left, long right)
            => long.Parse($"{left}{right}");
    }
}
