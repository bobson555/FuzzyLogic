using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
    public abstract class SingleDimFuzzySet : MultiDimFuzzySet
    {
        /// <summary>
        /// Generates new FuzzySet class instance based on instance of this class.
        /// </summary>
        /// <returns>This class instance conversed to FuzzySet</returns>
        public FuzzySet ToFuzzySet()
        {
            return new FuzzySet(FLV);
        }

        /// <summary>
        /// Membership function.
        /// </summary>
        /// <param name="value">Value which membership is to compute</param>
        /// <returns>Membership of given value</returns>
        public abstract double FLV(double value);

        /// <summary>
        /// Membership function. Throws exception if input point has more than 1 dimension.
        /// </summary>
        /// <param name="value">Point which membership is to compute</param>
        /// <returns>Membership of given point</returns>
        public override double FLV(IEnumerable<double> value)
        {
            if (value.Count() == 1) return FLV(value.First());
            throw new ArgumentException();
        }

        /// <summary>
        /// Membership function.
        /// </summary>
        /// <param name="value">Value which membership is to compute</param>
        /// <returns>Membership of given value</returns>
        public double this[double value]
        {
            get
            {
                return FLV(value);
            }
        }

        /// <summary>
        /// Finds intersection of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of input sets</returns>
        public static SingleDimFuzzySet Intersection(SingleDimFuzzySet s1, SingleDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return new FuzzySet(x => Norms.ApplyTNorm(s1.FLV(x), s2.FLV(x), norm));
        }

        /// <summary>
        /// Finds union of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of input sets</returns>
        public static SingleDimFuzzySet Union(SingleDimFuzzySet s1, SingleDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return new FuzzySet(x=> Norms.ApplySNorm(s1.FLV(x), s2.FLV(x), norm));
        }

        /// <summary>
        /// Finds union of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set union with</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of this and input fuzzy set</returns>
        public SingleDimFuzzySet UnionWith(SingleDimFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Union(this, other, norm);
        }

        /// <summary>
        /// Finds intersection of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set intersect with</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of this and input fuzzy set</returns>
        public SingleDimFuzzySet IntersectWith(SingleDimFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Intersection(this, other, norm);
        }

        /// <summary>
        /// Finds complement fuzzy set.
        /// </summary>
        /// <param name="s">Fuzzy set to complement</param>
        /// <returns>Fuzzy set which is complementary to input fuzzy set</returns>
        public static SingleDimFuzzySet Complement(SingleDimFuzzySet s)
        {
            return new FuzzySet(x => 1-s.FLV(x));
        }

        /// <summary>
        /// Dimension of fuzzy set.
        /// </summary>
        public override sealed int Dim
        {
            get { return 1; }
            protected set { throw new InvalidOperationException(); }
        }

        /// <summary>
        /// Finds complement fuzzy set.
        /// </summary>
        /// <param name="s">Fuzzy set to complement</param>
        /// <returns>Fuzzy set which is complementary to input fuzzy set</returns>
        public static SingleDimFuzzySet operator ~(SingleDimFuzzySet s)
        {
            return Complement(s);
        }
    }
}
