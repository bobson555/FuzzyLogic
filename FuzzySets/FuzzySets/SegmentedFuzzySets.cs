using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySets
{
    public class LinearSegment : IComparable<LinearSegment>
    {
        public const double ComparisonTolerance = 10 * Double.Epsilon;
        public double Start { get; private set; }
        public double End { get; private set; }
        public double StartValue { get; private set; }
        public double EndValue { get; private set; }

        public Func<double, double> FunctionFlv { get; private set; }

        public LinearSegment(double start, double startValue, double end, double endValue, bool leftClosed = true, bool rightClosed = true)
        {
            if (end < start) throw new ArgumentException();
            Start = start;
            End = end;
            StartValue = startValue;
            EndValue = endValue;
            RightClosed = rightClosed;
            LeftClosed = leftClosed;
            double a = Math.Abs(start - end) < ComparisonTolerance ? 0 : (startValue - endValue) / (start - end);
            if (double.IsNaN(a)) a = 0;
            double b = startValue - a * (double.IsInfinity(start)||double.IsNegativeInfinity(start)?0:start);
            FunctionFlv = x => a * x + b;
        }
        public double Flv(double x)
        {
            if (FunctionFlv == null || x < Start || x > End) return 0;
            if (IsIncreasing && (FunctionFlv(x) < StartValue || FunctionFlv(x) > EndValue)) throw new ArgumentOutOfRangeException();
            if (IsDecreasing && (FunctionFlv(x) > StartValue || FunctionFlv(x) < EndValue)) throw new ArgumentOutOfRangeException();
            if (IsSingleValued && (Math.Abs(FunctionFlv(x) - StartValue) > ComparisonTolerance || Math.Abs(StartValue - EndValue) > ComparisonTolerance)) throw new ArgumentOutOfRangeException();
            return FunctionFlv(x);
            //return R;
        }
        public double this[double value]
        {
            get
            {
                return Flv(value);
            }
        }

        public bool IsIncreasing { get { return StartValue < EndValue; } }
        public bool IsDecreasing { get { return StartValue > EndValue; } }
        public bool IsNonIncreasing { get { return !IsIncreasing; } }
        public bool IsNonDecreasing { get { return !IsDecreasing; } }
        public bool IsSingleValued { get { return Math.Abs(Start - End) < ComparisonTolerance; } }

        public int CompareTo(LinearSegment other)
        {
            var starts = Start.CompareTo(other.Start);
            if (starts != 0) return starts;
            return End.CompareTo(other.End);
        }
        public bool IsInside(double value)
        {
            if (Math.Abs(value - Start) < ComparisonTolerance) return LeftClosed;
            if (Math.Abs(value - End) < ComparisonTolerance) return RightClosed;
            return value > Start && value < End;
        }

        public bool LeftClosed { private set; get; }
        public bool RightClosed { private set; get; }
    }

    public class LinearSegmentFuzzySet : SingleDimFuzzySet
    {
        readonly List<LinearSegment> _segments;

        public LinearSegmentFuzzySet(IEnumerable<LinearSegment> nonZeroSegments)
        {
            var s = nonZeroSegments.ToList();
            s.Sort();
            for (int i = 1; i < s.Count; i++)
            {
                if (s[i - 1].End > s[i].Start) throw new ArgumentException();
            }
            _segments = s;
        }

        public override double Flv(double value)
        {
            var seg = _segments.Find(x => x.IsInside(value));
            return seg == null ? 0 : seg[value];
        }
    }

    public class TriangleFuzzySet : LinearSegmentFuzzySet
    {
        public double Left { get; protected set; }
        public double Right { get; protected set; }
        public double Center { get; protected set; }

        public TriangleFuzzySet() : base(null) { }

        public TriangleFuzzySet(double left, double center, double right, double centerValue)
            : base(new[] { new LinearSegment(left, 0, center, centerValue, true, false), new LinearSegment(center, centerValue, right, 0) })
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

        public LeftShoulderFuzzySet(double left, double center, double right, double leftShoulderValue)
            : base(new[] { new LinearSegment(left, leftShoulderValue, center, leftShoulderValue, true, false), new LinearSegment(center, leftShoulderValue, right, 0) })
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

        public RightShoulderFuzzySet(double left, double center, double right, double rightShoulderValue)
            : base(new[] { new LinearSegment(left, 0, center, rightShoulderValue, true, false), new LinearSegment(center, rightShoulderValue, right, rightShoulderValue) })
        {
            Left = left;
            Center = center;
            Right = right;
        }
    }
}
