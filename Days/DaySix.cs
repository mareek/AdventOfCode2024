using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Days;

internal class DaySix : IDay
{
    public int Day => 6;

    public bool IsSlow() => true;

    public string ComputeFirst(string input)
    {
        Map map = new(input);
        HashSet<Position> coveredPositions = new();
        foreach (var guard in map.GetGuards())
        {
            coveredPositions.UnionWith(guard.GetCoveredPosition(map));
        }
        return coveredPositions.Count.ToString();
    }

    public string ComputeSecond(string input)
    {
        Map map = new(input);
        HashSet<Position> blockPositions = new();
        foreach (var guard in map.GetGuards())
        {
            blockPositions.UnionWith(guard.GetPossibleBlocks(map));
        }
        return blockPositions.Count.ToString();
    }

    private class Map
    {
        private readonly string[] _grid;

        public Map(string input)
        {
            _grid = InputReader.GetLines(input);
            Height = _grid.Length;
            Width = _grid[0].Length;
        }

        public int Height { get; }
        public int Width { get; }

        public IEnumerable<Guard> GetGuards()
        {
            for (int y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Position pos = new(x, y);
                    if (IsGuard(pos))
                        yield return new(pos, GetCell(pos));
                }
            }
        }

        public bool IsOutOfBounds(Position pos)
            => pos.X < 0 || pos.Y < 0 || pos.Y >= Height || pos.X >= Width;

        public bool IsObstacle(Position pos)
            => GetCell(pos) == '#';

        public bool IsGuard(Position pos)
            => GetCell(pos) is '<' or '>' or '^' or 'v';

        public bool IsEmpty(Position pos) => !IsObstacle(pos) && !IsGuard(pos);

        private char GetCell(Position pos) => _grid[pos.Y][pos.X];

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

    private struct Position(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    private struct Direction(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public Direction TurnRight()
        {
            if (X == 1)
                return new(0, 1);
            if (Y == 1)
                return new(-1, 0);
            if (X == -1)
                return new(0, -1);
            if (Y == -1)
                return new(1, 0);

            throw new Exception("WHAT THE FUCKING FUCK ????");
        }

        public Position Move(Position position)
            => new(position.X + X, position.Y + Y);

        public static Direction Parse(char direction)
            => new(direction switch { '<' => -1, '>' => 1, _ => 0 }, direction switch { '^' => -1, 'v' => 1, _ => 0 });
    }

    private class Guard(Position start, char direction)
    {
        public IEnumerable<Position> GetCoveredPosition(Map map)
        {
            HashSet<(Position pos, Direction dir)> cycleDetector = new();
            var curPosition = start;
            var curDir = Direction.Parse(direction);
            while (true)
            {
                yield return curPosition;
                cycleDetector.Add((curPosition, curDir));
                var nextPosition = curDir.Move(curPosition);
                if (map.IsOutOfBounds(nextPosition))
                    yield break;

                if (map.IsObstacle(nextPosition))
                    curDir = curDir.TurnRight();
                else
                    curPosition = nextPosition;

                if (cycleDetector.Contains((curPosition, curDir)))
                    yield break;
            }
        }

        public IEnumerable<Position> GetPossibleBlocks(Map map)
        {
            var curPos = start;
            var curDir = Direction.Parse(direction);
            while (true)
            {
                var nextPosition = curDir.Move(curPos);
                if (map.IsOutOfBounds(nextPosition))
                    break;

                if (map.IsObstacle(nextPosition))
                    curDir = curDir.TurnRight();
                else
                {
                    if (IsGoodBlock(map, curPos, curDir, nextPosition))
                        yield return nextPosition;
                    curPos = nextPosition;
                }
            }
        }

        private bool IsGoodBlock(Map map, Position position, Direction direction, Position obstaclePos)
        {
            const int maxTryDistance = 10000;

            if (!map.IsEmpty(obstaclePos))
                return false;

            HashSet<(Position pos, Direction dir)> cycleDetector = new();

            var curDir = direction;
            var curPos = position;
            int tryDistance = 0;

            while (true)
            {
                if (tryDistance++ > maxTryDistance)
                    throw new Exception("LOL");

                if (cycleDetector.Contains((curPos, curDir)))
                    return true;

                cycleDetector.Add((curPos, curDir));

                var nextPosition = curDir.Move(curPos);
                if (map.IsOutOfBounds(nextPosition))
                    return false;

                if (obstaclePos.Equals(nextPosition) || map.IsObstacle(nextPosition))
                    curDir = curDir.TurnRight();
                else
                    curPos = nextPosition;
            }
        }
    }
}
