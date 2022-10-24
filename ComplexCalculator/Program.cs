using System;

namespace ComplexCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string text;
                text = Console.ReadLine();
                Equation e = new Equation(text);
                double result;
                if (e.Solve(out result))
                {
                    Console.WriteLine("{0}", result);
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }
    }
}
