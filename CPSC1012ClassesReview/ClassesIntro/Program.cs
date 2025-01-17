namespace ClassesIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        enum DogBreed { Retriever, Husky, Bulldog, GreatDane }
        enum PowersOfTwo { Zero = 0, One = 2, Two = 4, Three = 8, Four = 16 }

        class Golfer
        {
            private string _firstName;
            private int _handicap;

            public string FirstName
            {
                get { return _firstName.ToUpper(); }

                private set { _firstName = value; }
            }

            public string LastName { get; set; }

            public int Handicap
            {
                get { return _handicap; }
                set { _handicap = value; }
            }

            public Golfer(string first, string last, int handicap)
            {
                FirstName = first;
                LastName = last;
                Handicap = handicap;
            }
            
        }
    }
}
