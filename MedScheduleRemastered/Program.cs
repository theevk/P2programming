using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    class Program
    {
        static void Main(string[] args)
        {
            Plan p = new Plan(5);
            Plan p2 = p.SuperPlan(100000,10);
            p2.Printplan();
            Console.ReadKey();

        }
    }
}
