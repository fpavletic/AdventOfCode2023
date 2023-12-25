using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day12;

internal partial class Day12 : ISolution
{
    private long CountValid(char[] values, int valueIndex, int[] groupSizes, int groupSizeIndex, int currentGroupSize,
            Dictionary<(int valueIndex, int groupIndex, int currentGroupSize), long> keysToValidCount)
    {    
        var keys = (valueIndex, groupSizeIndex, currentGroupSize);
        if ( !keysToValidCount.TryGetValue(keys, out var validCount)){
            keysToValidCount[keys] = validCount = CountValidInternal(values, valueIndex, groupSizes, groupSizeIndex, 
                currentGroupSize, keysToValidCount);
        }
        return validCount;
    }

    private long CountValidInternal(char[] values, int valueIndex, int[] groupSizes, int groupSizeIndex, int currentGroupSize, 
            Dictionary<(int valueIndex, int groupIndex, int currentGroupSize), long> keysToValidCount)
    {
        if ( valueIndex == values.Length && groupSizeIndex == groupSizes.Length -1 && groupSizes[groupSizeIndex] == currentGroupSize ) return 1;
        if ( valueIndex == values.Length && groupSizeIndex == groupSizes.Length && currentGroupSize == 0) return 1;
        if ( valueIndex == values.Length) return 0;

        long validCount = 0;

        // part broken, increment group size
        if ( values[valueIndex] is '#' or '?'){
            validCount += CountValid(values, valueIndex + 1, groupSizes, groupSizeIndex, currentGroupSize + 1, keysToValidCount);
        }

        // part ok, try to close group if open
        if ( values[valueIndex] is '.' or '?'){
            validCount += currentGroupSize == 0 || (groupSizeIndex < groupSizes.Length && groupSizes[groupSizeIndex++] == currentGroupSize) 
                ? CountValid(values, valueIndex + 1, groupSizes, groupSizeIndex, 0, keysToValidCount)
                : 0;
        }
        return validCount;
    }

    public string Part1()
    {
        return Input.Split(Environment.NewLine)
            .Select(l => LinePatternRegex().Match(l))
            .Select(m => (values: m.Groups[1].Value.ToArray(), groupSizes: m.Groups[2].Value.Split(',').Select(int.Parse).ToArray()))
            .Select(g => CountValid(g.values, 0, g.groupSizes, 0, 0, new Dictionary<(int valueIndex, int groupIndex, int currentGroupSize), long>()))
            .Sum().ToString();
    }

    private string JoinStringOnItself(string value, int count, char separator){
        return string.Join(separator, Enumerable.Range(0, count).Select(_ => value));
    }

    public string Part2()
    {
        return Input.Split(Environment.NewLine)
            .Select(l => LinePatternRegex().Match(l))
            .Select(m => (values: JoinStringOnItself(m.Groups[1].Value, 5, '?').ToCharArray(), 
                groupSizes: JoinStringOnItself(m.Groups[2].Value, 5, ',').Split(',').Select(int.Parse).ToArray()))
            .Select(g => CountValid(g.values, 0, g.groupSizes, 0, 0, []))
            .Sum().ToString();
    }

    private const string LinePattern = @"([\.\?#]+) ([\d,]+)";
    [GeneratedRegex(LinePattern)]
    private static partial Regex LinePatternRegex();
}