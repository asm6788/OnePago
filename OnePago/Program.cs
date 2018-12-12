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
            List<OneCard.Player> players = new List<OneCard.Player>();
            Console.WriteLine("AI수");
            int howmany = Convert.ToInt32(Console.ReadLine());
            for (int i =0; i != howmany; i++)
            {
                players.Add(new OneCard.AI());
            }
            players.Add(new Human());
            OneCard Game = new OneCard(players);
            Thread thread = new Thread(() => Game.Start(), 125000*1000);
            thread.Start();
            thread.Join();
            Console.Read();
        }
    }
}
