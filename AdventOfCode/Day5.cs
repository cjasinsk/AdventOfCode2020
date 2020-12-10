using System;
using System.Collections.Generic;
using System.Linq;

using AdventOfCode.Common;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Day5
    {
    
        //--------------------------------------------------
        public static async Result<Day> Run([NotNull] IEnumerable<string> input)
        {
            var parsed = await ("Parse Input", Day5.ParseInput(input));
            
            return new Day(
                "--- Day 5: Binary Boarding ---",
                Day5.RunPartA(parsed),
                Day5.RunPartB(parsed));
        }


        //--------------------------------------------------
        [NotNull]
        public static async Result<Ticket[]> ParseInput([NotNull] IEnumerable<string> input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return (await input.Select<string, Result<Ticket>>(async (x, i) =>
                    await Ticket.From(x, i.ToString())))
                .ToArray();
        }


        //--------------------------------------------------
        [NotNull]
        public static string RunPartA([NotNull] Ticket[] tickets)
            => tickets.Aggregate(0, (id, ticket) => ticket.Id > id ? ticket.Id : id).ToString();

        
        //--------------------------------------------------
        [NotNull]
        private static string RunPartB([NotNull] Ticket[] tickets)
        {
            var (min, max) = (-1, -1);
            var occupied = new HashSet<int>();
            foreach (var ticket in tickets)
            {
                if (occupied.Contains(ticket.Id)) { throw new InvalidOperationException("Id has already been set."); }
                
                occupied.Add(ticket.Id);
                if (min == -1) { min = max = ticket.Id; }

                min = ticket.Id < min ? ticket.Id : min;
                max = ticket.Id > max ? ticket.Id : max;
            }

            for (var i = min; i < max; i += 1)
            {
                if (!occupied.Contains(i) && occupied.Contains(i - 1) && occupied.Contains(i + 1)) { return i.ToString(); }
            }
            throw new InvalidOperationException("Unable to find empty seat.");
        }
        
        
        
        //--------------------------------------------------
        public sealed class Ticket
        {

            //--------------------------------------------------
            public static async Result<Ticket> From([NotNull] string ticket, [CanBeNull] string id = null)
            {
                if (ticket is null) { throw new ArgumentNullException(ticket); }
                
                return await Result.From($"Ticket {id}", async () =>
                {
                    if (ticket.Length != 10) { await new Failure<bool>("Length", "Expecting the ticket to be 10 characters long.", ticket); }

                    var rows = await Result.From("Rows", async () => await
                        ticket.Substring(0, 7).ToUpper()
                            .Select<char, Result<bool>>(async (c, i) => c switch
                            {
                                'F' => true,
                                'B' => false,
                                _ => await new Failure<bool>($"{i}", $"Unknown row type '{c}'.", c)
                            }));

                    var columns = await Result.From("Columns", async () => await 
                        ticket.Substring(7, 3).ToUpper()
                            .Select<char, Result<bool>>(async (c, i) => c switch
                            {
                                'L' => true,
                                'R' => false,
                                _ => await new Failure<bool>($"{i}", $"Unknown column type '{c}'.", c)
                            }));
                    
                    return new Ticket(
                        Ticket.ComputePosition(rows.ToArray(), 127),
                        Ticket.ComputePosition(columns.ToArray(), 7));
                });
            }

            
            //--------------------------------------------------
            public readonly int Column;
            public readonly int Id;
            public readonly int Row;

            
            //--------------------------------------------------
            private Ticket(int row, int column)
            {
                this.Row = row;
                this.Column = column;
                this.Id = (row * 8) + column;                
            }


            //--------------------------------------------------
            private static int ComputePosition([NotNull] IReadOnlyList<bool> items, int max)
            {
                var (front, back) = (0, max);
                for (var i = 0; i < items.Count - 1; i += 1)
                {
                    var middle = (back - front) / 2;
                    (front, back) = items[i] switch
                    {
                        true => (front, back - middle - 1),
                        false => (front + middle + 1, back)
                    };
                }
                
                return items[items.Count - 1] switch
                {
                    true => front,
                    false => back
                };
            }

        }
        
    }
    
}
