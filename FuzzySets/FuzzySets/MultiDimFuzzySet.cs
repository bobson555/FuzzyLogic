using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzySets
{
    public delegate IEnumerable<double> flv(IEnumerable<double> In);
    public abstract class MultiDimFuzzySet
    {
        public static MultiDimFuzzySet operator ~(MultiDimFuzzySet s)
        {
            return Complement(s);
        }

        public SingleFunctionFuzzySet ToSingleFunctionFuzzySet()
        {
            return new SingleFunctionFuzzySet(this);
        }

        public double this[IEnumerable<double> value]
        {
            get
            {
                return FLV(value);
            }
        }

        public abstract double FLV(IEnumerable<double> value);

        public virtual int Dim
        {
            get;
            protected set;
        }

        public static SingleFunctionFuzzySet CartesianProduct(MultiDimFuzzySet s1, MultiDimFuzzySet s2, Norm norm)
        {
            Func<IEnumerable<double>, double> flv = null;
            //TODO: switch
            flv = x => Math.Min(s1.FLV(x.Take(s1.Dim)), s2.FLV(x.Skip(s1.Dim)));
            var dim = s1.Dim + s2.Dim;
            return new SingleFunctionFuzzySet(flv, dim);
        }

        public static MultiDimFuzzySet Complement(MultiDimFuzzySet s)
        {
            //flv FLV;
            //FLV += s.FLV;
            //FLV += ((x)=>(var L = new List<double>() ; L.Add(1- x[0]); return L));
            return (MultiDimFuzzySet)(new SingleFunctionFuzzySet(x => 1 - s.FLV(x), s.Dim));
        }

        internal FuzzySet TryReduceDim()
        {
            if (Dim != 1) throw new InvalidCastException();
            var l = new List<double>();
            for(int i=0; i<this.Dim; i++) l.Add(0);
            return new FuzzySet(this, l, 1);
        }
    }
}
