using System.Collections.Generic;

namespace Problem
{
    public interface IModelCase
    {
        IDictionary<string, Segment> Segments { get; }
        IDictionary<string, SegmentOutput> Run(int[] genes);
    }
}