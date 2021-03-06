﻿using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySets;

namespace WeatherClothes
{
    public class Attribute
    {
        readonly List<String> _labels;
        readonly List<FuzzySet> _attributes;
        private const double ComparisonTolerance = 10*Double.Epsilon;
        public Attribute(IEnumerable<String> labels, IEnumerable<FuzzySet> attributes)
        {
            var enumerable = labels as string[] ?? labels.ToArray();
            var fuzzySets = attributes as FuzzySet[] ?? attributes.ToArray();
            if(enumerable.Count() != fuzzySets.Count()) throw new ArgumentException();
            _labels = enumerable.ToList();
            _attributes = fuzzySets.ToList();
        }

        /// <summary>
        /// Calculates membership function value of given value.
        /// </summary>
        /// <param name="label">Label indentifying and attribute</param>
        /// <param name="value">Input value</param>
        /// <returns>Value of the membership function</returns>
        public double GetAttributeValue(String label, double value)
        {
            return GetFuzzySet(label)[value];
        }

        /// <summary>
        /// Gets all attributes' membership functions values based on the input.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Array of values of membership functions</returns>
        public double[] this[double value]
        {
            get{
                return GetValues(value);
            }
        }

        /// <summary>
        /// Gets all attributes' membership functions values based on the input.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Array of values of membership functions</returns>
        public double[] GetValues(double value)
        {
            return _labels.Select(s => GetAttributeValue(s, value)).ToArray();
        }

        /// <summary>
        /// Calculates membership function value of given value.
        /// </summary>
        /// <param name="index">Index of attribute</param>
        /// <param name="value">Input value</param>
        /// <returns>Value of the membership function</returns>
        public double GetAttributeValue(int index, double value)
        {
            return GetFuzzySet(index)[value];
        }

        /// <summary>
        /// Gets the fuzzy set at the given index
        /// </summary>
        /// <param name="index">Input index</param>
        /// <returns>Fuzzy set at given index</returns>
        public FuzzySet GetFuzzySet(int index)
        {
            return _attributes[index];
        }

        /// <summary>
        ///  Gets the fuzzy set which is labeled as input value.
        /// </summary>
        /// <param name="label">Input value</param>
        /// <returns>Fuzzy set labeled as input value</returns>
        public FuzzySet GetFuzzySet(String label)
        {
            var s = _labels.IndexOf(label);
            if (s == -1) throw new ArgumentException("Label not found");
            return GetFuzzySet(s);
        }

        /// <summary>
        /// Gets label of a fuzzy set in which input value has the biggest membership compared to other
        /// </summary>
        /// <param name="val">Input value</param>
        /// <param name="weights">Array of weights assigned to fuzzy sets in attribute</param>
        /// <param name="norm">Norm used to cut a fuzzy set to assigned weight</param>
        /// <returns>Label of a fuzzy set in which membership funtction reaches highest value</returns>
        public String GetMaxLabel(double val, double[] weights, Norm norm = Norm.Zadeh)
        {
            if (weights.Count() != _labels.Count()) throw new ArgumentException();
            String reslabel=null;
            var res = double.NegativeInfinity;
            int resid = -1;
            for (int i = 0; i < _attributes.Count; i++)
            {
                var fset = _attributes[i].IntersectWith(new FuzzySet(weights[i]), norm);
                var fval = fset[val];
                if (fval > res)
                {
                    reslabel = _labels[i];
                    res = fval;
                    resid = i;
                }
                else if (Math.Abs(fval - res) < ComparisonTolerance && _attributes[i][val] > _attributes[resid][val])
                {
                    reslabel = _labels[i];
                    res = fval;
                    resid = i;
                }
                else if (Math.Abs(fset[val] - res) < ComparisonTolerance && weights[i] > weights[resid])
                {
                    reslabel = _labels[i];
                    res = fset[val];
                    resid = i;
                }
                
            }
            if (reslabel==null) throw new Exception("World ends");
            return reslabel;
        }

        /// <summary>
        /// Gets label assigned to a fuzzy set of the given index
        /// </summary>
        /// <param name="i">Input index</param>
        /// <returns>Label at the given index</returns>
        public string GetLabel(int i)
        {
            return _labels[i];
        }
    }
}
