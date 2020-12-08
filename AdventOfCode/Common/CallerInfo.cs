using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{
    
    /// <summary>
    /// Stores information relating to where
    /// a call was made.
    /// </summary>
    public readonly struct CallerInfo
    {
        
        //--------------------------------------------------
        /// <summary>
        /// Construct the CallerInfo
        /// </summary>
        /// <param name="filePath">If left unset, the compiler fills this in with where this constructor was called</param>
        /// <param name="lineNumber">If left unset, the compiler fills this in with where this constructor was called</param>
        /// <param name="memberName">If left unset, the compiler fills this in with where this constructor was called</param>
        public CallerInfo(
            [CallerFilePath, NotNull] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName, NotNull] string memberName = "")
        {
            this.FilePath = filePath;
            this.LineNumber = lineNumber;
            this.MemberName = memberName;
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// The path of the file that the call was made from.
        /// </summary>
        [NotNull] public readonly string FilePath;
        
        
        //--------------------------------------------------
        /// <summary>
        /// The line number in the file the call was made from.
        /// </summary>
        public readonly int LineNumber;
        
        
        //--------------------------------------------------
        /// <summary>
        /// The member name of where the call was made from.
        /// </summary>
        [NotNull] public readonly string MemberName;
        
    }
    
}
