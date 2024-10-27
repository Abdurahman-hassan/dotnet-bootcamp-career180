using System;
using System.Collections.Generic;

namespace ATMApp
{
    // Class to represent a user
    class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string UserCategory { get; set; } // "VIP" or "Ordinary"
        public Account Account { get; set; }

        public User(string userName, string password, DateTime birthDate, string email, string userCategory)
        {
            UserId = Guid.NewGuid();
            UserName = userName;
            Password = password;
            BirthDate = birthDate;
            Email = email;
            UserCategory = userCategory;
            Account = new Account(userName);
        }
    }

    // Class to represent a transaction
    class TransactionInfo
    {
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
        public string OperationId { get; set; }
        public decimal OperationAmount { get; set; }
        public DateTime OperationDateTime { get; set; }
        public decimal UserBalanceBeforeOperation { get; set; }
        public decimal UserBalanceAfterOperation { get; set; }
        public Guid? ReceiverId { get; set; }
        public bool IsCompleteTransfer { get; set; }

        public TransactionInfo(Guid userId, string operationId, decimal operationAmount, decimal balanceBefore, decimal balanceAfter, Guid? receiverId = null, bool isCompleteTransfer = false)
        {
            TransactionId = Guid.NewGuid();
            UserId = userId;
            OperationId = operationId;
            OperationAmount = operationAmount;
            OperationDateTime = DateTime.Now;
            UserBalanceBeforeOperation = balanceBefore;
            UserBalanceAfterOperation = balanceAfter;
            ReceiverId = receiverId;
            IsCompleteTransfer = isCompleteTransfer;
        }

        public override string ToString()
        {
            return $"Transaction ID: {TransactionId}, User ID: {UserId}, Operation: {OperationId}, Amount: {OperationAmount:C}, Date: {OperationDateTime}, Balance Before: {UserBalanceBeforeOperation:C}, Balance After: {UserBalanceAfterOperation:C}, Receiver ID: {ReceiverId}, Transfer Complete: {IsCompleteTransfer}";
        }
    }

    // Class to represent the user account
    class Account
    {
        public string UserName { get; private set; }
        public decimal Balance { get; private set; } = 1000.00m; // Initial balance

        public Account(string userName)
        {
            UserName = userName;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public decimal GetBalance()
        {
            return Balance;
        }
    }

    // Main ATM Program
    class Program
    {
        static List<User> users = new List<User>();
        static List<TransactionInfo> operationHistory = new List<TransactionInfo>();

        static void Main(string[] args)
        {
            // Sample user for testing
            users.Add(new User("User1000", "Passw0rd", new DateTime(1990, 1, 1), "user1000@example.com", "Ordinary"));

            Console.WriteLine("Welcome to the ATM Program!");

            bool exit = false;
            while (!exit)
            {
                // Main menu
                Console.WriteLine("\nPlease choose an option:");
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Add New User");
                Console.WriteLine("3 - Delete User");
                Console.WriteLine("4 - Transfer Money");
                Console.WriteLine("5 - View Operation History");
                Console.WriteLine("6 - Exit");

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

                        User user = FindUserByUsernameAndPassword(username, password);
                        if (user != null)
                        {
                            Console.WriteLine("Login successful!");
                            RecordOperation(user.UserId, "Login", 0, user.Account.GetBalance(), user.Account.GetBalance());
                            UserMenu(user);
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                        }
                        break;

                    case "2":
                        // Add New User
                        AddNewUser();
                        break;

                    case "3":
                        // Delete User
                        DeleteUser();
                        break;

                    case "4":
                        // Transfer Money
                        PerformMoneyTransfer();
                        break;

                    case "5":
                        // View Operation History
                        DisplayOperationHistory();
                        break;

                    case "6":
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

        static void AddNewUser()
        {
            Console.Write("Enter New Username: ");
            string newUsername = Console.ReadLine();

            Console.Write("Enter Password: ");
            string newPassword = Console.ReadLine();

            Console.Write("Enter Birthdate (yyyy-mm-dd): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter User Category (VIP or Ordinary): ");
            string userCategory = Console.ReadLine();

            if (FindUserByUsername(newUsername) == null)
            {
                User newUser = new User(newUsername, newPassword, birthDate, email, userCategory);
                users.Add(newUser);
                Console.WriteLine("User added successfully.");
                RecordOperation(newUser.UserId, "User Creation", 0, newUser.Account.GetBalance(), newUser.Account.GetBalance());
            }
            else
            {
                Console.WriteLine("Username already exists.");
            }
        }

        static void DeleteUser()
        {
            Console.Write("Enter Username to Delete: ");
            string deleteUsername = Console.ReadLine();

            User userToDelete = FindUserByUsername(deleteUsername);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                Console.WriteLine("User deleted successfully.");
                RecordOperation(userToDelete.UserId, "User Deletion", 0, 0, 0);
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        static void PerformMoneyTransfer()
        {
            Console.Write("Enter Sender Username: ");
            string senderName = Console.ReadLine();
            User sender = FindUserByUsername(senderName);

            Console.Write("Enter Receiver Username: ");
            string receiverName = Console.ReadLine();
            User receiver = FindUserByUsername(receiverName);

            Console.Write("Enter Transfer Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            if (sender != null && receiver != null && amount > 0)
            {
                if (sender.Account.Withdraw(amount))
                {
                    receiver.Account.Deposit(amount);
                    RecordOperation(sender.UserId, "Transfer", amount, sender.Account.GetBalance() + amount, sender.Account.GetBalance(), receiver.UserId, true);
                    RecordOperation(receiver.UserId, "Receive", amount, receiver.Account.GetBalance() - amount, receiver.Account.GetBalance());
                    Console.WriteLine("Transfer successful.");
                }
                else
                {
                    Console.WriteLine("Insufficient funds in the sender's account.");
                }
            }
            else
            {
                Console.WriteLine("Invalid transfer details.");
            }
        }

        static User FindUserByUsernameAndPassword(string username, string password)
        {
            return users.Find(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }

        static User FindUserByUsername(string username)
        {
            return users.Find(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        static void UserMenu(User user)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nUser Menu:");
                Console.WriteLine("1 - Check Balance");
                Console.WriteLine("2 - Logout");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"Current Balance: {user.Account.GetBalance():C}");
                        RecordOperation(user.UserId, "Check Balance", 0, user.Account.GetBalance(), user.Account.GetBalance());
                        break;

                    case "2":
                        Console.WriteLine("Logging out...");
                        RecordOperation(user.UserId, "Logout", 0, user.Account.GetBalance(), user.Account.GetBalance());
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void RecordOperation(Guid userId, string operationId, decimal amount, decimal balanceBefore, decimal balanceAfter, Guid? receiverId = null, bool isCompleteTransfer = false)
        {
            TransactionInfo transaction = new TransactionInfo(userId, operationId, amount, balanceBefore, balanceAfter, receiverId, isCompleteTransfer);
            operationHistory.Add(transaction);
        }

        static void DisplayOperationHistory()
        {
            Console.WriteLine("\nOperation History:");
            foreach (var transaction in operationHistory)
            {
                Console.WriteLine(transaction);
            }
        }
    }
}
