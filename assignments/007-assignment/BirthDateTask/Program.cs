using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using DateTime = System.DateTime;

namespace BirthDate;

class Program
{
    static void Main(string[] args)
    {
        // from the user input 'birthdate' we will tell him what is the day he was born,
        // using his month, which season is it?
        // is the year he was born in a leap year?
        // how many days left for his next birthday?
        while (true)
        {
            try
            {
                Console.WriteLine("Enter your birth date in format dd.MM.yyyy");
                string dateInput = Console.ReadLine();
                DateTime birthDate = DateTime.ParseExact(dateInput, "dd.MM.yyyy", null);
                // day 
                string dayOfWeek = birthDate.DayOfWeek.ToString();
                Console.WriteLine($"you were born on {dayOfWeek}");
                // season
                string season = GetSeason(birthDate.Month);
                Console.WriteLine($"you were born on {season}");
                // check if his birthday is a leap year?
                bool isLeapYear = DateTime.IsLeapYear(birthDate.Year);
                Console.WriteLine($" Is the year you were born in a leap year? \n {isLeapYear}");
                // days to his next birthday
                DateTime today = DateTime.Now;
                DateTime currentYearBirthday= new DateTime(DateTime.Today.Year , birthDate.Month, birthDate.Day);
                // int daysLeftForNextBirthday = currentYearBirthday.Subtract(today).Days;
                int daysLeftForNextBirthday = (currentYearBirthday - DateTime.Today).Days;
                Console.WriteLine($"days left for next birthday is : {daysLeftForNextBirthday}");

            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input, please try again.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("an error occured, {0} ",ex.Message);
            }
        }
    }

static string GetSeason(int month) 
{ 
    return month switch
    { 
        12 or 1 or 2 => "Winter",
        3 or 4 or 5 => "Spring",
        6 or 7 or 8 => "Summer",
        9 or 10 or 11 => "Autumn", 
         _ => throw new ArgumentException("Invalid month") 
    } ; 
}

}


