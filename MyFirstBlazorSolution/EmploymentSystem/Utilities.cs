using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//for examining namespaces, this example has change the default namespace to something else
namespace EmploymentSystem
{
    public static class Utilities
    {
        //static classes are NOT instantiated by the outside user (developer/code)
        //static class items are referenced using:  classname.xxxx
        //methods within this class have the keyword static in their signature
        //static classes are shared between all outside users at the same time
        //DO NOT consider saving data within a static class BECAUSE you cannot be
        //  certain it will be there when you use the class again
        //consider placing GENERIC re-usable methods with a static class

        //sample of a generic method: numeric is a zero or positive value
        public static bool IsZeroOrPositive(double value)
        {
            //a structure method (apply to loops, etc) will have a single entry point
            //  and a single exit point
            //in this course you WILL AVOID where possible multiple returns from a method
            //in this course you WILL AVOID using a break to exit a loop structure
            //  or if structure
            bool valid = true;
            if(value < 0.0)
            {
                valid = false;
            }
            return valid;

            //this is an example of unstructured code
            //multiple exits from the if structure AND the method
            //this would voilate the class standard of structured code
            //if (valid < 0.0)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
        }

        //overload the IsZeroOrPositive so that it uses integers or decimals
        public static bool IsZeroOrPositive(int value)
        {
            bool valid = true;
            if (value < 0)
            {
                valid = false;
            }
            return valid;
        }
        public static bool IsZeroOrPositive(decimal value)
        {
            bool valid = true;
            if (value < 0.0m)
            {
                valid = false;
            }
            return valid;
        }
    }
}
