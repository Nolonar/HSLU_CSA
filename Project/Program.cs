using Explorer700Wrapper;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.WaitForDebugger();

            new PongGame(new Explorer700()).Run();
        }
    }
}
