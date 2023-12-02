

using AdventOfCode2023.Day01;
using AdventOfCode2023.Day02;

var domainDirectory = AppDomain.CurrentDomain.BaseDirectory;
var projectDirectory = Directory.GetParent(domainDirectory)?.Parent?.Parent?.Parent;
Console.WriteLine($"Project dir: {projectDirectory}");

var day = string.Format("{0:D2}", DateTime.Now.Date.Day);
//TODO reflexion the solution and download the input automatically
// var solution = new Day01();
// Console.WriteLine(solution.Part1(Day01.Input));
// Console.WriteLine(solution.Part2(Day01.Input));
var solution = new Day02();
Console.WriteLine(solution.Part1(Day02.Input));
Console.WriteLine(solution.Part2(Day02.Input));

namespace AdventOfCode2023
{
    public interface ISolution{

        public string Part1(string input);

        public string Part2(string input);

    }
}