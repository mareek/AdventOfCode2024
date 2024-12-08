namespace AdventOfCode2024.Mapping;

public struct Position(int x, int y) : IEquatable<Position>
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public override bool Equals(object? obj) => obj is Position position && Equals(position);

    public bool Equals(Position other) => X == other.X && Y == other.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !(left == right);
}
