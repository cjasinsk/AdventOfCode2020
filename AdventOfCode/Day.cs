using System;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public readonly struct Day
    {

        //--------------------------------------------------
        public Day([NotNull] string title, [NotNull] string partA, [NotNull] string partB)
        {
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
            this.PartA = partA ?? throw new ArgumentNullException(nameof(partA));
            this.PartB = partB ?? throw new ArgumentNullException(nameof(partB));
        }

        [NotNull] public readonly string PartA;
        [NotNull] public readonly string PartB;
        [NotNull] public readonly string Title;


        //--------------------------------------------------
        public static bool operator ==(Day a, Day b)
            => a.Equals(b);

        
        //--------------------------------------------------
        public static bool operator !=(Day a, Day b)
            => !(a == b);


        //--------------------------------------------------
        public override bool Equals(object obj)
            => obj is Day day && this.Equals(day);    

        
        //--------------------------------------------------
        public bool Equals(Day other)
            => this.PartA == other.PartA 
                && this.PartB == other.PartB 
                && this.Title == other.Title;

        
        //--------------------------------------------------
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.PartA.GetHashCode();
                hashCode = (hashCode * 397) ^ this.PartB.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Title.GetHashCode();
                return hashCode;
            }
        }

    }

}
