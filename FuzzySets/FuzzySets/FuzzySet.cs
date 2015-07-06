using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
    public class FuzzySet:SingleDimFuzzySet
    {
        readonly Func<double, double> _flv;

        public override double FLV(double value)
        {
            return _flv(value);
        }

        public FuzzySet(double val = 0)
        {
            _flv = x => val;
        }

        public FuzzySet(Func<double, double> flv)
        {
            _flv = flv;
        }

        public FuzzySet() : this(0)
        {
        }

        public FuzzySet(MultiDimFuzzySet s)
        {
            if (s.Dim!=1) throw new InvalidCastException();
            var l = new List<double>();
            for(int i=1; i<s.Dim; i++) l.Add(0);
            _flv = x =>
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
            _flv = x =>
            {
                var l2 = new double[l.Length];
                l.CopyTo(l2, 0);
                l2[dim] += x;
                return s.FLV(l2);
            };
        }

        public FuzzySet(SingleDimFuzzySet fuzzySet)
        {
            _flv = fuzzySet.FLV;
        }


    }
}
