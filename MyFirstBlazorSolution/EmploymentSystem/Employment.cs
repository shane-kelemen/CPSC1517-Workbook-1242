//this refers to a namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces

#endregion

namespace EmploymentSystem
{
    public class Employment
    {
        //data members (fields, variables)
        //typically data members are private and hold data for use
        //  within your class
        //usually associated with a property
        //a data member does not have an built-in validation
        private string _Title;
        private double _Years;
        private SupervisoryLevel _Level;

        //properties
        //are associated with a single piece of data.
        //Properties can be implemented by:
        //  a) fully implemented property
        //  c) auto implmented property

        //A property does not need to store data
        //  this type of property is referred to as a read-only
        //  this property typically uses existing data values
        //      within the instance to return a computed value

        //fully implemented properties usually has additional logic
        //  to execute for control over the data: such as validation
        //fully implemented properties will have a declared data
        //  member to store the data into
        
        //auto implemented properties do not have additional logic
        //Auto implemented properties do not have a declared
        //  data member instead the o/s will create on the property's
        //  behave a storage that is accessable ONLY by the property

        ///<summary>
        ///Property: Title
        ///validation: there must be a character in the string
        ///a property will always have a getter (accessor)
        ///a property may or maynot have a setter (mutator)
        /// no mutator the property is consider "read-only" and is
        ///         usually returning a compound field
        /// has a mutator, the property will a some point save the data
        ///     to storage
        /// the mutator may be public (default) or private
        ///     public: accessable by outside users of the class
        ///     private: accessable ONLY within the class, usually
        ///                 via the constructor or a method
        /// !!!!! a property DOES NOT have ANY declared incoming parameters !!!!!!
        /// </summary>
        
        public string Title
        {
            //accessor (getter)
            //returns the string associated with this property
            get { return _Title; }

            //mutator (setter)
            //it is within the set that the validation of the data
            //  is done to determine if the data is acceptable
            //if all processing of the string is done via the property
            //  it will ensure that good data is within the associated string
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                  throw new ArgumentNullException("Title", "Title is a required field");
              
                //it is a very good practice to remove leading and trailing spaces on strings
                //  so that only the required and important characters are stored.
                //to do this santization use .Trim()
                _Title = value.Trim();

             
            }
        }

        ///<summary>
        ///Property: Years
        ///validation: the value must be 0 to greater
        ///</summary>
        public double Years
        {
            get
            {
                return _Years;
            }
            set
            {
                //if (value < 0)
                if (!Utilities.IsZeroOrPositive(value))
                    throw new ArgumentOutOfRangeException("Years", value,
                            "Years must be 0 to greater (ie 3.75)");
                _Years = value;
            }
        }

        ///<summary>
        ///Property: StarDdate
        ///validation: none
        ///access: private
        ///</summary>
        //since the access to this property for the mutator is private ANY validation
        //  for this data will need to be done elsewhere
        //possible locations for the validation could be in
        //  a) a constructor
        //  b) any method that will alter the data
        //a private mutator will NOT allow alteration of the data via a property for the
        //  outside user, however, methods within the class will still be able to
        //  use the property

        //this property can be coded as an auto-implemented property
        public DateTime StartDate { get; private set; }

        ///<summary>
        ///Property: Level
        ///validation: none
        ///note: this is an enum using SupervisoryLevel
        ///</summary>
        
        //can an auto-implemented be coded as a fully implemented
        public SupervisoryLevel Level
        {
            get { return _Level; }
            //set { _Level = value; } //default public access
            private set 
            {
                //once this validation is added to this psuedo "auto-implement" property 
                //  it would need to be considered a fully-implemented property
                if (!Enum.IsDefined(typeof(SupervisoryLevel), value))
                    throw new ArgumentException($"Invalid supervisory level value of {value} ", "Level");
                _Level = value; 
            }
        }

        //constructors

        //your class does not technically need a constructor
        //if you code a constructor for your class you are responsible for coding ALL constructors
        //if you do not code a constructor then the system will assign the software datatype defaults
        //  to your variables (data members/auto-implemented properties)

        //syntax: accesslevel constructorname([list of parameters]) { .... }
        //NOTE: NO return datatype
        //      the constructorname MUST be the class name

        //Default
        //simulates the "system defaults"
        public Employment()
        {
            //if there is no code within this constructor, the actions for setting
            //  your internal fields will be using the system defaults for the datatype

            //optionally
            // you could assign values to your intial fields within this constructor typically
            // using literal values
            //Why?
            // your internal fields may have validation attached to the data for the field
            // this validation is usually within the property
            // you would wish to have valid data values for your internal fields

            Title = "unknown";
            Level = SupervisoryLevel.TeamMember;
            StartDate = DateTime.Today;

            //Years ?
            //the default is fine (0.0)
            //however, if you wish you could actually assign the value 0 yourself
            Years = 0.0;
        }

        //Greedy
        //this is the constructor typically used to assign values to a instance at the time of
        //    creation
        //the list of parameters may or maynot contain default parameter values
        //if you have assigned default parameter values then those parameters MUST be at the end of
        //  the parameter list

        public Employment (string title, SupervisoryLevel level,
                            DateTime startdate, double years = 0.0)
        {
            Title = title;
            Level = level;
            Years = years;

            //one could add valiation, especially if the property has a private set  OR the property
            //  is an auto-implemented property that has restrictions
            //example
            //validation, start date must not exist in the future
            //validation can be done anywhere in your class
            //since the property is auto-implemented AND/OR has a private set,
            //      validation can be done  in the constructor OR a behaviour 
            //      that alters the property
            //IF the validation is done in the property, IT WOULD NOT be an
            //      auto-implemented property BUT a fully-implemented property
            // .Today has a time of 00:00:00 AM
            // .Now has a specific time of day 13:05:45 PM
            //by using the .Today.AddDays(1) you cover all times on a specific date

            if (startdate >= DateTime.Today.AddDays(1))
            {
                throw new ArgumentException($"The start date {startdate} is in the future.","Startdate");
            }
            StartDate =startdate;

            //the unit test discovered that the years is not being correctly calculate
            //  if the default value for the parameter is used
            //the constructor should calculate the years from the supplied startdate
            //  to the current date
            if (years > 0.0)
            {
                Years = years;
            }
            else
            {
                TimeSpan span = DateTime.Today - StartDate;
                Years = Math.Round((span.Days / 365.2), 1);
            }

        }

        //methods (aka behaviours)

        //syntax: access returndatatype methodname ([list of parameters]) { ..... }

        //REMEMBER: YOU HAVE ACCESS TO ALL VALUES WITHIN THE INSTANCE SO YOU DO NOT
        //          HAVE TO PASS IN VALUES THAT ARE ALREADY CONTAINED IN THE INSTANCE.

        public override string ToString()
        {
            //this string is known as a "comma separate value" string (csv)
            //concern: when the date is used, it could have a , within the data value
            //solution: IF this is a possibillity that a value that is used in createing the string pattern
            //              could make the pattern invalid, you should explicitly handle how the value should be
            //              displayed in the string
            return $"{Title},{Level},{StartDate.ToString("MMM dd yyyy")},{Years}";
        }

        //if SypervisoryLevel setter is private this means altering the Level must be done in constructor
        //(which executes ONLY ONCE during creation) or via a method
        public void SetEmploymentResponsibilityLevel(SupervisoryLevel level)
        {
            Level = level;
        }

        //StartDate is private set
        //Note: when you have a private set, you MAY NEED to duplicate validation in several places
        //(constructor AND this method)
        public void CorrectStartDate(DateTime startdate)
        {
            if (startdate >= DateTime.Today.AddDays(1))
            {
                throw new ArgumentException($"The start date {startdate} is in the future.", "Startdate");
            }
            StartDate =startdate;

            //concern
            //if the startdate has been altered from the creation of the
            //  instance WHERE the years has been set, then should the 
            //  years be recalculated?
            TimeSpan days = DateTime.Today - startdate;
            Years = Math.Round((days.Days / 365.2), 1);
        }
        //create a method that would accept a string and convert the data within the string
        //  into an instance of Employment. Return the instance.
        //this would be the parsing of the string
        public static Employment Parse(string item)
        {
            string[] dataValues = item.Split(',');  // Separates the information in the string by the comma
            if (dataValues.Length != 4)
            {
                throw new FormatException($"Invalid record format: {item}");
            }
            return new Employment(dataValues[0],
                                  (SupervisoryLevel)Enum.Parse(typeof(SupervisoryLevel), dataValues[1]),
                                  DateTime.Parse(dataValues[2]),
                                  double.Parse(dataValues[3]));
        }
    }
}
