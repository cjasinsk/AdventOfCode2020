using System;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public sealed class Day
    {

        internal Day([NotNull] string title, [NotNull] string partA, [NotNull] string partB)
        {
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
            this.PartA = partA ?? throw new ArgumentNullException(nameof(partA));
            this.PartB = partB ?? throw new ArgumentNullException(nameof(partB));
        }

        [NotNull] public readonly string PartA;
        [NotNull] public readonly string PartB;
        [NotNull] public readonly string Title;

    }

}
