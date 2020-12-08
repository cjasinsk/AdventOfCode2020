using JetBrains.Annotations;

namespace AdventOfCode.Common
{
    
    /// <summary>
    /// Methods to validate multiple results at once
    /// </summary>
    public static class Validate
    {

        //--------------------------------------------------
        /// <summary>
        /// Validate 2 results in parallel
        /// </summary>
        public static Result<(T1, T2)> All<T1, T2>(
            [NotNull] string id,
            [NotNull] string message,
            [NotNull] Result<T1> result1,
            [NotNull] Result<T2> result2)
        {
            var (v1, e1) = result1;
            var (v2, e2) = result2;

            return !(e1 is null) || !(e2 is null)
                ? new Error(id, message)[e1, e2]
                : (v1, v2).Success();
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Validate 7 results in parallel
        /// </summary>
        public static Result<(T1, T2, T3, T4, T5, T6, T7)> All<T1, T2, T3, T4, T5, T6, T7>(
            [NotNull] string id,
            [NotNull] string message,
            [NotNull] Result<T1> result1,
            [NotNull] Result<T2> result2,
            [NotNull] Result<T3> result3,
            [NotNull] Result<T4> result4,
            [NotNull] Result<T5> result5,
            [NotNull] Result<T6> result6,
            [NotNull] Result<T7> result7)
        {
            var (v1, e1) = result1;
            var (v2, e2) = result2;
            var (v3, e3) = result3;
            var (v4, e4) = result4;
            var (v5, e5) = result5;
            var (v6, e6) = result6;
            var (v7, e7) = result7;

            return !(e1 is null) || !(e2 is null) || !(e3 is null) || !(e4 is null) || !(e5 is null) || !(e6 is null) || !(e7 is null)
                ? new Error(id, message)[e1, e2, e3, e4, e5, e6, e7]
                : (v1, v2, v3, v4, v5, v6, v7).Success();
        }
    }
}
