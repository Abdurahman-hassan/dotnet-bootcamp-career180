# 001 - Assignment
# using while loop
using System;

class Program
{
    static void Main()
    {
        string name = "";
        string city = "";

        // Loop until the correct name and city are entered
        while (name != "Alex" || city != "New York")
        {
            Console.Write("Enter your name: ");
            name = Console.ReadLine();

            Console.Write("Enter your city: ");
            city = Console.ReadLine();

            if (name != "Alex")
            {
                Console.WriteLine("Incorrect name. Please try again.");
            }
            else if (city != "New York")
            {
                Console.WriteLine("Incorrect city. Please try again.");
            }
        }

        // If the correct name and city are entered, print the full address
        Console.WriteLine($"{name} Johnson\n23 Main Street\n{city}, NY 10001\nUSA");
    }
}


// using for loop
# using for loop
using System;

class Program
{
    static void Main()
    {
        string name = "";
        string city = "";

        for (;;)
        {
            Console.Write("Enter your name: ");
            name = Console.ReadLine();

            Console.Write("Enter your city: ");
            city = Console.ReadLine();

            if (name == "Alex" && city == "New York")
            {
                Console.WriteLine($"{name} Johnson\n23 Main Street\n{city}, NY 10001\nUSA");
                break;
            }
            else
            {
                // Incorrect input; ask again
                if (name != "Alex")
                {
                    Console.WriteLine("Incorrect name. Please try again.");
                }
                if (city != "New York")
                {
                    Console.WriteLine("Incorrect city. Please try again.");
                }
            }
        }
    }
}

// using do-while loop
# using do-while loop
using System;

class Program
{
    static void Main()
    {
        string name = "";
        string city = "";

        do
        {
            Console.Write("Enter your name: ");
            name = Console.ReadLine();

            Console.Write("Enter your city: ");
            city = Console.ReadLine();

            if (name != "Alex")
            {
                Console.WriteLine("Incorrect name. Please try again.");
            }
            else if (city != "New York")
            {
                Console.WriteLine("Incorrect city. Please try again.");
            }
        } while (name != "Alex" || city != "New York");

        Console.WriteLine($"{name} Johnson\n23 Main Street\n{city}, NY 10001\nUSA");
    }
}