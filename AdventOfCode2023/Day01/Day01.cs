using System.Collections.Immutable;

namespace AdventOfCode2023.Day01;

internal partial class Day01 : ISolution
{
    public Day01()
    {
    }

    public string Part1(string input)
		{
            static bool IsDigit(char c) => c - '0' is >= 0 and < 10;
            var numbers =  input.Split(Environment.NewLine)
                .Select(line => (line.First(IsDigit)-'0') * 10 + (line.Last(IsDigit)-'0'))
                .ToList();
                
            return numbers.Sum().ToString();
		}

		public string Part2(string input)
		{
            return input.Split(Environment.NewLine)
                .Select(line => FindFirstNumber(line) * 10 + FindFirstNumber(line.Reverse(), true))
                .Sum().ToString();
		}

		private static int FindFirstNumber(IEnumerable<char> input, bool reverse = false)
		{
			var availableMachines = GetAvailableInitialStates(reverse);
			IEnumerable<Machine> activeMachines = new List<Machine>();
			foreach (char c in input)
			{
				activeMachines = activeMachines.Where(m => m.Transition(c));
				if (availableMachines.TryGetValue(c, out var newMachines))
				{
					activeMachines = activeMachines.Concat(newMachines.Select(m => m.Clone())).ToList();
				}

				Machine? completedMachine = activeMachines.FirstOrDefault(m => m.IsComplete, null);
				if ( completedMachine != null)
				{
					return completedMachine.Value;
				}
			}

			throw new ArgumentException();
		}

		private static IImmutableDictionary<char, List<Machine>> GetAvailableInitialStates(bool reverse)
		{ 
			var availableStates = Enumerable.Range(0, 10).Select(i => new Machine(i)).ToHashSet();
		    availableStates.Add(new Machine("zero", 0, reverse));
		    availableStates.Add(new Machine("one", 1, reverse));
		    availableStates.Add(new Machine("two", 2, reverse));
		    availableStates.Add(new Machine("three", 3, reverse));
		    availableStates.Add(new Machine("four", 4, reverse));
		    availableStates.Add(new Machine("five", 5, reverse));
		    availableStates.Add(new Machine("six", 6, reverse));
		    availableStates.Add(new Machine("seven", 7, reverse));
		    availableStates.Add(new Machine("eight", 8, reverse));
		    availableStates.Add(new Machine("nine", 9, reverse));
		    return availableStates.GroupBy(s => s.Initial)
			    .ToImmutableDictionary(m => m.Key, m => m.ToList());
		}

	private class Machine
		{
			public Machine(int number)
			{
				if (number is < 0 or > 9) throw new ArgumentException();
				_token = new[] { (char)(number + '0') };
                Value = number;
			}

			public Machine(string token, int value, bool reverse = false)
			{
				_token = reverse
					? token.Reverse().ToArray()
					: token.ToCharArray();
                Value = value;
			}

			private Machine(char[] token, int value)
			{
				_token = token;
                Value = value;
			}

			private int _index = 1;
			private readonly char[] _token;
            public int Value {get;}
			
			public char Initial => _token[0];

			public bool IsComplete => _index == _token.Length;

			public bool Transition(char c) => !IsComplete && _token[_index++] == c;

			public override string ToString() => new(_token);

			public Machine Clone() => new (_token, Value);
		}
}
