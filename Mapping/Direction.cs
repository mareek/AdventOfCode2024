namespace AdventOfCode2024.Mapping;

public struct Direction(int x, int y)
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
