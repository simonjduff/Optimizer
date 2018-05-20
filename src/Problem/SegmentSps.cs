using System;

namespace Problem
{
    public class SegmentSps : Segment
    {
        public SegmentSps(string id, params Func<Func<string, double>, (double T, int Q)>[] inputs)
            : base(id, 1000.0, inputs)
        {

        }
    }
}