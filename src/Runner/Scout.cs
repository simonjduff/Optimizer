using System;
using System.Threading;
using Problem;
using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    public class Scout
    {
        private readonly FirstModel _model;
        private readonly int _id;
        private readonly Action<double, Idea, int> _dance;
        private readonly Action<Idea, double> _report;
        private readonly CancellationToken _cancellationToken;
        private Neighbourhood _currentNeighbourhood;

        public Scout(FirstModel model, int id,
            Action<double, Idea, int> dance,
            Action<Idea, double> report,
            CancellationToken cancellationToken)
        {
            _model = model;
            _id = id;
            _dance = dance;
            _report = report;
            _cancellationToken = cancellationToken;
            var newIdea = Idea.NewIdea();
            _currentNeighbourhood = new Neighbourhood(newIdea, _model.Fitness(newIdea));
        }
        public void Search()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    var idea = _currentNeighbourhood.Nearby();
                    var fitness = _model.Fitness(idea);

                    if (AbandonSite(idea, fitness))
                    {
                        Console.WriteLine($"Abandoning site {_currentNeighbourhood.Output().Fitness}");
                        continue;
                    }

                    _dance(fitness,idea, _id);

                    if (_cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }); 

            thread.Start();
        }

        private bool AbandonSite(Idea idea, double fitness)
        {
            if (_currentNeighbourhood.RegisterSearch(idea, fitness))
            {
                Console.WriteLine($"Abandoning");
                var output = _currentNeighbourhood.Output();
                _report(output.Idea, output.Fitness);
                var newIdea = Idea.NewIdea();
                _currentNeighbourhood = new Neighbourhood(newIdea, _model.Fitness(newIdea));
                return true;
            }

            return false;
        }
    }
}