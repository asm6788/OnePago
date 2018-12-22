using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OnePago.OneCard;

namespace OnePago
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = new List<Player>();
            Console.WriteLine("플레이어수");
            Console.Title = "원파고 콘솔";
            int howmany = Convert.ToInt32(Console.ReadLine());
            for (int i =0; i != howmany; i++)
            {
                if (howmany == 2)
                {
                    players.Add(new AI() { Horizon = true, ID = 0 });
                    players.Add(new Human() { Horizon = true, ID = 1 });
                }
                else
                {
                    if (i == 2)
                    {
                        players.Add(new Human() { Horizon = true , ID = 2});
                    }
                    else
                    {
                        players.Add(new AI() { Horizon = (i == 0 || i == 2 ? true : false), ID = i });
                    }
                }
            }

            GUI GUI_system = new GUI();

            OneCard Game = new OneCard(players,GUI_system);
            Thread thread = new Thread(() => Game.Start(), 125000*1000);
            thread.Start();
            GUI_system.LOOP();
            thread.Join();
            Console.Read();
        }
    }
}
