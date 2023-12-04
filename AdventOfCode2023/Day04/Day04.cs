using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day04;

internal partial class Day04 : ISolution
{
    private const string LinePattern = "Card\\s+(\\d+):([\\d\\s]+)\\|([\\d\\s]+)"; 

    private static IEnumerable<(int ticketNumber, int winningNumberCount)> ParseTickets(string input){
        return input.Split(Environment.NewLine)
            .Select(l => Regex.Match(l, LinePattern))
            .Select(m => (ticket: int.Parse(m.Groups[1].Value.Trim()), 
                winningNumbers: m.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet(),
                ticketNumbers: m.Groups[3].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)))
            .Select(t => (t.ticket, t.winningNumbers.Where(n => t.ticketNumbers.Contains(n)).Count()));
    }

    public string Part1()
    {
        return ParseTickets(Input)
            .Where(ticket => ticket.winningNumberCount != 0)
            .Select(ticket => Math.Pow(2, ticket.winningNumberCount - 1))
            .Sum().ToString();
    }

    public string Part2()
    {
        Dictionary<int, long> ScoreTicket(Dictionary<int, long> previousTicketScores, (int ticketNumber, int winningNumberCount) ticket){
            previousTicketScores[ticket.ticketNumber] = 1 + Enumerable.Range(ticket.ticketNumber + 1, ticket.winningNumberCount)
                .Select(n => previousTicketScores[n]).Sum();
            return previousTicketScores;
        }

        var tickets = ParseTickets(Input)
            .Select(t => (t.ticketNumber, t.winningNumberCount))
            .OrderBy(t => -t.ticketNumber)
            .ToList();
        return tickets.Aggregate(new Dictionary<int, long>(), ScoreTicket)
            .Values
            .Sum().ToString();
    }
}