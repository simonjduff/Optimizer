using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new FirstModel();
            var orchestrator = new Orchestrator(4, 20);
            orchestrator.Run();
        }
    }
}
