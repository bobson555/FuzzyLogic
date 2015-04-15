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

        public SingleDimFuzzySet()
        { }

        public FuzzySet ToFuzzySet()
        {
            return new FuzzySet(x => this.FLV(x));
        }

        public abstract double FLV(double value);

        public override double FLV(IEnumerable<double> value)
        {
            if (value.Count<double>() == 1) return FLV(value.First());
            else throw new ArgumentException();
        }

        public double this[double value]
        {
            get
            {
                return FLV(value);
            }
        }

        public static FuzzySet Intersection(SingleDimFuzzySet s1, SingleDimFuzzySet s2, Norm norm)
        {
            Func<double, double> flv = null;
            switch (norm)
            {
                case Norm.Zadeh:
                    flv = x => Math.Min(s1.FLV(x), s2.FLV(x));
                    break;
                case Norm.Lukasiewicz:
                    throw new NotImplementedException();
                    break;
                case Norm.Eintein:
                    throw new NotImplementedException();
                    break;
                case Norm.Bounded:
                    flv = x => Math.Max(s1.FLV(x) + s2.FLV(x) - 1, 0);
                    break;
            }
            return new FuzzySet(flv);
        }

        public static FuzzySet Union(SingleDimFuzzySet s1, SingleDimFuzzySet s2, Norm norm)
        {
            Func<double, double> flv = null;
            switch (norm)
            {
                case Norm.Zadeh:
                    flv = x => Math.Max(s1.FLV(x), s2.FLV(x));
                    break;
                case Norm.Lukasiewicz:
                    throw new NotImplementedException();
                    break;
                case Norm.Eintein:
                    throw new NotImplementedException();
                    break;
                case Norm.Bounded:
                    flv = x => Math.Min(s1.FLV(x) + s2.FLV(x), 1);
                    break;
            }
            return new FuzzySet(flv);
        }

        public FuzzySet UnionWith(SingleDimFuzzySet other, Norm norm)
        {
            return FuzzySet.Union(this, other, norm);
        }

        public FuzzySet IntersectWith(SingleDimFuzzySet other, Norm norm)
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
