using System;

namespace Football
{
    class Program
    {
        static void Main(string[] args)
        {
            FootballApp app = new FootballApp();
            bool ret = app.ExtractInfoFromFile();

            Console.ReadKey();
        }
    }
}
