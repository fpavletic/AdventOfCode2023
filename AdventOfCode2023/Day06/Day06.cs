using System.Numerics;

namespace AdventOfCode2023.Day06;

internal partial class Day06 : ISolution
{

    // distance = t_charge * ( t_total - t_charge ) => - t_charge ^ 2 + t_charge * t_total - distance = 0 
    // t1,t2 = (-t_total +- sqrt(t_total^2 - 4 * distance )) / -2
    //total values = t2 - t1 + 1
    
    // eg. t_total = 7, min dist = 10
    // t1, t2 = (-7 +- sqrt(49 - 40)) / -2 = 3.5 -+ 3/2 = 2, 5
    // total values = 5 - 2 + 1 = 4

    private (long minT, long maxT) GetRoots(long time, long distance){
        var root = Math.Sqrt(time * time - 4L * (distance+1L)) / -2.0;
        return ((long)Math.Ceiling(time / 2.0 + root), (long)Math.Floor(time / 2.0 - root));
    }

    public string Part1()
    {
        var splitInput = Input.Split(Environment.NewLine);
        var times = splitInput[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        var distances = splitInput[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        return Enumerable.Range(0, times.Length)
            .Select(i => GetRoots(times[i], distances[i]))
            .Select(t => t.maxT - t.minT + 1)
            .Aggregate(1L, (l, f) => l*f).ToString();
    }

    public string Part2()
    {
        var splitInput = Input.Split(Environment.NewLine);
        var time = long.Parse(splitInput[0].Substring("Time:".Length + 1).Replace(" ", ""));
        var distance = long.Parse(splitInput[1].Substring("Distance:".Length + 1).Replace(" ", ""));
        var (minT, maxT) = GetRoots(time, distance);
        return (maxT - minT + 1L).ToString();
    }
}