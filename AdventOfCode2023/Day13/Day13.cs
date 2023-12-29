
namespace AdventOfCode2023.Day13;

internal partial class Day13 : ISolution
{

    private bool IsRowSymetrical(char[][] grid, int rowIndex, int expectedMismatchCount){
        var actualMismatchCount = 0;
        for (int j = 0; j < grid[0].Length; j++){ //for every column
            for ( int i = 0; rowIndex - i - 1 >= 0 && rowIndex + i < grid.Length; i++){ //get members equally far apart from the rowIndex
                if (grid[rowIndex - i - 1][j] != grid[rowIndex + i][j]) actualMismatchCount++; //and check if any are a mismatch
                if (actualMismatchCount > expectedMismatchCount) return false; //early exit
            }
        }
        return actualMismatchCount == expectedMismatchCount;
    }

    private bool IsColumnSymetrical(char[][] grid, int columnIndex, int expectedMismatchCount){
        var actualMismatchCount = 0;
        for (int i = 0; i < grid.Length; i++){ //for every row
            for ( int j = 0; columnIndex - j - 1 >= 0 && columnIndex + j < grid[0].Length; j++){ //get members equally far apart from columnIndex
                if (grid[i][columnIndex - j - 1] != grid[i][columnIndex + j]) actualMismatchCount++; //and check if any are a mismatch
                if (actualMismatchCount > expectedMismatchCount ) return false; //early exit
            }
        }
        return actualMismatchCount == expectedMismatchCount;
    }

    private long GetMirrorLine(char[][] grid, int mismatchCount)
    {
        var rowsAboveMirrorRowsSum = Enumerable.Range(1, grid.Length - 1)
            .Where(i => IsRowSymetrical(grid, i, mismatchCount))
            .Select(i => i * 100)
            .Sum();

        var columnsLeftOfMirrorColumnSum = Enumerable.Range(1, grid[0].Length -1)
            .Where(i => IsColumnSymetrical(grid, i, mismatchCount))
            .Sum();

        return rowsAboveMirrorRowsSum + columnsLeftOfMirrorColumnSum;
    }

    public string Part1()
    {
        return Input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(g => g.Split(Environment.NewLine).Select(r => r.ToCharArray()).ToArray())
            .Select(g => GetMirrorLine(g, 0))
            .Sum().ToString();
    }

    public string Part2()
    {
        return Input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(g => g.Split(Environment.NewLine).Select(r => r.ToCharArray()).ToArray())
            .Select(g => GetMirrorLine(g, 1))
            .Sum().ToString();
    }
}