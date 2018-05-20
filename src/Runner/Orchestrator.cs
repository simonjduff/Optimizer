using System;
using System.Linq;
using System.Threading.Tasks;
using Problem;
using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    public class Orchestrator
    {
        private readonly GlobalSearcher[] _globals;

        public Orchestrator(int globalSearchers, int localSearchers)
        {
            var model = new FirstModel();
            _globals = new GlobalSearcher[globalSearchers];
            for (int i = 0; i < globalSearchers; i++)
            {
                _globals[i] = new GlobalSearcher(model, i.ToString());
            }
        }

        public void Run()
        {
            int searches = 0;
            double delta = 0d;
            double bestFitness = 0d;
            Idea? bestIdea = null;
            do
            {
                var tasks = _globals.Select(g => Task.Run(() => g.Search())).ToArray();
                Task.WaitAll(tasks);
                var foundIdeas = tasks.Select(t => t.Result).OrderByDescending(r => r.fitness);
                var best = foundIdeas.First();
                if (best.fitness > bestFitness)
                {
                    delta = best.fitness - bestFitness;
                    bestFitness = best.fitness;
                    bestIdea = best.Idea;
                    Console.WriteLine($"{searches++} Best {bestFitness:0} {bestIdea}");
                }

            } while (searches <= 10 && delta > 0d);
        }
    }
}