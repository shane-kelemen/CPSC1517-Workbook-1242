using System.Diagnostics.Metrics;

namespace CPSC1012Review
{
    internal class Program
    {      
        static void Main(string[] args)
        {
            // Pre-increment/decrement vs Post-increment/decrement
            // One of the first questions asked in response to my stating of best practice to use the pre- versions 
            // of increment and decrement was "why?".  This is a good question.  Here is the review of the answer...
            //
            // All operators are at the end of the day methods.  They must be resolved by the order of operations as
            // you are well aware.  For example, consider the addition operator...
            //
            // public static int operator +(int left, int right)
            // {
            //     // details for binary addition of two values
            // }
            //
            // Now, whatever the internal details are, the result will be a returned integer that effectively replaces
            // the operator and the two values on either side.  This is called "resolving" the operator.
            //
            // There is a very good reason that post-increment/decrement have a higher precedence.
            // The post versions MUST be resolved first in order for the increment or decrement to happen, but to
            // look like it happens after the execution of the line of code has completed. 
            //
            // Internally, the post- versions looks
            // public static int operator ++(ref int value)
            // {
            //      int temp = value;
            //      ++value;
            //      return temp;
            // }
            //
            // While the pre- versions look like this:
            //
            // public static int operator ++(int value)
            // {
            //      return value + 1;
            // }
            //
            // As can be seen, the pre version is far more efficient for execution time and memory resources required.
            // However, even if this was not the case, the simple fact that the post-version is created in such a way
            // as to behave as if the operation does not happen until after the line of code has completed, makes it
            // very likely to introduce an unintended bug into your mathematical code at some point in the future.
            //
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Conditional Display Values");
            Console.ForegroundColor = ConsoleColor.Gray;
            //
            // Consider the following:
            int original = 10;
            int result = ++original * 10;
            Console.WriteLine("Original after pre-increment : " + original);
            Console.WriteLine("Result after pre-increment : " + result);
            //
            // You probably expected 11 and 110 to be displayed, correct?
            // Good.  That is what we would want 99.999% of the time.
            // 
            // Now predict the following:
            original = 10;
            result = original++ * 10;
            Console.WriteLine("Original after post-increment : " + original);
            Console.WriteLine("Result after post-increment : " + result);
            //
            // This time the result of the calculation is different.  There may be times when this is the behaviour you 
            // wish to see, but those times will be few and far between.  Better to have the pre-increment/decrement habit.
            //
            // Rule of thumb:  You should be able to defend why you need a post- version.  If you cannot, change it.
            Console.WriteLine();


            // Decisions - Branching
            //           - Three main constructs: if-else if-else ladders
            //                                    switch
            //                                    conditional (ternary operator)
            //
            // In class it appeared that everyone was comfortable with the first two constructs,
            // and so we did not spend any time on the syntax for them.  However, the conditional
            // operator was not a comforatable thing for everyone, so the following was demonstrated.
            //
            // This following block of code is a simple if-else construct that we use as a base
            // to show what the conditional statement is doing.  In this case a simple test of a
            // variable value, and then writing to the screen the result.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Conditional Display Values");
            Console.ForegroundColor = ConsoleColor.Gray;
            //
            int value = 10;
            if (value > 5)
            {
                Console.WriteLine("Bigger than 5");
            }
            else
            {
                Console.WriteLine("5 or less");
            }
            //
            // Here is the conditional operator in action.  We are still using a single WriteLine()
            // call, but this time what is written to the screen will be the value that is "resolved"
            // from the conditional operator inside the parenthesis.
            //
            // Basic conditional operator syntax:  test_expression ? true result : false result
            //
            // So in the case below, if value is greater than 5, then the operator will "resolve" as
            // "Bigger than 5", which will then be printed to the screen by the WriteLine() call.
            Console.WriteLine(value > 5 ? "Bigger than 5" : "5 or less");
            //
            // As a different example, we may also use the ternary operator in more complex situations,
            // but we must be careful that what we are expecting is what we actually get.
            //
            // Example:  If value is greater than 5, we wish to multiple value by 25, otherwise we
            //           wish to multiply 25 by 100.
            //
            // Executing the following yields a value of 10, not 250 as we were expecting.  Upon examining 
            // the order of operations that would occur, we find out that instead of checking to see if
            // value is greater than 5, we are actually checking to see if 25 * value is greater than 5.
            // Thus only value (10) is written to the screen instead of 25 * value.
            int number = 25 * value > 5 ? value : 100;
            Console.WriteLine(number);
            //
            // To correct the issue, we must place parenthesis around the ternary operator so that it is
            // resolved first before the multiplier of 25 is applied.  The following will yield 250 as
            // expected.
            number = 25 * (value > 5 ? value : 100);
            Console.WriteLine(number);
            Console.WriteLine();


            // Decisions - Repetition
            //           - Four main constructs: while      Executes 0 - infinite number of times
            //                                   do-while   Executes 1 - infinite number of times
            //                                   for        Executes a calculated number of times
            //                                   foreach    Executes equal to items in a collection
            //                                                  number of times
            //
            // Once again, for the most part these constructs were comfortable for most of the class.
            // We did spend a bit of time talking about best practices, like the purpose of each
            // construct, and when each should be used.  
            //
            // We also identified while, for, foreach constructs as pre-test, and do-while as post-test
            //
            // With all repetition constructs, remember that the values in the ending condition must change
            // in some way to avoid "infinite repetition".  Essentially the repetition must be allowed to
            // end at some point or your program will lock up and be unusable.
            //
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Repetition Display Values");
            Console.ForegroundColor = ConsoleColor.Gray;
            //
            // while - This construct is best used as a sentinel.  For instance, to execute code only
            //         when a test value is above or below an expected limit.  In this case it is
            //         possible that the test value is already above or below that test limit when the
            //         construct is encountered, and so the enclosed code may not run at all.
            //
            // The following will display: 0 1 2 3 4, but if i is set to 5 or greater it will not
            // display anything at all.
            //
            // Experiment changing the value of i to better understand.
            Console.Write("while : ");
            int i = 0;
            while (i < value)
            {
                Console.Write(i + " ");
                ++i;
            }
            Console.WriteLine();
            //
            // do-while - Because this is a post-test construct, if we set up the code exactly as above,
            //            the display will be the same if i is set to 0, but even if i is set to 5 or above,
            //            at least the starting value of i will be displayed, no matter what original value
            //            is.
            // 
            // This construct is very useful for accepting and validating user input.  You do not know
            // how many times the user will enter invalid data before they get it right, but you know you
            // must ask for that input at least once.
            //
            // Experiment changing the value of i to better understand how it operates.
            Console.Write("do-while : ");
            i = 0;
            do
            {
                Console.Write(i + " ");
                ++i;
            }
            while (i < value);
            Console.WriteLine();
            //
            // for - The following shows that the basic operation of a for construct is very similar to 
            //       the while construct.  It is pre-test, needs some kind of initial value to test against,
            //       and a way to change the value being tested.  The for construct is designed to run a
            //       set number of times that may be literal, or may be calculated.
            //
            // Notice how the for loop is far more succinct.  Basically a nicer way to format a while 
            // construct when you know the endpoints of operation.
            //
            // Syntax:
            // for (initialization of control variables(s); ending condition; control variable adjustment)
            //
            // Order of Execution: Initialization happens once only when the for construct is encountered
            //                     The ending condition is evaluated before a construct iteration runs
            //                     The scoped code is run if the ending condition evaluates as true
            //                     The control variable(s) are adjusted before the next iteration
            //
            // This first example shows exactly the same as the while example above.  Again, you may 
            // experiment with the starting value of i to see that the operation is the same.
            Console.Write("for : ");
            for(i = 0; i < value; ++i)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            //
            // This more complex example shows more than one control variable, how they may both be used
            // in the ending condition (not mandatory), and how they may both be changed during the
            // adjustment phase.  Not that increment and decrement are not the only options during the
            // adustment phase.
            //
            // Experiment with the values of j and k, but also with the adjustment phase value changes
            // to see how things change.
            Console.Write("for (with more than one control variable) : ");
            for (int j = 0, k = 20; j < k; ++j, k -= 2)
            {
                Console.Write(k - j + " ");
            }
            Console.WriteLine();
            //
            // foreach - This construct is a special one that is meant to operate over a collection, such
            //           as an Array or a List.  It comes with a few special restrictions that the for
            //           construct does not have:
            //
            //           Must begin at the start of a collection, not the middle or the end
            //           Must go through the collection one at a time, you may only skip to the next item
            //              using the continue keyword
            //           Must go through the collection front to back, you may not ever go back even one item
            //           Must go through the entire collection, not stop in the middle naturally, only if the 
            //              break keyword is used to exit the entire construct
            //           Data in the collection may not be changed as it is readonly when accessed using foreach
            //
            // Syntax:
            // foreach("Data type of collection item" "Alias name of collection item" in "Collection name")
            //
            // As we require a collection for this construct to operate on, a simple array of integers is
            // created and initialized with values below using the for construct, and then displayed using
            // the foreach construct.  The array will be discussed more after this simple example.
            //
            // Declare an array of 10 integers and initialize each spot in the array to the square of its index value. 
            int[] ints = new int[10];
            for (int index = 0; index < 10; ++index)
            {
                ints[index] = index * index;
            }
            //
            // The following will iterate over the array above and display the values to the screen.
            Console.Write("foreach (over initial integer array) : ");
            foreach (int num in ints)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            Console.WriteLine();


            // Arrays - This is the simplest collection.  It is a group of items all of the same data type (in C#)
            //          Any data type may be used as the elements of the collection.
            //          The items are accessed using a numerical index that begins with zero, and ends with one less
            //              than the number of elements being stored.
            //          The size of an array is set at the time of creation and may not be changed.
            //          Attempting to index an array with a negative value or a value larger than one less than the 
            //              the number of elements being stored will generate and IndexOutOfRangeException.
            //
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Display of Array Operations");
            Console.ForegroundColor = ConsoleColor.Gray;
            //
            // Managing an array is very manual, including when you find that the size of the array is too small or too 
            // large, and you wish to change the size of the array.
            //
            // Task : Change the size of an array from 10 to 20.  Follow these steps:
            //          - A new array must be created with the new desired size.  All elements will be initialized to
            //              the false equialent (false, zero, null) of the data type (in C#)
            //          - The old array data elements must be copied to the new array, maintaining index location
            //          - The variable for the old array must be attached to the new array
            //          - Any new data elements may now be saved to the added spots in the new array
            //          - The old array will automatically be destroyed by the memory manager 
            int[] temp = new int[20];
            for (int index = 0; index < 10; ++index)
            {
                temp[index] = ints[index];
            }
            ints = temp;
            ints[10] = 100;
            //
            // Again using the foreach loop, display the elements of the new array.  Note the zeros at the end. 
            Console.Write("foreach (over new larger integer array) : ");
            foreach (int num in ints)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            //
            // Now that you have arrays at your disposal, there are several simple algorithms that may be explored.
            // In C#, the collections will have methods that perform these operations for you, but it is advisable
            // to become familiar with how these algorithms are constructed in case you end up programming in
            // langauges with less rich support.
            //
            // The following blocks will demonstrate sum, average, min, and max.  Note that more than one related
            // algorithm may be completed in a single repetition construct / block of code.  Foreach constructs are
            // excellent tools for these algorithms that require the entire collection to be examined and where no
            // changes to the underlying data are required. 
            //
            // Sum and Average
            int sum = 0;
            foreach (int num in ints)
            {
                sum += num;
            }
            Console.WriteLine("Sum : " + sum);
            // 
            // When computing averages, you just divide the sum by the number of elements in the array.  In some 
            // languages, like C, you must either calculate the number of elements from the data size of the array,
            // or manually track the number of elements.  In C#, this is a Property of the array.
            //
            // This is a good time for a reminder of the existence of integer division as well.  We would not expect the 
            // average of a bunch of numbers to display a whole number result, but in this case that is exactly what
            // we will see because the sum and the Length property if the array are both integers.  Running the following,
            // you will find the result to be 19.  Displaying the remainder using the modulus (%) operator shows it should
            // not have been an even number.
            double average = sum / ints.Length;
            int remainder = sum % ints.Length;
            Console.WriteLine("Average (incorrect) : " + average);
            Console.WriteLine("Remainder (proof) : " + remainder);
            //
            // In order to find the true average, we must cast one of the values to a variable type that may store
            // non-integral values.
            average = (double)sum / ints.Length;
            Console.WriteLine("Average (correct) : " + average);
            //
            // The following will find the min and max values stored in out ints collection.  Assume the first element
            // of the array is the min and max, and then process the array elements to see what happens.
            int min = ints[0];
            int max = ints[0];
            foreach(int num in ints)
            {
                if (num < min)
                {
                    min = num;
                }

                if (num > max)
                {
                    max = num;
                }
            }
            Console.WriteLine("Minimum value : " + min);
            Console.WriteLine("Maximum value : " + max);
            //
            // The above all work really well using the foreach construct for iteration of the collection, but if you
            // slightly adjust what you are looking for, you will require a for construct instead. 
            //
            // The following will tell you WHERE the min and max values are located, not possible with a foreach construct
            // without a whole bunch more variable manipulation.  Again, start assuming the first element is the min and
            // the max.  Also assumes that a tie does not change the index value, so the first lowest and first largest
            // values will be located. If you want to find the last values, change the < and > to <= and >=.
            int minIndex = 0;
            int maxIndex = 0;
            for(int index = 0; index < ints.Length; ++index)
            {
                if (ints[index] < ints[minIndex])
                {
                    minIndex = index;
                }

                if (ints[index] > ints[maxIndex])
                { 
                    maxIndex = index;
                }
            }
            Console.WriteLine("Minimum value index : " + minIndex);
            Console.WriteLine("Maximum value index : " + maxIndex);
            //
            // There are many other array algorithms that may be practiced.  To find some of them, search for 500 programming
            // problems in your browser and search for the array section
            Console.WriteLine();


            // Lists - This is another collection that is based on the array.  In fact, it is effectively a managed array,
            //         meaning you do not have to do some operation manually, such as growing or shrinking the array.  It is 
            //         all managed for you by the runtime environment.
            //
            // The list may be indexed just like an array, but you may only index the list within the used portion of its
            // overall capacity, which is unlike the array where if there are 20 spots in the array, you may assign a value
            // to any of them even if the spot is not in use yet.  If you want the same capability in a list, you need to
            // add some extra manual elements to mimic the trailing zeros.
            //
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Display of List Operations");
            Console.ForegroundColor = ConsoleColor.Gray;
            //
            // The list has two Properties that help demonstrate how the array is being managed for you by the runtime.
            // When the list is declared, it starts off with a Capacity of 0, and a used spot Count of 0.
            Random rand = new Random();
            List<int> myInts = new List<int>();
            Console.WriteLine("Beginning Count : " + myInts.Count);
            Console.WriteLine("Beginning Capacity : " + myInts.Capacity); 
            //
            // When you add the first item into the list, the runtime immediately gives you an internal array of 4 spots.
            // To set a value to one of the spots, you may use the Add() method to set the value of the next available
            // spot, or Insert() to set the value of any spot from 0 to the Count property value of the list.
            // Notice how the capacity and count values have changed.  Also, a foreach construct may be used for display.
            myInts.Add(0);
            Console.WriteLine("Count after one Add() call : " + myInts.Count);
            Console.WriteLine("Capacity after one Add() call : " + myInts.Capacity);
            Console.Write("foreach of List after one Add() call : ");
            foreach (int num in myInts)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            //
            // Let's fill the list to 8 completely filled spots and check the values again.
            for (int input = 1; input < 8; ++input)
            {
                myInts.Add(input * input);
            }
            Console.WriteLine("Count after eight Add() calls : " + myInts.Count);
            Console.WriteLine("Capacity after eight Add() calls : " + myInts.Capacity);
            Console.Write("foreach of List after eight Add() calls : ");
            foreach (int num in myInts)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            //
            // Manually adding three more values to the list brings us back to the same elements we had used in the array,
            // but notice the foreach stops at the count of the list, not the capacity which would include all the zeros
            myInts.Add(64);
            myInts.Add(81);
            myInts.Add(100);
            Console.WriteLine("Count after eleven Add() calls : " + myInts.Count);
            Console.WriteLine("Capacity after eleven Add() calls : " + myInts.Capacity);
            Console.Write("foreach of List after eleven Add() calls : ");
            foreach (int num in myInts)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            //
            // As with the array, the list has many built in values, but we still may write manual algorithms if we choose.
            // We have already written the manual algoithms, so here are some examples of the built-in methods.
            // Note that only the used portion of the list will be considered for the following.
            // This will be most obvious in the calculation of the average which will use 11 instead of 20 that was used
            //      with the array for element count.
            Console.WriteLine("Sum : " + myInts.Sum());
            Console.WriteLine("Average : " + myInts.Average());
            Console.WriteLine("Minimum value : " + myInts.Min());
            Console.WriteLine("Maximum value : " + myInts.Max());
            Console.WriteLine("Minimum value index : " + myInts.IndexOf(myInts.Min()));
            Console.WriteLine("Maximum value index : " + myInts.IndexOf(myInts.Max()));
            Console.WriteLine();
            // 
            // Note that the last two lines are less efficient than the manual process as you must go through the list
            // twice.


            // Methods - A block of code that is basically a "program within a program" in an application.
            //
            // Syntax:
            // [access specifier(s)] returnType MethodName (Comma Separated Parameter List) 
            // {
            //      Method algorithm / instructions
            // }
            //
            // [Access Specifiers]
            // public - The method is available to be used by users outside the class
            // private - The method may only be used inside the class
            //
            // static - The method is shared by all instances of the class (more on this when talking about classes)
            //
            // Example:
            // public double Volume (double radius)
            // {
            //      return 4 / 3 * Math.PI * Math.Pow(radius, 3);
            // }
            //
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Display of Method Operations");
            Console.ForegroundColor = ConsoleColor.Gray;
            //
            // The following block of code sets up a call to the PowValues methods, and then displays the resulting
            // changes to the intput List of integers.
            int exp = 4;
            PowValues(exp, myInts);

            Console.WriteLine("List values after calling PowValues method");
            foreach (int num in myInts)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Applies the input exponent to each of the values in the input List of integers
        /// </summary>
        /// <param name="exponent">Power to be applied to all List values</param>
        /// <param name="values">The List of values to be "powered"</param>
        static private void PowValues (int exponent, List<int> values)
        {
            for (int index = 0; index < values.Count; ++index)
            {
                // Note the "cast" in front of the call to the Math.Pow method.  This is required because
                // there is a possiblity of losing information from the return value from Math.Pow when we
                // try to save the value into the integer List because it is a double.  Using the cast tells
                // the compiler that we understand the risks and take responsibility for them.  A cast is putting
                // the new data type in brackets immediately to the left of the thing we are trying to change.
                values[index] = (int)Math.Pow(values[index], exponent);
            }          
        }
    }
}


