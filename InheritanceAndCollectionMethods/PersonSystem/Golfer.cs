using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonSystem
{
    public class Golfer : Person
    {
        static Random rng = new Random();

        private double _handicap;

        public double Handicap
        {
            get {  return _handicap; }

            set
            {
                if (value < -10)
                    throw new ArgumentOutOfRangeException("Please be realistic!");

                _handicap = value;
            }
        }

        public Golfer(Color eyeColour, Color hairColour, string firstName, string lastName) :
                                                        base(eyeColour, hairColour, firstName, lastName)
        {
            _firstName = firstName;
            Age = rng.Next(20, 71);
        }

        public override string ToString()
        {
            return base.ToString() + $" - Handicap: {Handicap}";
        }

        public static bool PinkEyesOnly(Golfer golfer)
        {
            return golfer.EyeColour.Equals(Color.Pink);
        }
    }
}
