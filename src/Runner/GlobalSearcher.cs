using System;
using Problem;
using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    public class GlobalSearcher
    {
        private readonly FirstModel _model;
        private readonly string _id;

        public GlobalSearcher(FirstModel model, string id)
        {
            _model = model;
            _id = id;
        }
        public (Idea Idea, double fitness) Search()
        {
            var idea = Idea.NewIdea();

            double delta = 0d;
            double bestResult = 0;
            Idea? bestIdea = null;
            do
            {
                for (int i = 0; i < idea.Size; i++)
                {
                    var up = idea.Increment(i);
                    var down = idea.Decrement(i);
                    var upFitness = _model.Fitness(up);
                    var downFitness = _model.Fitness(down);


                    var oldBestResult = bestResult;
                    if (upFitness > bestResult)
                    {
                        bestIdea = up;
                        bestResult = upFitness;
                    }

                    if (downFitness > bestResult)
                    {
                        bestIdea = down;
                        bestResult = downFitness;
                    }

                    delta = bestResult - oldBestResult;
                }
            } while (delta > 0d);

            return (bestIdea.Value, bestResult);
        }
    }
}