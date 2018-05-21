using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new FirstModel();
            var orchestrator = new Orchestrator(8, 40);
            orchestrator.Run();
        }
    }
}
