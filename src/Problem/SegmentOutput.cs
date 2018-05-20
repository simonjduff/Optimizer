using System.Collections.Generic;

namespace Problem
{
    public class SegmentOutput
    {
        public SegmentOutput(string id,
            double t,
            int q,
            IEnumerable<(double T, int Q)> inputs,
            double inputTemperature)
        {
            Id = id;
            T = t;
            Q = q;
            Inputs = inputs;
            InputTemperature = inputTemperature;
        }
        public string Id { get; }
        public double T { get; }
        public int Q { get; }
        public IEnumerable<(double T, int Q)> Inputs { get; }
        public (double T, int Q) TQ => (T, Q);
        public double InputTemperature { get; }
    }
}