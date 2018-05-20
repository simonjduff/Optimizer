using System;
using System.Collections.Generic;
using System.Linq;

namespace Problem
{
    public class Segment
    {
        public string Id { get; }
        public IEnumerable<Func<Func<string, double>, (double T, int Q)>> Inputs { get; }
        private readonly double _area;
        private const double AmbientTemperature = 17.5;
        public const double Density = 0.277;
        public const double SpecificHeatCapacity = 2400;

        public Segment(string id,
            double area,
            params Func<Func<string, double>, (double T, int Q)>[] inputs)
        {
            Id = id;
            Inputs = inputs;
            _area = area;
        }

        public virtual SegmentOutput Output(Func<string, double> uFunc)
        {
            var inputs = Inputs.Select(i => i(uFunc)).ToArray();
            var q = inputs.Sum(i => i.Q);
            var inputTemperature = CalculateInputTemperature(inputs);

            if (q == 0)
            {
                return new SegmentOutput(Id, AmbientTemperature, q, inputs, inputTemperature);
            }

            var t = (inputTemperature - AmbientTemperature)
                    * Math.Exp((-1 * _area * uFunc(Id)) / (MassPerSecond(q) * SpecificHeatCapacity))
                    + AmbientTemperature;

            return new SegmentOutput(Id, t, q, inputs, inputTemperature);
        }

        protected double CalculateInputTemperature((double T, int Q)[] inputs)
        {
            return inputs.Sum(i => i.T * i.Q) / inputs.Sum(i => i.Q);
        }

        public double MassPerSecond(int q)
        {
            return q * Density;
        }
    }
}