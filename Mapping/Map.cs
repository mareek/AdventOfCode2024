using System;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Mapping;

public class Map
{
    private readonly string[] _grid;
    private readonly char _emptyGlyph;

    public Map(string input, char emptyGlyph = '.')
    {
        _grid = InputReader.GetLines(input);
        Height = _grid.Length;
        Width = _grid[0].Length;
        _emptyGlyph = emptyGlyph;
    }

    public int Height { get; }
    public int Width { get; }

    public bool IsOutOfBounds(Position pos)
        => pos.X < 0 || pos.Y < 0 || pos.Y >= Height || pos.X >= Width;

    public char GetCell(Position pos) => _grid[pos.Y][pos.X];

    public bool IsEmpty(Position pos) => GetCell(pos) == _emptyGlyph;

    public IEnumerable<Position> EnumerateAllPositions()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return new(x, y);
    }

    public void Print(IReadOnlyCollection<Position> objects, char glyph)
    {
        Console.WriteLine();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Position pos = new(x, y);
                Console.Write(objects.Contains(pos) ? glyph : GetCell(pos));
            }
            Console.Write('\n');
        }
    }
}
