using System.Text.RegularExpressions;
namespace AdventOfCode2023.Day02;

internal partial class Day02 : ISolution
{
    private const string CubeGroupPattern = @"(\d+?) (\w+)";
    private const string GameConst = "Game ";

    private readonly static IDictionary<string, int> _colorToMaxCubeCount = new Dictionary<string, int>{
        ["red"] = 12,
        ["green"] = 13,
        ["blue"] = 14
    };

    private static int GetGameId(string line) => int.Parse(line.AsSpan(GameConst.Length, line.IndexOf(":") - GameConst.Length));

    private static bool IsGameValid(string line) => Regex.Matches(line, CubeGroupPattern).All(match =>
        _colorToMaxCubeCount[match.Groups[2].Value] >= int.Parse(match.Groups[1].Value));

    public string Part1()
    {
        return Input.Split(Environment.NewLine)
            .Where(IsGameValid)
            .Select(GetGameId)
            .Sum()
            .ToString();
    }

    private static long GetGamePower(string line) => GetGameMinCounts(line)
            .Aggregate(1, (t, f) => t * f.Value);

    private static IDictionary<string, int> GetGameMinCounts(string line) => Regex.Matches(line, CubeGroupPattern)
            .GroupBy(match => match.Groups[2].Value)
            .ToDictionary(group => group.Key, group => group.Max(match => int.Parse(match.Groups[1].Value)));

    public string Part2()
    {
        return Input.Split(Environment.NewLine)
            .Select(GetGamePower)
            .Sum()
            .ToString();
    }
}