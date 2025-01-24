using System.Net.Http.Headers;

namespace EmploymentSystem
{
    public class Employment
    {
        // Data members / Fields
        private SupervisoryLevel _level;    // enumeration indicating the employee's position in the company
        private string _title;              // The employee's job title
        private double _years;              // The number of years the employee has been with the company


        // Manual Properties
        /// <summary>
        /// Allow the user to retrieve and modify the employee's rank within the company
        /// </summary>
        public SupervisoryLevel Level
        {
            
            get { return _level; }

            set 
            { 
                
                if(!Enum.IsDefined(typeof(SupervisoryLevel), value))
                {
                    throw new ArgumentException("The provided value is undefined for the SupervisoryLevel enumeration!");
                }

                _level = value; 
            }
        }

        public string Title
        {
            get { return _title; }

            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The Title is required!");
                }

                _title = value.Trim(); 
            }
        }

        public double Years
        {
            get { return _years; }

            set 
            {
                //if (!(value >= 0 && value <= 40))
                //{
                //    throw new ArgumentOutOfRangeException("The Year value supplied was out of range!");
                //}

                if(value < 0 || value > 40)
                {
                    throw new ArgumentOutOfRangeException("The Year value supplied was out of range!");
                }

                _years = value; 
            }
        }


        // Automatic Properties
        public DateTime StartDate { get; set; }

        public List<WriteUp> WriteUps { get; set; }


        // Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="level"></param>
        /// <param name="startDate"></param>
        /// <param name="writeUps"></param>
        /// <param name="years"></param>
        public Employment(string title, SupervisoryLevel level, 
                                        DateTime startDate, List<WriteUp> writeUps, double years = 0.0)
        {
            Title = title;
            Level = level;
            StartDate = startDate;
            Years = years;
            WriteUps = writeUps;
        }

        /// <summary>
        /// 
        /// </summary>
        public Employment() : this("Hello World!", SupervisoryLevel.TeamMember, 
                                                DateTime.Today, null, 10)
        { 
        }


        // Other Methods
        public void CorrectStartDate(DateTime startDate) 
        {
            StartDate = startDate;
        }

        public void SetEmploymentResponsibilityLevel(SupervisoryLevel level) 
        {
            Level = level;
        }

        public override string ToString()
        {
            return $"{Title},{Level},{StartDate.ToShortDateString()},{Years}";
        }

        public static bool DivisibleBySix(int value)
        {
            return value % 2 == 0 && value % 3 == 0;
        }
    }
}
