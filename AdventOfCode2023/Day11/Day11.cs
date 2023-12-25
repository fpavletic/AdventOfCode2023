namespace AdventOfCode2023.Day11;

internal partial class Day11 : ISolution
{
    private bool[][] Grid {get;}
    private SortedSet<int> EmptyRowIndices {get;}
    private SortedSet<int> EmptyColumnIndices{get;}
    private (int rowIndex, int columnIndex)[] Galaxies {get;}
    public Day11(){
        Grid = ParseGrid();
        EmptyRowIndices = GetEmptyMemberIndices(Grid);
        EmptyColumnIndices = GetEmptyMemberIndices(Grid
            .SelectMany(boolArray => 
                boolArray.Select((bool @bool, int columnIndex) => (@bool, columnIndex)))
            .GroupBy(t => t.columnIndex)
            .Select(g => g.Select(t => t.@bool).ToArray()));
        Galaxies = Grid.SelectMany((boolArray, rowIndex) => boolArray.Select((bool @bool, int columnIndex) => (@bool, rowIndex, columnIndex)))
            .Where(t => t.@bool)
            .Select(t => (t.rowIndex, t.columnIndex))
            .ToArray(); 
    }

    private bool[][] ParseGrid() => Input.Split(Environment.NewLine)
            .Select(l => l.Select(c => c == '#').ToArray())
            .ToArray(); 

    private static SortedSet<int> GetEmptyMemberIndices(IEnumerable<bool[]> input) => 
        input.Select((boolArray, index) => (boolArray, index))
            .Where(t => t.boolArray.All(@bool => !@bool))
            .Select(t => t.index)
            .Aggregate(new SortedSet<int>(), (tree, index) => {
                tree.Add(index);
                return tree;
            });

    static long GetOneDimensionalDistance(int valA, int valB, SortedSet<int> emptyIndices, int emptyIndexMultiplier) => valA > valB
            ? valA - valB + emptyIndices.GetViewBetween(valB, valA).Count * (emptyIndexMultiplier - 1)
            : valB - valA + emptyIndices.GetViewBetween(valA, valB).Count * (emptyIndexMultiplier - 1);

    long GetGalaxyDistanceSum(int emptyIndexMultiplier){
        var distanceSum = 0L;
        for ( int i = 0; i < Galaxies.Length; i++ ){
            for ( int j = i + 1; j < Galaxies.Length; j++ ){
                var galaxyA = Galaxies[i];
                var galaxyB = Galaxies[j];
                distanceSum += GetOneDimensionalDistance(galaxyA.rowIndex, galaxyB.rowIndex, EmptyRowIndices, emptyIndexMultiplier);
                distanceSum += GetOneDimensionalDistance(galaxyA.columnIndex, galaxyB.columnIndex, EmptyColumnIndices, emptyIndexMultiplier);
            }
        }    
        return distanceSum;
    }

    public string Part1() => GetGalaxyDistanceSum(2).ToString();

    public string Part2() => GetGalaxyDistanceSum(1000000).ToString();
}