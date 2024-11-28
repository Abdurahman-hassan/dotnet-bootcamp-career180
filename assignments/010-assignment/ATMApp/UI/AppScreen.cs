using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public class AppScreen
    {
        internal const string Cur = "$ ";

        internal static void Welcome()
        {

            Console.Clear();
            Console.Title = "My ATM App";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n-------------Welcome to my ATM app-------------\n\n");
            //ask the user for card number
            Console.WriteLine("Please insert your ATM card");
            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validator.Convert<long>("your card number.");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("enter your Card PIN"));
            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking Card Number and Card Pin...");
            Utility.PrintDotAnimation();
        }
        internal static void PrintLockScreen() 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Utility.PrintMessage("Your account is Locked, please go to the nearest branch to unlock your account" +
                " Thank you.", false);
            Console.ForegroundColor = ConsoleColor.White;
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }


    internal static void WelcomeCustomer(string fullName)
    {

        Console.WriteLine($"Welcome back, {fullName}");
    }
        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("------------My App Menu------------");
            Console.WriteLine(":                                  :");
            Console.WriteLine("1. Account Balance               :");
            Console.WriteLine("2. Cash Deposit               :");
            Console.WriteLine("3. Withdrawal               :");
            Console.WriteLine("4. Transfer               :");
            Console.WriteLine("5. Transactions               :");
            Console.WriteLine("6. Pending Transfers               :");  
            Console.WriteLine("7. LogOut               :");
        }
        internal static void LogOutProgress() {
            Console.WriteLine("thanks for using my ATM app");
            Utility.PrintDotAnimation();
            Console.Clear();
        }
        internal static int SelectAmount() {
            Console.WriteLine("");
            Console.WriteLine($":1.{Cur}500         5.{Cur}10,000");
            Console.WriteLine($":2.{Cur}1000         6.{Cur}15,000");
            Console.WriteLine($":3.{Cur}2000         7. {Cur}20,000");
            Console.WriteLine($":4.{Cur}5000         8. {Cur}40,000");
            Console.WriteLine($":0.other");
            Console.WriteLine("");
            int selectedAmount = Validator.Convert<int>("option: ");

            switch (selectedAmount) { 
                case 1:
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("invalid input. try again");
                    return -1;
                    break;
            }
        }
        internal InternalTransfer InternalTransferForm()
        {
            var InternalTransfer = new InternalTransfer();
            InternalTransfer.RecipientBankAccountNumber = Validator.Convert<long>("recipient's account number"); ;
            InternalTransfer.TransferAmount = Validator.Convert<decimal>($"amount{Cur}");
            InternalTransfer.RecipientBankAccountName = Utility.GetuserInput("recipient name");
            return InternalTransfer;
        }
    }
}
