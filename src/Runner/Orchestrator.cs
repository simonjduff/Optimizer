using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Problem;
using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    public class Orchestrator
    {
        private readonly Scout[] _scouts;
        private readonly Forager[] _foragers;
        private readonly List<Neighbourhood> _danceFloor = new List<Neighbourhood>();
        private static readonly object Synlock = new Object();
        private static readonly Dictionary<Idea, double> Ideas = new Dictionary<Idea, double>(); 
        private static readonly Dictionary<double, List<Idea>> Fitnesses = new Dictionary<double, List<Idea>>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static readonly Random Random = new Random();
        private readonly ManualResetEvent[] _syncs;

        public Orchestrator(int scouts, int localSearchers)
        {
            _syncs = new ManualResetEvent[scouts];

            void DanceFunc(double fitness, Idea idea, int scoutId)
            {
                var neighbourhood = new Neighbourhood(idea, fitness);

                lock (Synlock)
                {
                    _danceFloor.Add(neighbourhood);
                    _syncs[scoutId].Set();
                }

                Thread.Sleep((int) fitness);
                lock (Synlock)
                {
                    _danceFloor.Remove(neighbourhood);
                    _syncs[scoutId].Reset();
                }
            }

            void ReportFunc(Idea idea, double fitness)
            {
                Console.WriteLine($"Reporting {idea} {fitness}");
                lock (Synlock)
                {
                    if (Ideas.ContainsKey(idea))
                    {
                        return;
                    }

                    Ideas.Add(idea, fitness);
                    if (!Fitnesses.ContainsKey(fitness))
                    {
                        Fitnesses.Add(fitness, new List<Idea>());
                    }

                    Fitnesses[fitness].Add(idea);
                }
            }

            Neighbourhood ForagerFunc()
            {
                WaitHandle.WaitAny(_syncs);
                lock (Synlock)
                {
                    if (!_danceFloor.Any())
                    {
                        return null;
                    }

                    var dancer = _danceFloor[Random.Next(0, _danceFloor.Count - 1)];
                    return dancer;
                }
            }

            var model = new FirstModel();
            _scouts = new Scout[scouts];
            for (int i = 0; i < scouts; i++)
            {
                _syncs[i] = new ManualResetEvent(false);
                _scouts[i] = new Scout(model, 
                    i,
                    DanceFunc,
                    ReportFunc,
                    _cancellationTokenSource.Token);

                _scouts[i].Search();
            }

            _foragers = new Forager[localSearchers];
            for (int i = 0; i < localSearchers; i++)
            {
                _foragers[i] = new Forager(_cancellationTokenSource.Token, ForagerFunc, model);
                _foragers[i].Search();
            }
        }

        public void Run()
        {
            int counter = 0;
            while (counter <= 10)
            {
                Thread.Sleep(1000);
                lock (Synlock)
                {
                    Console.WriteLine($"Fitnesses {Fitnesses.Count}");
                    if (Fitnesses.Count == 0)
                    {
                        continue;
                    }

                    var maxFitness = Fitnesses.Max(f => f.Key);
                    Console.WriteLine($"{maxFitness:0.0} {Fitnesses[maxFitness].First()} ");
                    counter++;
                }
            }

            _cancellationTokenSource.Cancel();
        }
    }
}