using System;

namespace FuzzySets
{
    /// <summary>
    /// Implemented norms
    /// </summary>
    public enum Norm
    {
        Zadeh,
        Einstein,
        Algebraic,
        Lukasiewicz
    }

    /// <summary>
    /// Implements computing of S- and T-norms
    /// </summary>
    public static class Norms
    {
        /// <summary>
        /// Applies given S-Norm to given values
        /// Zadeh - max(a,b)
        /// Algebraic - a+b-a*b
        /// Lukasiewicz - Min(1,a+b)
        /// Einstein - (a+b)/(1+a*b)
        /// </summary>
        /// <param name="a">Input value</param>
        /// <param name="b">Input value</param>
        /// <param name="norm">Input norm</param>
        /// <returns>Value of a norm</returns>
        public static double ApplySNorm(double a, double b, Norm norm=Norm.Zadeh)
        {
            switch (norm)
            {
                case Norm.Zadeh:
                    return Math.Max(a, b);
                case Norm.Algebraic:
                    return a + b - a * b;
                case Norm.Lukasiewicz:
                    return Math.Min(1, a + b);
                case Norm.Einstein:
                    return (a + b) / (1 + a * b);
                default: throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Applies given T-Norm to given values
        /// Zadeh - Min(a,b)
        /// Algebraic - a*b
        /// Lukasiewicz - Max(0,a+b-1)
        /// Einstein - (a*b)/(2-(a+b-a*b))
        /// </summary>
        /// <param name="a">Input value</param>
        /// <param name="b">Input value</param>
        /// <param name="norm">Input norm</param>
        /// <returns>Value of a norm</returns>
        public static double ApplyTNorm(double a, double b, Norm norm = Norm.Zadeh)
        {
            switch (norm)
            {
                case Norm.Zadeh:
                    return Math.Min(a, b);
                case Norm.Algebraic:
                    return a * b;
                case Norm.Lukasiewicz:
                    return Math.Max(0, a + b - 1);
                case Norm.Einstein:
                    return a * b / (2 - (a + b - a * b));
                default: throw new InvalidOperationException();
            }
        }
    }

}
