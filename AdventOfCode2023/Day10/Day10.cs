using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2023.Day10;

internal partial class Day10 : ISolution
{
    private char[][] Grid {get;}

    private (int rowIndex, int columnIndex) StartPosition {get;}

    public Day10(){
        Grid = ParseGrid();
        StartPosition = GetStartPosition();
    }

    private char[][] ParseGrid() => Input.Split(Environment.NewLine)
            .Select(l => l.ToArray())
            .ToArray(); 

    private  (int rowIndex, int columnIndex) GetStartPosition(){
        var (_, startRow, startColumn) = Grid
            .SelectMany((line, rowIndex) => line.Select((symbol, columnIndex) => (symbol, rowIndex, columnIndex)))
            .First(t => t.symbol == 'S');
        return (startRow, startColumn);
    }

    private bool GetNextPipeSegment((int rowIndex, int columnIndex) s, LinkedList<(int, int)> v, out (int rowIndex, int columnIndex) nextPipeSegment){
        nextPipeSegment =  Grid[s.rowIndex][s.columnIndex] switch {
            'S' when v.Count == 0 => GetStartConnectorSegment(s.rowIndex, s.columnIndex),

            '-' when v.Last!.Value != (s.rowIndex, s.columnIndex + 1) => (s.rowIndex, s.columnIndex + 1),
            '-' when v.Last!.Value != (s.rowIndex, s.columnIndex - 1) => (s.rowIndex, s.columnIndex - 1),
            
            '|' when v.Last!.Value != (s.rowIndex + 1, s.columnIndex) => (s.rowIndex + 1, s.columnIndex),
            '|' when v.Last!.Value != (s.rowIndex - 1, s.columnIndex) => (s.rowIndex - 1, s.columnIndex),
            
            'F' when v.Last!.Value != (s.rowIndex + 1, s.columnIndex) => (s.rowIndex + 1, s.columnIndex),
            'F' when v.Last!.Value != (s.rowIndex, s.columnIndex + 1) => (s.rowIndex, s.columnIndex + 1),  
            
            'L' when v.Last!.Value != (s.rowIndex - 1, s.columnIndex) => (s.rowIndex - 1, s.columnIndex),
            'L' when v.Last!.Value != (s.rowIndex, s.columnIndex + 1) => (s.rowIndex, s.columnIndex + 1),  
            
            'J' when v.Last!.Value != (s.rowIndex - 1, s.columnIndex) => (s.rowIndex - 1, s.columnIndex),  
            'J' when v.Last!.Value != (s.rowIndex, s.columnIndex - 1) => (s.rowIndex, s.columnIndex - 1),
            
            '7' when v.Last!.Value != (s.rowIndex + 1, s.columnIndex) => (s.rowIndex + 1, s.columnIndex),
            '7' when v.Last!.Value != (s.rowIndex, s.columnIndex - 1) => (s.rowIndex, s.columnIndex - 1),
            _ => (-1, -1)
        };
        return nextPipeSegment != (-1, -1);
    }

    private (int, int) GetStartConnectorSegment(int startRowIndex, int startColumnIndex){
        if ( Grid[startRowIndex - 1][startColumnIndex] is '|' or 'F' or '7') return (startRowIndex - 1, startColumnIndex);
        if ( Grid[startRowIndex + 1][startColumnIndex] is '|' or 'J' or 'L') return (startRowIndex + 1, startColumnIndex);
        if ( Grid[startRowIndex][startColumnIndex - 1] is '-' or 'F' or 'L') return (startRowIndex, startColumnIndex - 1);
        if ( Grid[startRowIndex][startColumnIndex + 1] is '-' or 'J' or '7') return (startRowIndex, startColumnIndex + 1);
        throw new Exception("Failed to link start segment to pipe");
    }

    private LinkedList<(int x, int y)> GetPipeSegments((int rowIndex, int columnIndex) startSegment){
        var visited = new LinkedList<(int, int)>();
        while ( GetNextPipeSegment(startSegment, visited, out var newSegment)){
            visited.AddLast(startSegment);
            startSegment = newSegment;
        }
        return visited;
    }

    public string Part1() => Math.Ceiling(GetPipeSegments(StartPosition).Count / 2.0).ToString();

    public string Part2() 
    {
        var pipeSegments = GetPipeSegments(StartPosition);

        //Shoelace
        static (int area, int lastRow, int lastColumn) AggregateFunc((int, int, int) aggregate, (int rowIndex, int columnIndex) vertex){
            var (area, lastRowindex, lastColumnIndex) = aggregate;
            return (area + (lastRowindex * vertex.columnIndex) - (lastColumnIndex * vertex.rowIndex), vertex.rowIndex, vertex.columnIndex);
        } 
        var area = AggregateFunc(pipeSegments.Aggregate((0, 0, 0), AggregateFunc), StartPosition).area / 2;
        
        //Pick's
        var boundaryVertices = pipeSegments.Count;
        var internalVertices = area - boundaryVertices / 2 + 1;
        return internalVertices.ToString();
    }
}