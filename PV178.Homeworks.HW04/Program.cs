using System;

namespace PV178.Homeworks.HW04
{
    public class Program
    {
        static void Main(string[] args)
        {
            Queries q = new Queries();
            q.AreAllLongSharksGenderIgnoringQuery();
            /*
            foreach (var item in res)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Value);
            }
            Console.ReadLine();
            */
        }
    }
}
