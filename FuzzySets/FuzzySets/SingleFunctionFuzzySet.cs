using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
    public sealed class SingleFunctionFuzzySet : MultiDimFuzzySet
    {
        /// <summary>
        /// Membership functin instance
        /// </summary>
        public Func<IEnumerable<double>, double> FunctionFlv { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flv">One dimensional membership function</param>
        /// <param name="dim">Dimension in which membership function is as given</param>
        public SingleFunctionFuzzySet(Func<double, double> flv, int dim = 1)
        {
            FunctionFlv = x => flv(x.ElementAt(dim-1));
            Dim = 1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flv">Membership function</param>
        /// <param name="dim">Number of dimensions</param>
        public SingleFunctionFuzzySet(Func<IEnumerable<double>, double> flv, int dim)
        {
            FunctionFlv = flv;
            Dim = dim;
        }

        public SingleFunctionFuzzySet(SingleDimFuzzySet s)
            : this(s.Flv)
        { }

        public SingleFunctionFuzzySet(MultiDimFuzzySet s)
            : this(s.Flv, s.Dim)
        { }

        /// <summary>
        /// Membership function. If dimension of given point and this fuzzy set instance doesn't match throws ArgumentException
        /// </summary>
        /// <param name="value">Point which membership is to compute</param>
        /// <returns>Value of membership of given point</returns>
        public override double Flv(IEnumerable<double> value)
        {
            var enumerable = value as IList<double> ?? value.ToList();
            if (enumerable.Count() != Dim) throw new ArgumentException();
            return FunctionFlv(enumerable);
        }

        /// <summary>
        /// Finds intersection of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of input sets</returns>
        public static SingleFunctionFuzzySet Intersection(SingleFunctionFuzzySet s1, SingleFunctionFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return (SingleFunctionFuzzySet) MultiDimFuzzySet.Intersection(s1, s2, norm);
        }

        /// <summary>
        /// Finds union of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of input sets</returns>
        public static SingleFunctionFuzzySet Union(SingleFunctionFuzzySet s1, SingleFunctionFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return (SingleFunctionFuzzySet)MultiDimFuzzySet.Union(s1, s2, norm);
        }

        /// <summary>
        /// Finds union of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set union with</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of this and input fuzzy set</returns>
        public SingleFunctionFuzzySet UnionWith(SingleFunctionFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Union(this, other, norm);
        }

        /// <summary>
        /// Finds intersection of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set intersect with</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of this and input fuzzy set</returns>
        public SingleFunctionFuzzySet IntersectWith(SingleFunctionFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Intersection(this, other, norm);
        }
    }
}
