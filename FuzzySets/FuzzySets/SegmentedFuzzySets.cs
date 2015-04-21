using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzySets
{
    public class LinearSegment : IComparable<LinearSegment>
    {
        public double Start { get; private set; }
        public double End { get; private set; }
        public double StartValue { get; private set; }
        public double EndValue { get; private set; }

        public Func<double, double> Flv { get; private set; }

        public LinearSegment(double start, double startValue, double end, double endValue, bool leftClosed = true, bool rightClosed = true)
        {
            if (end < start) throw new ArgumentException();
            this.Start = start;
            this.End = end;
            this.StartValue = startValue;
            this.EndValue = endValue;
            this.RightClosed = rightClosed;
            this.LeftClosed = leftClosed;
            double a = start == end ? 0 : (startValue - endValue) / (start - end);
            double b = startValue - a * start;
            Flv = x => a * x + b;
        }
        public double FLV(double x)
        {
            if (Flv == null || x < Start || x > End) return 0;
            if (isIncreasing && (Flv(x) < StartValue || Flv(x) > EndValue)) throw new ArgumentOutOfRangeException();
            if (isDecreasing && (Flv(x) > StartValue || Flv(x) < EndValue)) throw new ArgumentOutOfRangeException();
            if (isSingleValued && (Flv(x) != StartValue || StartValue != EndValue)) throw new ArgumentOutOfRangeException();
            return Flv(x);
        }
        public double this[double value]
        {
            get
            {
                return this.FLV(value);
            }
        }

        public bool isIncreasing { get { return StartValue < EndValue; } }
        public bool isDecreasing { get { return StartValue > EndValue; } }
        public bool isNonIncreasing { get { return !isIncreasing; } }
        public bool isNonDecreasing { get { return !isDecreasing; } }
        public bool isSingleValued { get { return Start == End; } }

        public int CompareTo(LinearSegment other)
        {
            var starts = this.Start.CompareTo(other.Start);
            if (starts != 0) return starts;
            return this.End.CompareTo(other.End);
        }
        public bool isInside(double value)
        {
            if (value == Start) return LeftClosed;
            if (value == End) return RightClosed;
            return value > Start && value < End;
        }

        public bool LeftClosed { private set; get; }
        public bool RightClosed { private set; get; }
    }

    public class LinearSegmentFuzzySet : SingleDimFuzzySet
    {
        List<LinearSegment> segments;

        public LinearSegmentFuzzySet(IEnumerable<LinearSegment> nonZeroSegments)
            : base()
        {
            var s = nonZeroSegments.ToList<LinearSegment>();
            s.Sort();
            for (int i = 1; i < s.Count; i++)
            {
                if (s[i - 1].End > s[i].Start) throw new ArgumentException();
            }
            segments = s;
        }

        public override double FLV(double value)
        {
            var seg = segments.Find(x => x.isInside(value));
            if (seg == null) return 0;
            return seg[value];
        }

        public static LinearSegmentFuzzySet Intersection(LinearSegmentFuzzySet s1, LinearSegmentFuzzySet s2, Norm norm)
        {
            Func<double, double> flv = null;
            //TODO: Improve Segmentation
            switch (norm)
            {
                case Norm.Zadeh:
                    flv = x => Math.Min(s1.FLV(x), s2.FLV(x));
                    break;
                case Norm.Lukasiewicz:
                    flv = x => Math.Max(0, s1.FLV(x) + s2.FLV(x) - 1);
                    break;
                case Norm.Einstein:
                    flv = x => (s1.FLV(x) * s2.FLV(x)) / (2 - (s1.FLV(x) + s2.FLV(x) - s1.FLV(x) * s2.FLV(x)));
                    break;
                case Norm.Algebraic:
                    flv = x => (s1.FLV(x) *s2.FLV(x));
                    break;
            }
            var points = new List<double>();
            //Todo: znaleźć punkty przecięć, stworzyć nową funkcję przynależności

            return null;
        }

        public static LinearSegmentFuzzySet Union(LinearSegmentFuzzySet s1, LinearSegmentFuzzySet s2, Norm norm)
        {
            Func<double, double> flv = null;
            //TODO: Improve segmentation
            switch (norm)
            {
                case Norm.Zadeh:
                    flv = x => Math.Max(s1.FLV(x), s2.FLV(x));
                    break;
                case Norm.Lukasiewicz:
                    flv = x => Math.Min(s1.FLV(x) + s2.FLV(x), 1);
                    break;
                case Norm.Einstein:
                    flv = x => (s1.FLV(x) + s2.FLV(x)) / (1 + s1.FLV(x) * s2.FLV(x));
                    break;
                case Norm.Algebraic:
                    flv = x => 1 - (1 - s1.FLV(x))*(1 - s2.FLV(x));
                    break;
            }

            //Todo: znaleźć punkty przecięć, stworzyć nową funkcję przynależności
            return null;
        }

        public LinearSegmentFuzzySet UnionWith(LinearSegmentFuzzySet other, Norm norm)
        {
            return LinearSegmentFuzzySet.Union(this, other, norm);
        }

        public LinearSegmentFuzzySet IntersectWith(LinearSegmentFuzzySet other, Norm norm)
        {
            return LinearSegmentFuzzySet.Intersection(this, other, norm);
        }
    }

    public class TriangleFuzzySet : LinearSegmentFuzzySet
    {
        public double Left { get; protected set; }
        public double Right { get; protected set; }
        public double Center { get; protected set; }

        public TriangleFuzzySet() : base(null) { }

        public TriangleFuzzySet(double left, double center, double right, double value)
            : base(new LinearSegment[] { new LinearSegment(left, 0, center, value, true, false), new LinearSegment(center, value, right, 0) })
        {
            Left = left;
            Center = center;
            Right = right;
        }
    }

    public class LeftShoulderFuzzySet : LinearSegmentFuzzySet
    {
        public double Left { get; protected set; }
        public double Right { get; protected set; }
        public double Center { get; protected set; }

        public LeftShoulderFuzzySet() : base(null) { }

        public LeftShoulderFuzzySet(double left, double center, double right, double value)
            : base(new LinearSegment[] { new LinearSegment(left, value, center, value, true, false), new LinearSegment(center, value, right, 0) })
        {
            Left = left;
            Center = center;
            Right = right;
        }
    }

    public class RightShoulderFuzzySet : LinearSegmentFuzzySet
    {
        public double Left { get; protected set; }
        public double Right { get; protected set; }
        public double Center { get; protected set; }

        public RightShoulderFuzzySet() : base(null) { }

        public RightShoulderFuzzySet(double left, double center, double right, double value)
            : base(new LinearSegment[] { new LinearSegment(left, 0, center, value, true, false), new LinearSegment(center, value, right, value) })
        {
            Left = left;
            Center = center;
            Right = right;
        }
    }
}
