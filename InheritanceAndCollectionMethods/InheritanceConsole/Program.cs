using PersonSystem;
using System.Drawing;

namespace InheritanceConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            object obj = new object();
            
            Person person = new Person(Color.Maroon, Color.Brown, "Harriette", "Spector");
            Console.WriteLine(person);
           

            Golfer golfer = new Golfer(Color.Brown, Color.Brown, "Mike", "Ross");
            Console.WriteLine(golfer);
            Console.WriteLine();


            Random rng = new Random();
            List<int> numbers = new List<int>();
            for(int i = 0; i < 100; ++i)
            {
                numbers.Add(rng.Next(1,26));
            }
            foreach (int i in numbers)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine();
            Console.WriteLine();

            List<int> results = numbers.Where(LessThan20).ToList();
            foreach(int i in results)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine();
            Console.WriteLine();

            List<int> resultsByLambda = numbers.Where(number => number < 20).ToList();
            foreach (int i in resultsByLambda)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine();
            Console.WriteLine();

            List<Golfer> golfers = new List<Golfer>();
            golfers.Add(golfer);
            golfers.Add(new Golfer(Color.Pink, Color.Violet, "Donna", "Paulson"));
            golfers.Add(new Golfer(Color.Brown, Color.Brown, "Harvey", "Spector"));
            golfers.Add(new Golfer(Color.Blue, Color.Violet, "Rachael", "Zane"));
            golfers.Add(new Golfer(Color.Pink, Color.LightYellow, "Louis", "Litt"));
            golfers.Add(new Golfer(Color.Green, Color.Green, "Robert", "Zane"));
            golfers.Add(new Golfer(Color.Red, Color.Brown, "Jessica", "Pearson"));

            foreach(Golfer g in golfers.Where(BrownHairOnly))
            {
                Console.WriteLine(g.ToString());
            }
            Console.WriteLine();

            foreach(Golfer g in golfers.Where(Golfer.PinkEyesOnly))
            {
                Console.WriteLine(g.ToString());
            }
            Console.WriteLine();

            foreach (Golfer g in golfers.Where(golfer => golfer.LastName.Equals("Zane")))
            {
                Console.WriteLine(g.ToString());
            }
            Console.WriteLine();

            List<int> uniqueInts = new List<int>();
            int discardCount = 0;
            while (uniqueInts.Count < 50)
            {
                int temp = rng.Next(0, 75);
                if(!uniqueInts.Contains(temp))
                {
                    uniqueInts.Add(temp);
                }
                else
                {
                    Console.WriteLine(temp + " discarded!");
                    ++discardCount;
                }
            }
            Console.WriteLine();

            Console.WriteLine($"There were {discardCount} discarded integers while generating the following list:");
            foreach (int i in uniqueInts)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine("\n");
            

            Golfer left = new Golfer(Color.Red, Color.Brown, "Jessica", "Pearson");
            Golfer right = new Golfer(Color.Red, Color.Brown, "Jessica", "Pearson");
            if (left.Equals(right))
            {
                Console.WriteLine("The two golfers are equal!");
            }
            else
            {
                Console.WriteLine("The two golfers are NOT equal!");
            }

            if (golfers.Contains(right))
            {
                Console.WriteLine("\"Right\" is in the list of golfers");
            }
            else
            {
                Console.WriteLine("\"Right\" is NOT in the list of golfers");
            }
            Console.WriteLine();

            uniqueInts.Clear();
            uniqueInts.Add(10);
            uniqueInts.Add(2);
            uniqueInts.Add(87);
            uniqueInts.Add(34);
            uniqueInts.Add(6);
       
            //for (int i = 0; i < uniqueInts.Count; ++i)
            //{
            //    // 10 2 87 34 6
            //    for(int j = 0; j < uniqueInts.Count; ++j)
            //    {
            //        if (uniqueInts[i] <= uniqueInts[j])
            //        {
            //            int temp = uniqueInts[j];
            //            uniqueInts[j] = uniqueInts[i];
            //            uniqueInts[i] = temp;
            //        }
            //    }

            //    foreach (int num in uniqueInts)
            //    {
            //        Console.Write(num + ", ");
            //    }  
            //    Console.WriteLine();
            //}


            uniqueInts.Sort();
            foreach (int i in uniqueInts)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine("\n");


            golfers.Sort();
            foreach (Golfer g in golfers)
            {
                Console.WriteLine(g.ToString());
            }
            Console.WriteLine();

            golfers.Sort(DescendingLastName);
            foreach (Golfer g in golfers)
            {
                Console.WriteLine(g.ToString());
            }
            Console.WriteLine();

            golfers.Sort((left, right) => left.Age.CompareTo(right.Age));
            foreach (Golfer g in golfers)
            {
                Console.WriteLine(g.ToString());
            }
            Console.WriteLine();

            Console.WriteLine("Does any golfer have brown hair? "
                                    + golfers.Any(golfer => golfer.HairColour.Equals(Color.Brown)));

            Console.WriteLine("The default Max golfer is: " + golfers.Max());
         


            Console.Write("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        static int DescendingLastName (Golfer left, Golfer right)
        {
            if (left == null && right == null) 
            { 
                return 0; 
            }

            if (left == null)
            {
                return 1;
            }

            if (right == null)
            {
                return -1;
            }

            //return left.CompareTo(right) * -1;
            return right.CompareTo(left);
        }

        static bool BrownHairOnly (Golfer golfer)
        {
            return golfer.HairColour.Equals(Color.Brown);
        }

        static bool LessThan20 (int value)
        {
            return value < 20;
        }
    }
}
