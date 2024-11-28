using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public static class Utility
    {
        private static long trans_ID;
        private static CultureInfo culture = new CultureInfo("en-US");
        public static string GetSecretInput(string prompt)
        {
            StringBuilder input = new StringBuilder();
            Console.WriteLine(prompt);

            while (true)
            {
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 6)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\nPlease enter 6 digits", false);
                        input.Clear();
                        Console.WriteLine(prompt);
                        continue;
                    }
                }

                if (inputKey.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");  // Clear the last asterisk
                }
                else if (inputKey.Key != ConsoleKey.Backspace && char.IsDigit(inputKey.KeyChar))
                {
                    // Allow typing more than 6 digits
                    input.Append(inputKey.KeyChar);
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return input.ToString();
        }

        public static void PrintMessage(string msg, bool success = true)
        {
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
            PressEnterToContinue();
        }

        public static string GetuserInput(string prompt)
        {
            Console.WriteLine($"Enter {prompt}");
            return Console.ReadLine();
        }

        public static void PrintDotAnimation(int timer = 10)
        {
            for (int i = 0; i < timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }


        public static void PressEnterToContinue()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
        public static string FormatAmount(decimal amount) {
            return String.Format(culture, "{0:C2}", amount);
        } 
        public static long GetTransactionID()
        {
            return ++trans_ID;
        }



    }
}