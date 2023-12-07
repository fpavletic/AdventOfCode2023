namespace AdventOfCode2023.Day07;

internal partial class Day07 : ISolution
{
    int ScoreHand(string hand, bool joker){

        var groups = hand.GroupBy(c => c);
        
        int ScoreGroup(string hand, bool joker){
            
            int jokerOffset = 0;
            if ( joker && groups.Any(g => g.Key == 'J')){
                jokerOffset = groups.First(g => g.Key == 'J').Count();
                groups = groups.Where(g => g.Key != 'J');     
            }
            if (jokerOffset == 5) return 7;

            if ( groups.Any(g => g.Count() + jokerOffset == 5)){ return 7; } // Five of a kind, WTF!?
            if ( groups.Any(g => g.Count() + jokerOffset == 4)){ return 6; } // Poker
            if ( groups.Count() == 2){ return 5;} //FH 
            if ( groups.Any(g => g.Count() + jokerOffset == 3)) { return 4; } //Three of a kind
            if ( groups.Count() == 3){ return 3;} //Two pairs
            if ( groups.Count() == 4){ return 2; } //Pair
            return 1; //High card
        }

        int ScoreSymbol(char c, bool joker) => c switch {
            'T' => 10,
            'J' => joker ? 0 : 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => c - '0'
        };

        var score = ScoreGroup(hand, joker);
        foreach(var symbol in hand){
            score = score << 4; //Hex should be enough, there are only 14 cards
            score |= ScoreSymbol(symbol, joker);
        }
        return score;
    }

    public string Part1() //249483956
    {
        return Input.Split(Environment.NewLine)
            .Select(l => l.Split(' '))
            .Select(s => (score: ScoreHand(s[0], false), bid: s[1]))
            .OrderBy(t => t.score)
            .Select((t, i) => long.Parse(t.bid) * (i + 1))
            .Sum().ToString();
    }

    public string Part2() //252137472 
    {
        return Input.Split(Environment.NewLine)
            .Select(l => l.Split(' '))
            .Select(s => (score: ScoreHand(s[0], true), bid: s[1]))
            .OrderBy(t => t.score)
            .Select((t, i) => long.Parse(t.bid) * (i + 1))
            .Sum().ToString();
    }
}