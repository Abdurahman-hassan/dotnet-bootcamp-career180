using System;

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
        private string Username = "User1000";
        private string Password = "Passw0rd";
        public decimal Balance { get; private set; } = 1000.00m; // Initial balance
        public Transaction[] Transactions { get; private set; } = new Transaction[100];
        private int transactionCount = 0;

        public bool Authenticate(string username, string password)
        {
            return Username == username && Password == password;
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
            if (transactionCount < Transactions.Length)
            {
                Transactions[transactionCount] = transaction;
                transactionCount++;
            }
            else
            {
                Console.WriteLine("Transaction log is full.");
            }
        }

        public void DisplayBalance()
        {
            Console.WriteLine($"Your current balance is: {Balance:C}");
        }

        public void DisplayTransactions()
        {
            Console.WriteLine("Transaction History:");
            for (int i = 0; i < transactionCount; i++)
            {
                Console.WriteLine(Transactions[i].ToString());
            }
        }
    }

    // Main ATM Program
    class Program
    {
        static void Main(string[] args)
        {
            Account account = new Account();

            Console.WriteLine("Welcome to the ATM Program!");

            // User Authentication
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            if (account.Authenticate(username, password))
            {
                Console.WriteLine("Login successful!\n");

                bool exit = false;
                while (!exit)
                {
                    // Display menu
                    Console.WriteLine("Please choose an option:");
                    Console.WriteLine("1 - Check Balance");
                    Console.WriteLine("2 - Deposit");
                    Console.WriteLine("3 - Withdraw");
                    Console.WriteLine("4 - Exit");

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
                            // Exit
                            Console.WriteLine("Exiting the program. Thank you!");
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }

                    Console.WriteLine(); // Add a line for spacing
                }
            }
            else
            {
                Console.WriteLine("Invalid username or password. Access denied.");
            }
        }
    }
}
