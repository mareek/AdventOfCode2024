using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Days;

internal class DaySix : IDay
{
    public int Day => 6;

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

        private char GetCell(Position pos) => _grid[pos.Y][pos.X];
    }

    private struct Position(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    private class Guard(Position start, char direction)
    {
        private int Xdirection { get; set; } = direction switch { '<' => -1, '>' => 1, _ => 0 };
        private int Ydirection { get; set; } = direction switch { '^' => -1, 'v' => 1, _ => 0 };

        public IEnumerable<Position> GetCoveredPosition(Map map)
        {
            HashSet<(Position pos, int xDir, int yDir)> cycleDetector = new();
            var curPosition = start;
            while (true)
            {
                yield return curPosition;
                cycleDetector.Add((curPosition, Xdirection, Ydirection));
                var nextPosition = GetNextPosition(curPosition);
                if (map.IsOutOfBounds(nextPosition))
                    yield break;

                if (map.IsObstacle(nextPosition))
                    TurnRigh();
                else
                    curPosition = nextPosition;

                if (cycleDetector.Contains((curPosition, Xdirection, Ydirection)))
                    yield break;
            }
        }

        public IEnumerable<Position> GetPossibleBlocks(Map map)
        {
            HashSet<(Position pos, int xDir, int yDir)> cycleDetector = new();
            var originalDirections = (Xdirection, Ydirection);
            var curPos = start;
            while (true)
            {
                cycleDetector.Add((curPos, Xdirection, Ydirection));
                var nextPosition = GetNextPosition(curPos);
                if (map.IsOutOfBounds(nextPosition))
                    break;

                if (map.IsObstacle(nextPosition))
                    TurnRigh();
                else
                    curPos = nextPosition;
            }

            (Xdirection, Ydirection) = originalDirections;
            curPos = start;
            while (true)
            {
                var nextPosition = GetNextPosition(curPos);

                if (map.IsOutOfBounds(nextPosition))
                    break;

                if (!map.IsObstacle(nextPosition) && IsGoodBlock(map, curPos, cycleDetector))
                    yield return nextPosition;


                if (map.IsObstacle(nextPosition))
                    TurnRigh();
                else
                    curPos = nextPosition;
            }
        }

        private void TurnRigh()
            => (Xdirection, Ydirection) = TurnDirectionRigh(Xdirection, Ydirection);

        private static (int xDir, int yDir) TurnDirectionRigh(int xDirection, int yDirection)
        {
            if (xDirection == 1)
                return (0, 1);
            if (yDirection == 1)
                return (-1, 0);
            if (xDirection == -1)
                return (0, -1);
            if (yDirection == -1)
                return (1, 0);

            throw new Exception("WHAT THE FUCKING FUCK ????");
        }

        private Position GetNextPosition(Position position)
            => new(position.X + Xdirection, position.Y + Ydirection);

        private bool IsGoodBlock(Map map, Position position, HashSet<(Position pos, int xDir, int yDir)> cycleDetector)
        {
            const int maxTryDistance = 100;

            HashSet<(Position pos, int xDir, int yDir)> localCycleDetector = new();

            var direction = TurnDirectionRigh(Xdirection, Ydirection);
            var curPos = position;
            int tryDistance = 0;

            while (true)
            {
                if (tryDistance++ > maxTryDistance)
                    return false;

                localCycleDetector.Add((curPos, direction.xDir, direction.yDir));
                Position nextPosition = new(curPos.X + direction.xDir, curPos.Y + direction.yDir);
                if (map.IsOutOfBounds(nextPosition))
                    return false;

                if (map.IsObstacle(nextPosition))
                    direction = TurnDirectionRigh(Xdirection, Ydirection);
                else
                    curPos = nextPosition;

                var posAndDir = (curPos, direction.xDir, direction.yDir);
                if (cycleDetector.Contains(posAndDir) || localCycleDetector.Contains(posAndDir))
                    return true;
            }
        }
    }
}
