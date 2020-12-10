using System.Threading.Tasks;

using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    public static class Day5Tests
    {
        
        [Test]
        public static async Task TicketTests()
        {
            var ticket = await Day5.Ticket.From("FBFBBFFRLR");
            Assert.AreEqual(44, ticket.Row);
            Assert.AreEqual(5, ticket.Column);
            Assert.AreEqual(357, ticket.Id);
            
            ticket = await Day5.Ticket.From("BFFFBBFRRR");
            Assert.AreEqual(70, ticket.Row);
            Assert.AreEqual(7, ticket.Column);
            Assert.AreEqual(567, ticket.Id);
            
            ticket = await Day5.Ticket.From("FFFBBBFRRR");
            Assert.AreEqual(14, ticket.Row);
            Assert.AreEqual(7, ticket.Column);
            Assert.AreEqual(119, ticket.Id);
            
            ticket = await Day5.Ticket.From("BBFFBBFRLL");
            Assert.AreEqual(102, ticket.Row);
            Assert.AreEqual(4, ticket.Column);
            Assert.AreEqual(820, ticket.Id);
        }
        
        
        [Test]
        public static async Task Day5Test()
            => Assert.AreEqual(
                "820",
                Day5.RunPartA(await Day5.ParseInput(
                    new [] {"FBFBBFFRLR", "BFFFBBFRRR", "FFFBBBFRRR", "BBFFBBFRLL" })));
    }
}
