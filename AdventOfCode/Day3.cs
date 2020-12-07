using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Day3
    {

        //--------------------------------------------------
        public static Day Run([NotNull] IEnumerable<string> input)
        {
            var trees = new Trees(input);
            
            return new Day(
                title: "--- Day 3: Toboggan Trajectory ---",
                partA: Day3.Run(trees, new Slope(3, 1)).ToString(),
                partB: Day3.Run(trees, 
                        new Slope(1, 1),
                        new Slope(3, 1),
                        new Slope(5, 1),
                        new Slope(7, 1),
                        new Slope(1, 2))
                    .ToString());
        }

        
        //--------------------------------------------------
        /// <summary>
        /// Given a set of trees, and a set of slopes,
        /// calculate how many trees are hit per slope,
        /// then multiple all trees hit per slope together.
        /// </summary>
        private static long Run(Trees trees, [NotNull] params Slope[] slopes)
        {
            if (slopes is null) { throw new ArgumentNullException(nameof(slopes)); }

            return slopes.Aggregate(1L, (total, slope) =>
            {
                var count = 0;
                var toboggan = new Toboggan(0, 0, slope);
                for (var i = 0; i < trees.Height; i += 1)
                {
                    count = trees[toboggan++]
                        ? count + 1
                        : count;
                }

                return total * count;
            });
        }


        //--------------------------------------------------
        /// <summary>
        /// The position and slope of trajectory of the toboggan.
        /// </summary>
        private readonly struct Toboggan
        {
            
            //--------------------------------------------------
            public Toboggan(int x, int y, Slope slope)
            {
                this.X = x;
                this.Y = y;
                this._slope = slope;
            }
            
            
            //--------------------------------------------------
            /// <summary>
            /// Move the toboggan's position by one slope
            /// increment.
            /// </summary>
            public static Toboggan operator ++(Toboggan toboggan)
                => new Toboggan(toboggan.X + toboggan._slope.X, toboggan.Y + toboggan._slope.Y, toboggan._slope);
            
            
            //--------------------------------------------------
            public readonly int X;
            public readonly int Y;
            private readonly Slope _slope;
            
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// The slope of the toboggan
        /// </summary>
        private readonly struct Slope
        {
            public Slope(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public readonly int X;
            public readonly int Y;
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// A map of trees. The map infinitely repeats to the
        /// right, but does not repeat vertically.
        /// </summary>
        private readonly struct Trees
        {

            //--------------------------------------------------
            /// <summary>
            /// Converts string input into a map of trees,
            /// where '#' indicates a tree.
            /// </summary>
            public Trees([NotNull] IEnumerable<string> input)
            {
                if (input is null) { throw new ArgumentNullException(nameof(input)); }
                this._trees = input.ToArray();
            }

            
            //--------------------------------------------------
            /// <summary>
            /// Identifies if a toboggan hits a tree.
            /// </summary>
            public bool this[Toboggan toboggan]
                => toboggan.Y >= 0 
                    && (toboggan.Y < this.Height 
                        && this._trees[toboggan.Y][toboggan.X % this._trees[toboggan.Y].Length] == '#');

            
            //--------------------------------------------------
            /// <summary>
            /// The height of the tree line.
            /// </summary>
            public int Height
                => this._trees.Length;
            
            
            //--------------------------------------------------
            private readonly string[] _trees;
            
        }
        
    }
    
}
