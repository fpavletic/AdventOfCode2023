using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2023.Day14;

internal partial class Day14 : ISolution
{
    private SortedSet<int>[] _rows;
    private int _rowWidth;
    private SortedSet<int>[] _columns;
    private int _columnHeight;
    private Dictionary<(int rowIndex, int columnIndex), char> _indexToCharacter;

    private void Init(){
        static SortedSet<T> AddToSortedSet<T>(SortedSet<T> set, T item){
            set.Add(item);
            return set;
        }
        var grid = Input.Split(Environment.NewLine);
        _rowWidth = grid[0].Length;
        _columnHeight = grid.Length;
        var gridItems = grid
            .SelectMany((row, rowIndex) => row
                .Select((item, columnIndex) => (item, rowIndex, columnIndex))
                .Where(t => t.item != '.'))
            .ToArray();
        _rows = gridItems.GroupBy(t => t.rowIndex)
            .OrderBy(g => g.Key)
            .Select(g => g.Select(t => t.columnIndex)
                .Aggregate(new SortedSet<int>(), AddToSortedSet))
            .ToArray();
        _columns = gridItems.GroupBy(t => t.columnIndex)
            .OrderBy(g => g.Key)
            .Select(g => g.Select(t => t.rowIndex)
                .Aggregate(new SortedSet<int>(), AddToSortedSet))
            .ToArray();
        _indexToCharacter = gridItems.ToDictionary(t => (t.rowIndex, t.columnIndex), t => t.item); 
    }

    private void UpdateDateStructures(int rowIndex, int newRowIndex, int columnIndex, int newColumnIndex){
        _rows[rowIndex].Remove(columnIndex);
        _rows[newRowIndex].Add(newColumnIndex);
        _columns[columnIndex].Remove(rowIndex);
        _columns[newColumnIndex].Add(newRowIndex);
        if ( rowIndex != newRowIndex || columnIndex != newColumnIndex) {
            _indexToCharacter[(newRowIndex, newColumnIndex)] = 'O';
            _indexToCharacter.Remove((rowIndex, columnIndex));
        }
    }

    private void PushUp(){
        for(int columnIndex = 0; columnIndex < _columns.Length; columnIndex++){
            var lastRowIndex = 0;
            for(int stoneIndex = 0; stoneIndex < _columns[columnIndex].Count; stoneIndex ++){
                var rowIndex = _columns[columnIndex].GetViewBetween(lastRowIndex, int.MaxValue).Min;
                lastRowIndex = rowIndex + 1;
                if ( _indexToCharacter[(rowIndex, columnIndex)] == '#') continue;
                var newRowIndex = rowIndex <= _columns[columnIndex].Min
                    ? 0
                    : _columns[columnIndex].GetViewBetween(0, rowIndex - 1).Max + 1;
                UpdateDateStructures(rowIndex, newRowIndex, columnIndex, columnIndex);
            };
        }
    }

    private void PushLeft(){
        for (int rowIndex = 0; rowIndex < _rows.Length; rowIndex++){
            var lastColumnIndex = 0;
            for (int stoneIndex = 0; stoneIndex < _rows[rowIndex].Count; stoneIndex++){
                var columnIndex = _rows[rowIndex].GetViewBetween(lastColumnIndex, int.MaxValue).Min;
                lastColumnIndex = columnIndex + 1;
                if (_indexToCharacter[(rowIndex, columnIndex)] == '#') continue;
                var newColumnIndex = columnIndex <= _rows[rowIndex].Min
                    ? 0
                    : _rows[rowIndex].GetViewBetween(0, columnIndex - 1).Max + 1;
                UpdateDateStructures(rowIndex, rowIndex, columnIndex, newColumnIndex);
            }
        }
    }

    private void PushDown(){
        for (int columnIndex = 0; columnIndex < _columns.Length; columnIndex++){
            var lastRowIndex = _columnHeight - 1;
            for(int stoneIndex = 0; stoneIndex < _columns[columnIndex].Count; stoneIndex ++){
                var rowIndex = _columns[columnIndex].GetViewBetween(0, lastRowIndex).Max;
                lastRowIndex = rowIndex - 1;
                if ( _indexToCharacter[(rowIndex, columnIndex)] == '#') continue;
                var newRowIndex = rowIndex >= _columns[columnIndex].Max
                    ? _columnHeight - 1
                    : _columns[columnIndex].GetViewBetween(rowIndex + 1, _columnHeight).Min - 1;
                UpdateDateStructures(rowIndex, newRowIndex, columnIndex, columnIndex);
            };
        }
    }

    private void PushRight(){
        for (int rowIndex = 0; rowIndex < _rows.Length; rowIndex++){
            var lastColumnIndex = _rowWidth - 1;
            for (int stoneIndex = 0; stoneIndex < _rows[rowIndex].Count; stoneIndex++){
                var columnIndex = _rows[rowIndex].GetViewBetween(0, lastColumnIndex).Max;
                lastColumnIndex = columnIndex - 1;
                if (_indexToCharacter[(rowIndex, columnIndex)] == '#') continue;
                var newColumnIndex = columnIndex >= _rows[rowIndex].Max
                    ? _rowWidth - 1 
                    : _rows[rowIndex].GetViewBetween(columnIndex + 1, _rowWidth).Min - 1;
                UpdateDateStructures(rowIndex, rowIndex, columnIndex, newColumnIndex);
            }
        }
    }

    private void SpinStones(int iterations){
        for ( int i = 0; i < iterations; i++){
            PushUp();
            PushLeft();
            PushDown();
            PushRight();
        }
    }

    private bool FindCycle(int iterationCap, out int cycleLength, out int cycleOffset){
        var states = new LinkedList<(HashSet<(int rowIndex, int columnIndex)> set, int iteration)>();
        var iteration = 0;
        while (iteration != iterationCap ){
            states.AddLast((new HashSet<(int rowIndex, int columnIndex)>(_indexToCharacter.Keys), iteration++));
            SpinStones(1);
            var matchingState = states.FirstOrDefault(oldState => oldState.set.SetEquals(_indexToCharacter.Keys));
            if (matchingState != default){
                cycleLength = iteration - matchingState.iteration;
                cycleOffset = matchingState.iteration;
                return true;
            }
        };

        cycleLength = -1;
        cycleOffset = -1;
        return false;
    }

    private int ScoreStones(){
        return _indexToCharacter.Where(t => t.Value == 'O')
            .Select(t => _columnHeight - t.Key.rowIndex)
            .Sum();
    }

    public string Part1()
    {
        Init();
        PushUp();
        return ScoreStones().ToString();
    }

    public string Part2()
    {
        Init();
        FindCycle(-1, out var cycleLength, out var cycleOffset);
        SpinStones((1_000_000_000 - cycleLength - cycleOffset) % cycleLength);
        return ScoreStones().ToString();
    }
}