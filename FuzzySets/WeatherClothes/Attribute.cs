using FuzzySets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherClothes
{
    public class Attribute
    {
        List<String> labels;
        List<FuzzySet> attributes;

        public Attribute(IEnumerable<String> labels, IEnumerable<FuzzySet> attributes)
        {
            if(labels.Count() != attributes.Count()) throw new ArgumentException();
            this.labels = labels.ToList();
            this.attributes = attributes.ToList();
        }

        public double GetAttributeValue(String label, double value)
        {
            return GetFuzzySet(label)[value];
        }

        public double[] this[double value]
        {
            get{
                return GetValues(value);
            }
        }

        public double[] GetValues(double value)
        {
            var l = new List<double>();
            foreach (String s in labels)
            {
                l.Add(GetAttributeValue(s, value));
            }
            return l.ToArray();
        }

        public double GetAttributeValue(int index, double value)
        {
            return GetFuzzySet(index)[value];
        }

        public FuzzySet GetFuzzySet(int index)
        {
            return attributes[index];
        }

        public FuzzySet GetFuzzySet(String label)
        {
            var s = labels.IndexOf(label);
            if (s == -1) throw new ArgumentException("Label not found");
            return GetFuzzySet(s);
        }

        public String GetMaxLabel(double val)
        {
            String reslabel=null;
            var res = double.MinValue;
            for (int i = 0; i < attributes.Count; i++)
            {
                if (attributes[i][val] > res)
                {
                    reslabel = labels[i];
                    res = attributes[i][val];
                }
            }
            if (reslabel==null) throw new Exception("World ends");
            return reslabel;
        }
    }
}
