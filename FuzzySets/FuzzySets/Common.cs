using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzySets
{
    public enum Norm
    {
        Zadeh,
        Einstein,
        Algebraic,
        Lukasiewicz
    }

    public static class Norms
    {
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
