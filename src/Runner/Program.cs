using System;
using Problem;
using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new FirstModel();
            var orchestrator = new Orchestrator(5, 20);
            orchestrator.Run();
        }
    }
}
