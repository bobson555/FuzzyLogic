using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
    public class SingleFunctionFuzzySet : MultiDimFuzzySet
    {

        public SingleFunctionFuzzySet(Func<double, double> flv, int dim = 1)
        {
            _flv = x =>
            {
                if (x.Count() != 1) throw new ArgumentException();
                return flv(x.First());
            };
            Dim = 1;
        }

        public SingleFunctionFuzzySet(Func<IEnumerable<double>, double> flv, int dim)
        {
            _flv = flv;
            Dim = dim;
        }

        public SingleFunctionFuzzySet(SingleDimFuzzySet s)
            : this(s.FLV)
        { }

        public SingleFunctionFuzzySet(MultiDimFuzzySet s)
            : this(s.FLV, s.Dim)
        { }

        protected Func<IEnumerable<double>, double> _flv
        { get; private set; }

        public override double FLV(IEnumerable<double> value)
        {
            if (value.Count() != Dim) throw new ArgumentException();
            return _flv(value);
        }
    }
}
