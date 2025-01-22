namespace EmploymentSystem
{
    public class Employment
    {
        // Data members / Fields
        private SupervisoryLevel _level;
        private string _title;
        private double _years;


        // Manual Properties
        public SupervisoryLevel Level
        {
            get { return _level; }

            set { _level = value; }
        }

        public string Title
        {
            get { return _title; }

            set { _title = value; }
        }

        public double Years
        {
            get { return _years; }

            set { _years = value; }
        }


        // Automatic Properties
        public DateTime StartDate { get; set; }


        // Constructors
        public Employment(string title, SupervisoryLevel level, 
                                        DateTime startDate, double years = 0.0)
        {
            Title = title;
            Level = level;
            StartDate = startDate;
            Years = years;
        }

        public Employment() : this("Hello World", SupervisoryLevel.Owner, 
                                                DateTime.Today, 10.23578)
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
            return $"{Title},{Level},{StartDate},{Years}";
        }
    }
}
