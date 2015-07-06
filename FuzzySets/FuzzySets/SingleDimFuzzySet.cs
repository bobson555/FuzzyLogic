using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzySets
{
    public abstract class SingleDimFuzzySet : MultiDimFuzzySet
    {
        public static SingleDimFuzzySet operator ~(SingleDimFuzzySet s)
        {
            return Complement(s);
        }

        protected SingleDimFuzzySet()
        { }

        public FuzzySet ToFuzzySet()
        {
            return new FuzzySet(FLV);
        }

        public abstract double FLV(double value);

        public override double FLV(IEnumerable<double> value)
        {
            if (value.Count<double>() == 1) return FLV(value.First());
            throw new ArgumentException();
        }

        public double this[double value]
        {
            get
            {
                return FLV(value);
            }
        }

        public static FuzzySet Intersection(SingleDimFuzzySet s1, SingleDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return new FuzzySet(x => Norms.ApplyTNorm(s1.FLV(x), s2.FLV(x), norm));
        }

        public static FuzzySet Union(SingleDimFuzzySet s1, SingleDimFuzzySet s2, Norm norm = Norm.Zadeh)
        {
            return new FuzzySet(x=> Norms.ApplySNorm(s1.FLV(x), s2.FLV(x), norm));
        }

        public FuzzySet UnionWith(SingleDimFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return FuzzySet.Union(this, other, norm);
        }

        public FuzzySet IntersectWith(SingleDimFuzzySet other, Norm norm = Norm.Zadeh)
        {
            return FuzzySet.Intersection(this, other, norm);
        }

        public static FuzzySet Complement(SingleDimFuzzySet s)
        {
            return new FuzzySet(x => 1-s.FLV(x));
        }

        public override sealed int Dim
        {
            get { return 1; }
            protected set { throw new InvalidOperationException(); }
        }
    }
}
