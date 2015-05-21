using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MedScheduleRemastered
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Plan p = new Plan(5);
            Stopwatch s = new Stopwatch();
            s.Restart();
            Plan p2 = p.SuperPlan(10000,10);
            s.Stop();
            p2.Printplan();
            Console.WriteLine(s.ElapsedMilliseconds);
            Console.ReadKey();

        }
    }
}
