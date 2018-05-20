using System;
using System.Collections.Generic;
using System.Linq;
using Problem.Extensions;

namespace Problem
{
    public class FirstModelCase : IModelCase
    {
        public IDictionary<string, Segment> Segments { get; } = new Dictionary<string, Segment>(7);

        public static Func<string, double> UFetcher(IList<int> genes)
        {
            return id =>
            {

                switch (id)
                {
                    case "a":
                        return genes[0];
                    case "b":
                        return genes[1];
                    case "c":
                        return genes[2];
                    case "d":
                        return genes[3];
                    case "e":
                        return genes[4];
                    case "f":
                        return genes[5];
                    case "g":
                        return 5;
                    default:
                        throw new Exception($"Unknown gene id {id}");
                }
            };
        }

        public FirstModelCase(string[] bypasses, params (double T, int Q)[] inputs)
        {
            Segment SegmentFunc(string id, double area, Func<Func<string, double>, (double T, int Q)>[] i) => new Segment(id, area, i);
            Segment SegmentSpsFunc(string id, double area, Func<Func<string, double>, (double T, int Q)>[] i) => new SegmentSps(id, i);

            BuildSegment(bypasses,
                "a", 0,
                SegmentSpsFunc,
                uFunc => (inputs[0].T, inputs[0].Q));

            BuildSegment(bypasses,
                "b", 0,
                SegmentSpsFunc,
                uFunc => (inputs[1].T, inputs[1].Q));

            BuildSegment(bypasses,
                "c", 0,
                SegmentSpsFunc,
                uFunc => Segments["a"].Output(uFunc).TQ,
                uFunc => Segments["b"].Output(uFunc).TQ);

            BuildSegment(bypasses,
                "d", Area(7400),
                SegmentFunc,
                uFunc => Segments["c"].Output(uFunc).TQ);

            BuildSegment(bypasses,
                "e", 0,
                SegmentSpsFunc,
                uFunc => (inputs[2].T, inputs[2].Q));

            BuildSegment(bypasses,
                "f", Area(10000),
                SegmentFunc,
                uFunc => Segments["d"].Output(uFunc).TQ,
                uFunc => Segments["e"].Output(uFunc).TQ);

            BuildSegment(bypasses,
                "g", Area(1600),
                SegmentFunc,
                uFunc => Segments["f"].Output(uFunc).TQ);
        }

        private void BuildSegment(string[] bypasses,
            string id,
            double area,
            Func<string, double, Func<Func<string, double>, (double T, int Q)>[], Segment> constructor,
            params Func<Func<string, double>, (double T, int Q)>[] inputs)
        {
            Segments.Add(id, !bypasses.ContainsSafe(id) ? constructor(id, area, inputs) : new SegmentBypass(id, inputs));
        }

        public IDictionary<string, SegmentOutput> Run(int[] genes)
        {
            var finalGenes = genes.Concat(new[] { 6 }).ToArray();
            return Segments.Values.Select(s => s.Output(UFetcher(finalGenes))).ToDictionary(i => i.Id);
        }

        private double Area(int length)
        {
            return length * Math.PI * 0.359;
        }
    }
}