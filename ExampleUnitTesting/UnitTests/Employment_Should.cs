using Xunit;
using FluentAssertions;
using EmploymentSystem;
using System.Reflection.Emit;
using System;
using System.ComponentModel;

namespace TDDExampleTests
{
    public class Employment_Should
    {
        // The test for properties are probably the best ones to build and verify first so that the 
        // properties may be used when building the class constructors and other methods.
        #region Property Tests
        // A Fact is used to construct a single test, generally the successful operation.
        // A theory could be used, but they are generally reserved for running multiple tests with
        // different sets of data, most often used for testing failure conditions.
        [Fact]
        public void Allow_Change_To_Title()
        {
            // Arrange - Setting up the testing scenario
            // In this case we need a class object, using the default constructor is sufficient as 
            // we are just looking to assign new values to the contained data members through the
            // properties.
            Employment actual = new Employment();
            string expectedTitle = "Hello World!";  // Tradition says use this for your first string test!

            // Act - The action that we are trying to test
            // To show that the test will first fail with incorrect values, use the commented version
            // before using the version that should cause the test to pass.
            //actual.Title = "Hello World";  // Missing the exclamation mark.
            actual.Title = "  Hello World!  ";  // Comment out when testing the fail path.

            // Assert - The verification that the action is working correctly (or not)
            actual.Title.Should().Be(expectedTitle);  // If the actual and expected values match, this 
                                                      // test will pass.
        }

        //// Theory is used to test multiple combinations of input data using a sinlge method set up to
        //// run the test.
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Throw_Exception_For_Changing_Title(string title)  // The parameter will accept the 
                                                                      // data contained in the InLineData
                                                                      // definitions under the Theory tag
        {
            // Arrange
            Employment actual = new Employment();  // Create the default object

            // Act
            // The Action will save the definition of the operation to be performed by each run of the 
            // test.  In this case, the "action"  is the assignment of the input title value to the 
            // Title property of the actual object created above.
            Action action = () => actual.Title = title;

            // Assert
            // A test passes if the expected exception is thrown with a message that matches the 
            // pattern provided in the WithMessage() call.  The * represents 0 or more characters.
            // Any other characters must match exactly.  In this case, the exception message must end
            // with " is required!" because there is no * after the provided text.
            action.Should().Throw<ArgumentException>().WithMessage("* is required!");

        }

        [Fact]
        public void Allow_Change_To_Years()
        {
            // Arrange - Setting up the testing scenario
            Employment actual = new Employment();
            double expectedYears = 34.5;

            // Act - The action that we are trying to test
            actual.Years = 34.5;

            // Assert - The verification that the action is working correctly (or not)
            actual.Years.Should().Be(expectedYears);
        }

        [Theory]
        [InlineData(-0.00000001)]
        [InlineData(40.00000001)]
        public void Throw_Exception_For_Changing_Years(double years)
        {
            // Arrange
            Employment actual = new Employment();

            // Act
            Action action = () => actual.Years = years;  // A straight assignment is satisfactory here 
                                                         // because the property is testing against a 
                                                         // range.  If equality was being looked for as
                                                         // the test, then rounding both values to the
                                                         // same number of decimal places is required
                                                         // to eliminate epsilon errors.

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("* out of range*");
        }

        [Fact]
        public void Allow_Change_To_SupervisoryLevel()
        {
            // Arrange - Setting up the testing scenario
            Employment actual = new Employment();
            SupervisoryLevel expectedLevel = SupervisoryLevel.Supervisor;

            // Act - The action that we are trying to test
            actual.Level = (SupervisoryLevel)3;

            // Assert - The verification that the action is working correctly (or not)
            actual.Level.Should().Be(expectedLevel);
        }

        [Theory]
        [InlineData((SupervisoryLevel)6)]  // Note that this value is not defined for the 
                                           // SupervisoryLevel enumeration.  More explanation in the 
                                           // property for options for how to conduct the tests.
        public void Throw_Exception_For_Changing_SupervisoryLevel(SupervisoryLevel level)
        {
            // Arrange
            Employment actual = new Employment();

            // Act
            Action action = () => actual.Level = level;

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("* is undefined*");
        }

        [Fact]
        public void Allow_Change_To_StartDate()
        {
            // Arrange - Setting up the testing scenario
            Employment actual = new Employment();
            DateTime expectedLevel = DateTime.Today;  // Using today's date makes the test dynamic so
                                                      // that you do not have to manually change the
                                                      // test over time.

            // Act - The action that we are trying to test
            // Using DateTime.Now as the date from which to extract the year, month, and day values
            // provides an alternate path for testing dynamic values.
            actual.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Assert - The verification that the action is working correctly (or not)
            actual.StartDate.Should().Be(expectedLevel);
        }

        // The following was added to show how to deal with class types in testing.
        // In particular, the "WriteUp" class was added and is now being tested.
        [Fact]
        public void Allow_Change_To_WriteUps()
        {
            // Arrange - Setting up the testing scenario
            Employment actual = new Employment();
            // Set up a sample WriteUp instance
            WriteUp expectedWriteUp = new WriteUp("Late", "Henry", "Had to clean the dishes");

            // Act
            actual.WriteUps = new List<WriteUp>() { expectedWriteUp };
            //actual.WriteUps = new List<WriteUp>();
            //actual.WriteUps.Add(expectedWriteUp);

            // Assert
            // Compare the values of the instance that was added to ensure that the information
            // is being assigned without contamination or throwing exceptions.
            actual.WriteUps[0].Violation.Should().Be(expectedWriteUp.Violation);
            actual.WriteUps[0].SupervisorName.Should().Be(expectedWriteUp.SupervisorName);
            actual.WriteUps[0].Resolution.Should().Be(expectedWriteUp.Resolution);
        }
        #endregion

        #region Constructor Tests
        // We have it set up so that the WriteUp instance in the Employment class will be null when the 
        // the default constructor is used.
        [Fact]
        public void Create_A_Good_Default_Employment()
        {
            // Given - Arrange
            // These values are equivalent to those assigned in the default constructor
            SupervisoryLevel expectedLevel = SupervisoryLevel.TeamMember;
            string expectedTitle = "Hello World!";
            double expectedYears = 10;
            DateTime expectedStartDate = DateTime.Today;

            // When - Act
            Employment actual = new Employment();  // Instantiation using default constructor 

            // Then - Assert
            // Verify the default constructor is assigning the correct values.
            actual.Level.Should().Be(expectedLevel);
            actual.Title.Should().Be(expectedTitle);
            actual.Years.Should().Be(expectedYears);
            actual.StartDate.Should().Be(expectedStartDate);
            actual.WriteUps.Should().BeNull();  // Testing for a null instance
        }

        // The following tests the creation of a good instance including setting up a WriteUp instance for submission to the constructor.
        [Fact]
        public void Create_A_Good_Employment()
        {
            // Given - Arrange
            SupervisoryLevel expectedLevel = SupervisoryLevel.TeamMember;
            string expectedTitle = "Hello World!";
            double expectedYears = 10;
            DateTime expectedStartDate = DateTime.Today;
            WriteUp expectedWriteUp = new WriteUp("Late", "Henry", "Had to clean the dishes");
            List<WriteUp> writeUps = new List<WriteUp>() { expectedWriteUp };


            // When - Act
            Employment actual = new Employment("Hello World!", SupervisoryLevel.TeamMember, DateTime.Today, writeUps, 10.0);

            // Then - Assert
            actual.Level.Should().Be(expectedLevel);
            actual.Title.Should().Be(expectedTitle);
            actual.Years.Should().Be(expectedYears);
            actual.StartDate.Should().Be(expectedStartDate);
            actual.WriteUps[0].Violation.Should().Be(expectedWriteUp.Violation);
            actual.WriteUps[0].SupervisorName.Should().Be(expectedWriteUp.SupervisorName);
            actual.WriteUps[0].Resolution.Should().Be(expectedWriteUp.Resolution);
        }

        // Note that when dealing with a constructor that requires a class object to be submitted, you may either
        // choose to submit a null, assuming the class allows it, or you may create a class instance in the Arrange
        // section of the test.
        [Theory]
        [InlineData(null, SupervisoryLevel.Supervisor, "2024-09-20", 25.7)]
        [InlineData("", SupervisoryLevel.Supervisor, "2024-09-20", 25.7)]
        [InlineData("    ", SupervisoryLevel.Supervisor, "2024-09-20", 25.7)]
        [InlineData("Hello World!", (SupervisoryLevel)(-1), "2024-09-20", 25.7)]
        [InlineData("Hello World!", (SupervisoryLevel)6, "2024-09-20", 25.7)]
        [InlineData("Hello World!", SupervisoryLevel.Supervisor, "2024-09-20", -0.0001)]
        [InlineData("Hello World!", SupervisoryLevel.Supervisor, "2024-09-20", 40.0001)]
        public void Throw_Exception_For_Bad_Data_Using_Constructor
                        (string title, SupervisoryLevel level, DateTime startDate, double years)
        {
            // Given - Arrange
            //DateTime start = DateTime.Parse("2024-09-20");  // This Parse call is why the string date         
            // submission above is working

            //WriteUp writeUp = new WriteUp("Testing", "Somebody", "Writing Lines");  // If an instance is required, replace
            // the null with "writeUp"

            // When - Act
            Action action = () => new Employment(title, level, startDate, null, years);

            // Then - Assert
            action.Should().Throw<ArgumentException>();
        }
        #endregion

        #region Method Tests
        // The following merely tests an output method similar to that you have seen in the first exercise.
        [Fact]
        public void Test_ToString_Return_As_Expected()
        {
            // Given - Arrange
            SupervisoryLevel expectedLevel = SupervisoryLevel.TeamMember;
            string expectedTitle = "Hello World!";
            double expectedYears = 10;
            DateTime expectedStartDate = DateTime.Today;
            Employment actual = new Employment(expectedTitle, expectedLevel, expectedStartDate, null, expectedYears);
            string expectedOutput = $"{expectedTitle},{expectedLevel},{expectedStartDate.ToShortDateString()},{expectedYears}";

            // Act
            string actualOutput = actual.ToString();

            // Assert
            actualOutput.Should().Be(expectedOutput);
        }
        #endregion

        #region Testing Static Method in Employment Class
        [Fact]
        public void Test_Value_Is_Divisible_By_Six()
        {
            // Arrange
            //Employment employment = new Employment();  

            // Act
            bool isDivisible = Employment.DivisibleBySix(216);

            // Assert
            isDivisible.Should().BeTrue();
        }

        [Fact]
        public void Test_Value_Is_Not_Divisible_By_Six()
        {
            // Arrange

            // Act
            bool isDivisible = Employment.DivisibleBySix(100);

            // Assert
            isDivisible.Should().BeFalse();
        }
        #endregion
    }
}