using AdventOfCode2024.Mapping;

namespace AdventOfCode2024.Days;

internal partial class DaySix : IDay
{
    public int Day => 6;

    public bool IsSlow() => true;

    public string ComputeFirst(string input)
    {
        GuardMap map = new(input);
        HashSet<Position> coveredPositions = new();
        foreach (var guard in map.GetGuards())
        {
            coveredPositions.UnionWith(guard.GetCoveredPosition(map));
        }
        return coveredPositions.Count.ToString();
    }

    public string ComputeSecond(string input)
    {
        GuardMap map = new(input);
        HashSet<Position> blockPositions = new();
        foreach (var guard in map.GetGuards())
        {
            blockPositions.UnionWith(guard.GetPossibleBlocks(map));
        }
        return blockPositions.Count.ToString();
    }
    private class GuardMap(string input) : Map(input)
    {
        public bool IsObstacle(Position pos)
            => GetCell(pos) == '#';

        public bool IsGuard(Position pos)
            => GetCell(pos) is '<' or '>' or '^' or 'v';

        public IEnumerable<Guard> GetGuards()
            => from pos in EnumerateAllPositions()
               where IsGuard(pos)
               select new Guard(pos, GetCell(pos));
    }

    private class Guard(Position start, char direction)
    {
        public IEnumerable<Position> GetCoveredPosition(GuardMap map)
        {
            var curPosition = start;
            var curDir = Direction.Parse(direction);
            while (true)
            {
                yield return curPosition;
                
                var nextPosition = curDir.Move(curPosition);
                if (map.IsOutOfBounds(nextPosition))
                    yield break;

                if (map.IsObstacle(nextPosition))
                    curDir = curDir.TurnRight();
                else
                    curPosition = nextPosition;
            }
        }

        public IEnumerable<Position> GetPossibleBlocks(GuardMap map)
        {
            var curPos = start;
            var curDir = Direction.Parse(direction);

            return GetCoveredPosition(map).Where(p => IsGoodBlock(map, curPos, curDir, p));
        }

        private bool IsGoodBlock(GuardMap map, Position position, Direction direction, Position obstaclePos)
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

                if (obstaclePos == nextPosition || map.IsObstacle(nextPosition))
                    curDir = curDir.TurnRight();
                else
                    curPos = nextPosition;
            }
        }
    }
}
