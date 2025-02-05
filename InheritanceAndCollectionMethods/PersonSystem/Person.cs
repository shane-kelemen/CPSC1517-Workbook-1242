using System.Drawing;

namespace PersonSystem
{
    public class Person : IComparable
    {
        // readonly data fields can only be changed one time, and that 
        // is during the construction of the instance
        protected readonly Color _eyeColour;
        private readonly Color _hairColour;

        // Usually list things not usually changed right after readonly items
        protected string _firstName;
        private string _lastName;

        // Then list items that are expected to change often
        private double _height;
        private double _weight;
        private int _age;


        public Color EyeColour
        {
            get { return _eyeColour; }
        }

        public Color HairColour
        { 
            get { return _hairColour; } 
        }


        public string FirstName
        {
            get { return _firstName; }
            set 
            { 
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("A Person must have a first name!");

                _firstName = value.Trim(); 
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("A Person must have a last name!");

                _lastName = value.Trim();
            }
        }

        public double Height
        {
            get { return _height; } 

            set 
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("A Person must have a height of more than zero centimetres!");

                _height = value; 
            }
        }

        public double Weight
        {
            get { return _weight; }

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("A Person must weigh more than zero kilograms!");

                _weight = value;
            }
        }

        public int Age
        {
            get { return _age; }

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("A Person must be more than 0 years old!");

                _age = value;
            }
        }


        public Person(Color eyeColour, Color hairColour, string firstName, string lastName, 
                                                            double height = 25, double weight = 3.5, int age = 1)
        {
            _eyeColour = eyeColour;
            _hairColour = hairColour;

            FirstName = firstName;
            LastName = lastName;
            Height = height;
            Weight = weight;
            Age = age;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Person person)) return false;

            //Person person = (Person)obj;
            //Person person = obj as Person;

            return FirstName.Equals(person.FirstName);
        }

        public override int GetHashCode()
        {
            return 1;
        }


        public int CompareTo(object obj)
        {
            if (!(obj is Person person))
                throw new ArgumentException("You have not passed in a Person object!");

            return LastName.CompareTo(person.LastName);
        }


        public override string ToString()
        {
            return $"{FirstName} {LastName} - {HairColour.ToKnownColor()} Hair, {EyeColour.ToKnownColor()} Eyes - " +
                                                        $"Height: {Height} cm, Weight: {Weight} kg, Age: {Age} years old";
        }
    }
}
