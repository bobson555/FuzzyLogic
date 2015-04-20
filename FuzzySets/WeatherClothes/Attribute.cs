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
            var s = labels.IndexOf(label);
            if (s == -1) throw new ArgumentException("Label not found");
            return attributes[s][value];
        }

        public double[] this[double value]
        {
            get{
                var l = new List<double>();
                foreach(String s in labels)
                {
                    l.Add(GetAttributeValue(s, value));
                }
                return l.ToArray();
            }
        }

    }
}
