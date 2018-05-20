using System.Collections.Generic;

namespace Problem
{
    public interface IModel
    {
        IModelCase[] Cases { get; }
        string Output(int[] genes);
        double Failure(IDictionary<string, SegmentOutput> segments);
    }
}