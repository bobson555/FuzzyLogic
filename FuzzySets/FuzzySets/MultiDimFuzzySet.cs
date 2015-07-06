using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
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

        public static SingleFunctionFuzzySet CartesianProduct(MultiDimFuzzySet s1, MultiDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            var dim = s1.Dim + s2.Dim;
            Func<IEnumerable<double>, double> flv = x =>
            {
                if (x.Count()!= dim) throw new ArgumentException();
                return Norms.ApplyTNorm(s1.FLV(x.Take(s1.Dim)), s2.FLV(x.Skip(s1.Dim)));
            };
            return new SingleFunctionFuzzySet(flv, dim);
        }

        public static MultiDimFuzzySet Complement(MultiDimFuzzySet s)
        {
            return (MultiDimFuzzySet)(new SingleFunctionFuzzySet(x => 1 - s.FLV(x), s.Dim));
        }
    }
}
