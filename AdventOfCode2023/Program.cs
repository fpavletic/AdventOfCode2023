using AdventOfCode2023;
using AdventOfCode2023.Day01;
using AdventOfCode2023.Day02;
using AdventOfCode2023.Day03;
using AdventOfCode2023.Day04;
using AdventOfCode2023.Day06;
using AdventOfCode2023.Day07;
using AdventOfCode2023.Day08;
using AdventOfCode2023.Day09;
using AdventOfCode2023.Day10;
using AdventOfCode2023.Day11;
using AdventOfCode2023.Day12;

var domainDirectory = AppDomain.CurrentDomain.BaseDirectory;
var projectDirectory = Directory.GetParent(domainDirectory)?.Parent?.Parent?.Parent;
Console.WriteLine($"Project dir: {projectDirectory}");

var year = DateTime.Now.Year;
var day = string.Format("{0:D2}", DateTime.Now.Date.Day);

//TODO reflexion the solution and download the input automatically
var solution = new Day12();
Console.WriteLine(solution.Part1());
Console.WriteLine(solution.Part2());

namespace AdventOfCode2023
{
    public interface ISolution{

        public string Part1();

        public string Part2();

    }
}