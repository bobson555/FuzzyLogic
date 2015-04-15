using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzySets
{
    public class SingleFunctionFuzzySet : MultiDimFuzzySet
    {

        public SingleFunctionFuzzySet(Func<double, double> flv)
            : base()
        {
            this._flv = x =>
            {
                if (x.Count<double>() != 1) throw new ArgumentException();
                return flv(x.First()); //Czy jest potrzebne
            };
            this.Dim = 1;
        }

        public SingleFunctionFuzzySet(Func<IEnumerable<double>, double> flv, int dim)
            : base()
        {
            this._flv = flv;
            this.Dim = dim;
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
            if (value.Count<double>() != this.Dim) throw new ArgumentException();
            return _flv(value);
        }
    }
}
