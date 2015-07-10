﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
    public class FuzzySet:SingleDimFuzzySet
    {
        public Func<double, double> FunctionFLV { get; private set; }

        public override double FLV(double value)
        {
            return FunctionFLV(value);
        }

        public FuzzySet(double val = 0)
        {
            FunctionFLV = x => val;
        }

        public FuzzySet(Func<double, double> flv)
        {
            FunctionFLV = flv;
        }

        public FuzzySet() : this(0)
        {
        }

        public FuzzySet(MultiDimFuzzySet s)
        {
            if (s.Dim!=1) throw new InvalidCastException();
            var l = new List<double>();
            for (int i = 1; i < s.Dim; i++) l.Add(0);
            FunctionFLV = x =>
            {
                var l2 = new List<double> {x};
                l2.AddRange(l);
                return s.FLV(l2);
            };
        }

        public FuzzySet(MultiDimFuzzySet s, IEnumerable<double> offset, int dim)
        {
            //var l = new List<double>();
            if (offset.Count() != s.Dim || dim >= s.Dim || dim < 0) throw new ArgumentException();
            var l = offset.Select(x => -x).ToArray();
            FunctionFLV = x =>
            {
                var l2 = new double[l.Length];
                l.CopyTo(l2, 0);
                l2[dim] += x;
                return s.FLV(l2);
            };
        }

        public FuzzySet(SingleDimFuzzySet fuzzySet)
        {
            FunctionFLV = fuzzySet.FLV;
        }

        /// <summary>
        /// Finds intersection of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of input sets</returns>
        public new static FuzzySet Intersection(FuzzySet s1, FuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return (FuzzySet) SingleDimFuzzySet.Intersection(s1, s2, norm);
        }

        /// <summary>
        /// Finds union of given fuzzy sets
        /// </summary>
        /// <param name="s1">Fuzzy set to compute</param>
        /// <param name="s2">Fuzzy set to compute</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of input sets</returns>
        public static FuzzySet Union(FuzzySet s1, FuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return (FuzzySet) SingleDimFuzzySet.Union(s1, s2, norm);
        }

        /// <summary>
        /// Finds union of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set union with</param>
        /// <param name="norm">S-Norm to be used in computing intersection of given sets</param>
        /// <returns>Union of this and input fuzzy set</returns>
        public FuzzySet UnionWith(FuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Union(this, other, norm);
        }

        /// <summary>
        /// Finds intersection of this instance of fuzzy set with the other.
        /// </summary>
        /// <param name="other">Fuzzy set intersect with</param>
        /// <param name="norm">T-Norm to be used in computing intersection of given sets</param>
        /// <returns>Intersection of this and input fuzzy set</returns>
        public FuzzySet IntersectWith(FuzzySet other, Norm norm = Norm.Zadeh)
        {
            return Intersection(this, other, norm);
        }
    }
}
