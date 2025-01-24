using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem
{
    public class WriteUp
    {
        public string Violation {  get; set; }
        public string SupervisorName { get; set; }
        public string Resolution { get; set; }


        public WriteUp(string violation, string supervisorName, string resolution)
        {
            Violation = violation;
            SupervisorName = supervisorName;
            Resolution = resolution;
        }
    }
}
