using AdventOfCode2024.Mapping;

namespace AdventOfCode2024.Days;

internal class DayEight : IDay
{
    public int Day => 8;

    public string ComputeFirst(string input)
    {
        var map = new AntennaMap(input);
        var antinodes = map.GetAllAntinodes().ToHashSet();
        return antinodes.Count.ToString();
    }

    public string ComputeSecond(string input)
    {
        var map = new AntennaMap(input);
        var antinodes = map.GetAllAntinodes(harmonics: true).ToHashSet();
        return antinodes.Count.ToString();
    }

    private class AntennaMap(string input) : Map(input)
    {
        public IEnumerable<Antenna> GetAntennas()
            => from pos in EnumerateAllPositions()
               where !IsEmpty(pos)
               select new Antenna(GetCell(pos), pos);

        public IEnumerable<Position> GetAllAntinodes(bool harmonics = false)
        {
            var antennasByFrequency = GetAntennas().ToLookup(x => x.Frequency);

            foreach (var frequencyGroup in antennasByFrequency)
                foreach (var antinode in GetAntinodesByFrequency(frequencyGroup.ToArray(), harmonics))
                    yield return antinode;
        }

        private IEnumerable<Position> GetAntinodesByFrequency(Antenna[] antennas, bool harmonics)
        {
            for (int i = 0; i < antennas.Length; i++)
                for (int j = i + 1; j < antennas.Length; j++)
                    foreach (var antinode in GetAntinodes(antennas[i], antennas[j], harmonics))
                        yield return antinode;
        }

        private IEnumerable<Position> GetAntinodes(Antenna first, Antenna second, bool harmonics)
            => harmonics ? GetHarmonicsAntinodes(first, second) : GetSimpleAntinodes(first, second);

        private IEnumerable<Position> GetHarmonicsAntinodes(Antenna first, Antenna second)
        {
            yield return first.Position;
            yield return second.Position;

            var horizontalDistance = first.Position.X - second.Position.X;
            var verticalDistance = first.Position.Y - second.Position.Y;

            Position candidate = new(first.Position.X + horizontalDistance, first.Position.Y + verticalDistance);
            while (!IsOutOfBounds(candidate))
            {
                yield return candidate;
                candidate = new(candidate.X + horizontalDistance, candidate.Y + verticalDistance);
            }

            candidate = new(first.Position.X - horizontalDistance, first.Position.Y - verticalDistance);
            while (!IsOutOfBounds(candidate))
            {
                yield return candidate;
                candidate = new(candidate.X - horizontalDistance, candidate.Y - verticalDistance);
            }
        }

        private IEnumerable<Position> GetSimpleAntinodes(Antenna first, Antenna second)
        {
            var horizontalDistance = first.Position.X - second.Position.X;
            var verticalDistance = first.Position.Y - second.Position.Y;

            Position candidate = new(first.Position.X + horizontalDistance, first.Position.Y + verticalDistance);
            if (!IsOutOfBounds(candidate) && candidate != second.Position)
                yield return candidate;

            candidate = new(first.Position.X - horizontalDistance, first.Position.Y - verticalDistance);
            if (!IsOutOfBounds(candidate) && candidate != second.Position)
                yield return candidate;

            candidate = new(second.Position.X + horizontalDistance, second.Position.Y + verticalDistance);
            if (!IsOutOfBounds(candidate) && candidate != first.Position)
                yield return candidate;

            candidate = new(second.Position.X - horizontalDistance, second.Position.Y - verticalDistance);
            if (!IsOutOfBounds(candidate) && candidate != first.Position)
                yield return candidate;
        }
    }

    private record struct Antenna(char Frequency, Position Position);
}
