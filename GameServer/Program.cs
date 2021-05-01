using System;
using System.Threading;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Football Game Server";
            FootballGameServer n = new FootballGameServer();
            n.Start();

            Console.ReadKey();
        }
    }
}
