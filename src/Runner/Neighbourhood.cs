using System;
using Problem;

namespace Runner
{
    public class Neighbourhood
    {
        private Idea _idea;
        private double _fitness;

        public (Idea Idea, double Fitness) Output()
        {
            lock (_synlock)
            {
                return (_idea, _fitness);
            }
        }

        private int _attempts = 0;
        private const int MaxAttempts = 20;
        private readonly object _synlock = new object();

        public Neighbourhood(Idea idea, double fitness)
        {
            _idea = idea;
            _fitness = fitness;
        }

        public Idea Nearby()
        {
            return _idea.Nearby();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idea"></param>
        /// <param name="fitness"></param>
        /// <returns>True if the site should be abandoned</returns>
        public bool RegisterSearch(Idea idea, double fitness)
        {
            lock (_synlock)
            {
                if (fitness <= _fitness)
                {
                    _attempts++;

                    return _attempts > MaxAttempts;
                }

                //Console.WriteLine($"{FitnessDelta(fitness):0.0}");

                _idea = idea;
                _fitness = fitness;
                _attempts = 0;
                return false;
            }
        }

        public double FitnessDelta(double test)
        {
            lock (_synlock)
            {
                return test - _fitness;
            }
        }
    }
}