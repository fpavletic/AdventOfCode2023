using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day08;

internal partial class Day08 : ISolution
{
    private const string NodePattern = @"([A-Z0-9]{3}) = \(([A-Z0-9]{3}), ([A-Z0-9]{3})\)";

    IDictionary<string, (string left, string right)> ParseNodes(){
        return Regex.Matches(Input, NodePattern)
            .Select(m => (current: m.Groups[1].Value, left: m.Groups[2].Value, right: m.Groups[3].Value))
            .ToDictionary(n => n.current, n => (n.left, n.right));
    }

    long GetCycleLength(string node, Predicate<string> isCycleComplete, bool[] sequence, IDictionary<string, (string left, string right)> nodes){
        long count = 0;
        while ( !isCycleComplete(node)){
            node = sequence[count++ % sequence.Length] ? nodes[node].left : nodes[node].right;
        }
        return count;
    }

    long GetGreatestCommonDivisor(long n, long m) {
        if (m == 0) return n;
        return GetGreatestCommonDivisor(m, n%m);
    }

    public string Part1()
    {
        var sequence = Input[..Input.IndexOf(Environment.NewLine)]
            .Select(c => c == 'L').ToArray();
        var nodes = ParseNodes();
        return GetCycleLength("AAA", n => n.Equals("ZZZ"), sequence, nodes).ToString();
    }

    public string Part2()
    {
        var sequence = Input[..Input.IndexOf(Environment.NewLine)]
            .Select(c => c == 'L').ToArray();
        var nodes = ParseNodes();
        return nodes.Keys.Where(n => n[2] == 'A')
            .Select(n => GetCycleLength(n, n => n[2] == 'Z', sequence, nodes))
            .Aggregate(1L, (t, c) => t * c / GetGreatestCommonDivisor(t, c))
            .ToString();
    }
}