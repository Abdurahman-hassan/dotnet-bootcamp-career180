using System;
using System.Collections.Generic;

namespace ATMApp
{
    // Class to represent a transaction
    class Transaction
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Transaction(string type, decimal amount)
        {
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Date}: {Type} - {Amount:C}";
        }
    }

    // Class to represent the user account
    class Account
    {
        public string Username { get; private set; }
        private string Password;
        public decimal Balance { get; private set; } = 1000.00m; // Initial balance
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public Account(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public bool Authenticate(string password)
        {
            return Password == password;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            AddTransaction(new Transaction("Deposit", amount));
            Console.WriteLine($"Successfully deposited {amount:C}. New balance: {Balance:C}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                AddTransaction(new Transaction("Withdraw", amount));
                Console.WriteLine($"Successfully withdrew {amount:C}. New balance: {Balance:C}");
            }
            else
            {
                Console.WriteLine("Insufficient balance.");
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void DisplayBalance()
        {
            Console.WriteLine($"Your current balance is: {Balance:C}");
        }

        public void DisplayTransactions()
        {
            Console.WriteLine("Transaction History:");
            foreach (var transaction in Transactions)
            {
                Console.WriteLine(transaction);
            }
        }
    }

    // Main ATM Program
    class Program
    {
        static List<Account> accounts = new List<Account>();

        static void Main(string[] args)
        {
            // Add a default user
            accounts.Add(new Account("User1000", "Passw0rd"));

            Console.WriteLine("Welcome to the ATM Program!");

            bool exit = false;
            while (!exit)
            {
                // Main menu
                Console.WriteLine("\nPlease choose an option:");
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Add New User");
                Console.WriteLine("3 - Delete User");
                Console.WriteLine("4 - Exit");

                Console.Write("Enter your choice: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1":
                        // Login
                        Console.Write("Enter Username: ");
                        string username = Console.ReadLine();

                        Console.Write("Enter Password: ");
                        string password = Console.ReadLine();

                        Account account = FindAccount(username, password);
                        if (account != null)
                        {
                            Console.WriteLine("Login successful!");
                            UserMenu(account);
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                        }
                        break;

                    case "2":
                        // Add New User
                        Console.Write("Enter New Username: ");
                        string newUsername = Console.ReadLine();

                        Console.Write("Enter New Password: ");
                        string newPassword = Console.ReadLine();

                        if (FindAccount(newUsername) == null)
                        {
                            accounts.Add(new Account(newUsername, newPassword));
                            Console.WriteLine("User added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Username already exists.");
                        }
                        break;

                    case "3":
                        // Delete User
                        Console.Write("Enter Username to Delete: ");
                        string deleteUsername = Console.ReadLine();

                        Console.Write("Enter Password: ");
                        string deletePassword = Console.ReadLine();

                        Account accountToDelete = FindAccount(deleteUsername, deletePassword);
                        if (accountToDelete != null)
                        {
                            accounts.Remove(accountToDelete);
                            Console.WriteLine("User deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                        }
                        break;

                    case "4":
                        // Exit
                        Console.WriteLine("Exiting the program. Thank you!");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        // Method to find an account based on username and password
        static Account FindAccount(string username, string password = null)
        {
            foreach (var account in accounts)
            {
                if (account.Username == username && (password == null || account.Authenticate(password)))
                {
                    return account;
                }
            }
            return null;
        }

        // User Menu for account operations
        static void UserMenu(Account account)
        {
            bool exit = false;
            while (!exit)
            {
                // User operations menu
                Console.WriteLine("\nPlease choose an option:");
                Console.WriteLine("1 - Check Balance");
                Console.WriteLine("2 - Deposit");
                Console.WriteLine("3 - Withdraw");
                Console.WriteLine("4 - View Transaction History");
                Console.WriteLine("5 - Logout");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Check Balance
                        account.DisplayBalance();
                        break;

                    case "2":
                        // Deposit
                        Console.Write("Enter amount to deposit: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount) && depositAmount > 0)
                        {
                            account.Deposit(depositAmount);
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount entered.");
                        }
                        break;

                    case "3":
                        // Withdraw
                        Console.Write("Enter amount to withdraw: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount) && withdrawAmount > 0)
                        {
                            account.Withdraw(withdrawAmount);
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount entered.");
                        }
                        break;

                    case "4":
                        // View Transaction History
                        account.DisplayTransactions();
                        break;

                    case "5":
                        // Logout
                        Console.WriteLine("Logging out...");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}
