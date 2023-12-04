using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day03;

internal partial class Day03 : ISolution
{
    private static bool IsDigit(char c) => c - '0' is >= 0 and < 10;
    
    private static bool IsSymbol(char c) => !IsDigit(c) && c != '.';

    private static bool TryGetChar(string input, int index, out char character){
        var isIndexInBounds = index >= 0 && index < input.Length -1;
        character = isIndexInBounds ? input[index] : '.';
        return isIndexInBounds;
    }

    static ISet<(int index, char character)> GetAdjecentSymbols(string input, int inputLineLength, int index, int length)
    {
        return Enumerable.Range(index - 1, length + 2)
            .SelectMany(index => new int[]{index - inputLineLength, index, index + inputLineLength})
            .Select(index => (index, character: TryGetChar(input, index, out var character) ? character : '.'))
            .Where(t => IsSymbol(t.character))
            .ToHashSet();
    }
    const string PartNumberPattern = "\\d+";

    public string Part1()
    {
        static bool IsValidPartNumber(string input, int inputLineLength, int index, int length) => 
            GetAdjecentSymbols(input, inputLineLength, index, length).Any();

        var lineLength = Input.IndexOf(Environment.NewLine) + Environment.NewLine.Count();
        return Regex.Matches(Input, PartNumberPattern)
            .Where(match => IsValidPartNumber(Input, lineLength, match.Index, match.Length))
            .Select(match => int.Parse(match.ValueSpan))
            .Sum().ToString();
    }

    public string Part2()
    {   
        var lineLength = Input.IndexOf(Environment.NewLine) + Environment.NewLine.Count();
        return Regex.Matches(Input, PartNumberPattern)
            .SelectMany(match => GetAdjecentSymbols(Input, lineLength, match.Index, match.Length)
                .Where(symbol => symbol.character == '*')
                .Select(symbol => (partNumber: int.Parse(match.Value), symbol.index)))
            .GroupBy(symbol => symbol.index)
            .Where(group => group.Count() == 2)
            .Select(group => group.Aggregate(1, (t, p) => t * p.partNumber))
            .Sum().ToString();
    }
}