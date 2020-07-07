namespace Common {
    public static class IntExtensions {
        /// <summary>
        /// Returns whether an integer is in the range [min, max)
        /// </summary>
        /// <param name="n">The integer to check</param>
        /// <param name="min">Inclusive lower bound</param>
        /// <param name="max">Exclusive upper bound</param>
        /// <returns></returns>
        public static bool Between(this int n, int min, int max) {
            return (n >= min && n < max);
        }
    }
}