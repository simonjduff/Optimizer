using System;
using System.Threading;
using Problem;
using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    public class Forager
    {
        private readonly CancellationToken _cancellationToken;
        private readonly Func<Neighbourhood> _neighbourhoodFunc;
        private readonly FirstModel _model;

        public Forager(CancellationToken cancellationToken,
            Func<Neighbourhood> neighbourhoodFunc,
            FirstModel model)
        {
            _cancellationToken = cancellationToken;
            _neighbourhoodFunc = neighbourhoodFunc;
            _model = model;
        }

        public void Search()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var neighbourhood = _neighbourhoodFunc();
                    if (neighbourhood == null)
                    {
                        continue;
                    }

                    var idea = neighbourhood.Nearby();
                    neighbourhood.RegisterSearch(idea, _model.Fitness(idea));
                }
            });

            thread.Start();
        }
    }
}