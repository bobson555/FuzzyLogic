using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzySets
{
    public class FuzzySet:SingleDimFuzzySet
    {
        Func<double, double> _flv;

        public override double FLV(double value)
        {
            return _flv(value);
        }

        public FuzzySet(double val = 0)
        {
            this._flv = x => val;
        }

        public FuzzySet(Func<double, double> flv)
        {
            this._flv = flv;
        }

        public FuzzySet()
        {
            this._flv = x => 0;
        }

        public FuzzySet(MultiDimFuzzySet s, IEnumerable<double> offset, int dim)
        {
            var l = new List<double>();
            var off = offset.ToArray();
            for (int i = 0; i < s.Dim; i++)
            {
                l.Add(-off[i]);
            }
            this._flv = x => { l[dim-1] += x; return s.FLV(l); };
        }


    }
}
