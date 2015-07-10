using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
    public abstract class MultiDimFuzzySet
    {

        /// <summary>
        /// Membership function.
        /// </summary>
        /// <param name="value">Point which membership is to compute</param>
        /// <returns>Membership of given point</returns>
        public abstract double Flv(IEnumerable<double> value);

        /// <summary>
        /// Membership function. If dimensions of point and instance of this class doesn't match throws ArgumentException.
        /// </summary>
        /// <param name="value">Point which membership is to compute</param>
        /// <returns>Membership of given point</returns>
        public double this[IEnumerable<double> value]
        {
            get
            {
                var enumerable = value as IList<double> ?? value.ToList();
                if(enumerable.Count() != Dim) throw new ArgumentException();
                return Flv(enumerable);
            }
        }

        /// <summary>
        /// Dimension of fuzzy set.
        /// </summary>
        public virtual int Dim
        {
            get;
            protected set;
        }

        /// <summary>
        /// Computes cartesian product of give fuzzy sets.
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Cartesian product of given sets</returns>
        public static SingleFunctionFuzzySet CartesianProduct(MultiDimFuzzySet s1, MultiDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            var dim = s1.Dim + s2.Dim;
            Func<IEnumerable<double>, double> flv = x =>
            {
                var enumerable = x as IList<double> ?? x.ToList();
                if (enumerable.Count()!= dim) throw new ArgumentException();
                return Norms.ApplyTNorm(s1.Flv(enumerable.Take(s1.Dim)), s2.Flv(enumerable.Skip(s1.Dim)));
            };
            return new SingleFunctionFuzzySet(flv, dim);
        }

        /// <summary>
        /// Finds complement fuzzy set.
        /// </summary>
        /// <param name="s">Fuzzy set to complement</param>
        /// <returns>Fuzzy set which is complementary to input fuzzy set</returns>
        public static MultiDimFuzzySet Complement(MultiDimFuzzySet s)
        {
            return new SingleFunctionFuzzySet(x => 1 - s.Flv(x), s.Dim);
        }

        /// <summary>
        /// Finds intersection of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of input sets</returns>
        public static MultiDimFuzzySet Intersection(MultiDimFuzzySet s1, MultiDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            if (s1.Dim != s2.Dim) throw new ArgumentException();
            return new SingleFunctionFuzzySet(x =>
            {
                var enumerable = x as IList<double> ?? x.ToList();
                return Norms.ApplyTNorm(s1.Flv(enumerable), s2.Flv(enumerable), norm);
            }, s1.Dim);
        }

        /// <summary>
        /// Finds union of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of input sets</returns>
        public static MultiDimFuzzySet Union(MultiDimFuzzySet s1, MultiDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            if (s1.Dim != s2.Dim) throw new ArgumentException();
            return new SingleFunctionFuzzySet(x =>
            {
                var enumerable = x as IList<double> ?? x.ToList();
                return Norms.ApplySNorm(s1.Flv(enumerable), s2.Flv(enumerable), norm);
            }, s1.Dim);
        }

        /// <summary>
        /// Finds union of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set union with</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of this and input fuzzy set</returns>
        public MultiDimFuzzySet UnionWith(MultiDimFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Union(this, other, norm);
        }

        /// <summary>
        /// Finds intersection of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set intersect with</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of this and input fuzzy set</returns>
        public MultiDimFuzzySet IntersectWith(MultiDimFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Intersection(this, other, norm);
        }

        /// <summary>
        /// Generates new SingleFunctionFuzzySet based on instance of this class.
        /// </summary>
        /// <returns>This class instance conversed to SingleFunctionFuzzySet</returns>
        public SingleFunctionFuzzySet ToSingleFunctionFuzzySet()
        {
            return new SingleFunctionFuzzySet(this);
        }

        /// <summary>
        /// Finds complement fuzzy set.
        /// </summary>
        /// <param name="s">Fuzzy set to complement</param>
        /// <returns>Fuzzy set which is complementary to input fuzzy set</returns>
        public static MultiDimFuzzySet operator ~(MultiDimFuzzySet s)
        {
            return Complement(s);
        }
    }
}
