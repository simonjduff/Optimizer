using System;
using System.Linq;

namespace Problem
{
    public class SegmentBypass : Segment
    {
        public SegmentBypass(string id,
            params Func<Func<string, double>, (double T, int Q)>[] inputs) : base(id, -1, inputs)
        {
        }

        public override SegmentOutput Output(Func<string, double> uFunc)
        {
            var inputs = Inputs.Select(i => i(uFunc)).ToArray();
            var q = inputs.Sum(i => i.Q);
            var inputTemperature = CalculateInputTemperature(inputs);

            return new SegmentOutput(Id, inputTemperature, q, inputs, inputTemperature);
        }
    }
}