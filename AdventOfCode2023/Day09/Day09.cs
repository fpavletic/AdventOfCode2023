using System.Buffers;

namespace AdventOfCode2023.Day09;

internal partial class Day09 : ISolution
{
    long GetNextValue(int[] sequence) =>
        Reduce(sequence, sequence.Length,
        (value, aggregate) => value + aggregate,
        (sequence, sequenceLength) => sequence[sequenceLength - 1]);

    long GetPreviousValue(int[] sequence) =>
        Reduce(sequence, sequence.Length, 
        (value, aggregate) => value - aggregate, 
        (sequence, sequenceLength) => sequence[0]);

    T Reduce<T>(int[] sequence, int sequenceLength, Func<T, T, T> aggregateFunc, Func<int[], int, T> valueFunc)
    {
        if (sequenceLength == 0) return default;
        
        var value = valueFunc(sequence, sequenceLength);

        if (sequence.Take(sequenceLength--).All(n => n == 0)) return default;
        
        for ( int i = 0; i < sequenceLength; i++ ){
            sequence[i] = sequence[i+1] - sequence[i];
        }
        return aggregateFunc(value, Reduce<T>(sequence, sequenceLength, aggregateFunc, valueFunc));
    }

    public string Part1()
    {
        return Input.Split(Environment.NewLine)
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray())
            .Select(GetNextValue)
            .Sum().ToString();
    }

    public string Part2()
    {
        return Input.Split(Environment.NewLine)
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray())
            .Select(GetPreviousValue)
            .Sum().ToString();
    }
}