using System.Diagnostics.CodeAnalysis;

namespace ReviewOfFundamentalBasics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Pre-increment method definition
            //
            //    int operator++ (int value)
            //    {
            //       return value + 1;
            //    }

            // Post-increment method definition
            //
            //    int operator++ (ref int value)
            //    {
            //       int temp = value; 
            //       ++value;
            //       return temp;
            //    }

            {
                int original = 10;
                int result = ++original * (int)Math.Pow(original, 2);
                Console.WriteLine("Original after pre-increment : " + original);
                Console.WriteLine("Result after pre-increment : " + result);
                Console.WriteLine();

                original = 10;
                result = original++ * 10;
                Console.WriteLine("Original after post-increment : " + original);
                Console.WriteLine("Result after post-increment : " + result);
            }


            // Decision - Branching
            // if-else ladder
            // Use when: full expressions are needed
            //           when testing a range
            //           when the data to be tested is not integral (means exact)
            //           differing data types between parts of the ladder
            {
                int original = 1;
                if (original > 5)
                {
                    Console.WriteLine("Bigger than 5");
                }

                if (original < 5)
                {
                    if (original % 2 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Even number less than 5 : {0}", original);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Odd number less than 5 : {0}", original);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }   
                
                // Rewrite the above using a single if statement if the inner else was to be excluded.
                if (original < 5 && original % 2 == 0) 
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Even number less than 5 : {0}", original);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                if (original == 0)
                {
                    Console.WriteLine("Hello");
                }
                else if (original == 1)
                {
                    Console.WriteLine("World!");
                }
                //else if(original == 2)
                //{
                //    Console.WriteLine("Bleh!");
                //}
                else
                {
                    Console.WriteLine("Everything else");
                }


                if (original == 0 || original == 1)
                {

                }

                switch(original)
                {
                    case 0:
                        Console.WriteLine("Hello");
                        break;

                    case 1:
                        Console.WriteLine("World!");
                        break;

                    case 2:
                        Console.WriteLine("Bleh!");
                        break;
                }

                DayOfWeek day = DayOfWeek.Wednesday;
                switch(day)
                { 
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        Console.WriteLine("Weekday!");
                        break;

                    case DayOfWeek.Sunday:
                    case DayOfWeek.Saturday:
                        Console.WriteLine("Weekend!");
                        break;

                    default:
                        Console.WriteLine("What planet are you from?");
                        break;
                }


                Console.WriteLine(day == DayOfWeek.Tuesday ? "Cheap movies!" : "Regular price");

                bool upper = false;
                bool lower = false;
                bool special = true;
                bool digit = true;
                int strength = (upper ? 1 : 0) + (lower ? 1 : 0) + (special? 1 : 0) + (digit ? 1 : 0);
                if (strength < 4)
                {
                    Console.WriteLine("Not good enough!");
                }
            }


            // Repetition - Looping
            // Different loop constructs : For, do-while, while, foreach
            {
                // while
                // Best used when we cannot determine how many times the loop will run, and we are fine with 0 runs.
                //int count = 0;
                //while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                //{
                //    Console.Write(++count + ", ");
                //}
                //Console.WriteLine("\nAll Done!\n");

                //for(int index = 0; Console.ReadKey(true).Key != ConsoleKey.Enter; ++index)
                //{
                //    Console.Write(++index + ", ");
                //}
                //Console.WriteLine("\nAll Done!\n");

                //count = 0;
                //do
                //{
                //    Console.Write(++count + ", ");
                //}
                //while (Console.ReadKey(true).Key != ConsoleKey.Enter);
                //Console.WriteLine("\nAll Done!\n");

                //count = 0;
                //Console.Write("How many times should I run the loop? ");
                //int input = int.Parse(Console.ReadLine());
                //for(int i = 0; i < input; ++i)
                //{
                //    Console.Write(i + ", ");
                //}
                //Console.WriteLine("\nAll Done!\n");

                //string s = "This is a collection of characters!";
                //foreach(char c in s)
                //{
                //    Console.Write(c + ", ");
                //}
                //Console.WriteLine("\nAll Done!\n");


                List<double> doubles = null;  // = new List<double>();
                Random rng = new Random();
                double average = 1;
                
                for(int i = 0; i < 20; ++i)
                {
                    doubles.Add(rng.NextDouble() * rng.Next(1, 50));
                }

                foreach(double d in doubles)
                {
                    average += d;
                }
                average /= doubles.Count;
                Console.WriteLine($"Average : {average:G6}\n");
            }


















            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
