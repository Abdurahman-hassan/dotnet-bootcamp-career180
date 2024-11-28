using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM
{
    public enum UserCategory { Ordinary, VIP }
    
    public enum OperationType
    {
        Login,
        Logout,
        UserCreation,
        BalanceCheck,
        Deposit,
        Withdrawal,
        MoneyTransfer,
        MoneyReceive
    }

    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public UserCategory Category { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new();

        public User(string username, string password, string email, DateTime birthDate)
        {
            UserId = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            Email = email;
            BirthDate = birthDate;
            Balance = 0;
            Category = UserCategory.Ordinary;
        }
    }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public Transaction(DateTime date, string description, decimal amount)
        {
            Date = date;
            Description = description;
            Amount = amount;
        }
    }

    public class TransactionInfo
    {
        public string UserId { get; }
        public OperationType OperationType { get; }
        public decimal Amount { get; }
        public decimal BalanceBefore { get; }
        public decimal BalanceAfter { get; }
        public string? ReceiverId { get; }

        public TransactionInfo(string userId, OperationType operationType, decimal amount,
            decimal balanceBefore, decimal balanceAfter, string? receiverId = null)
        {
            UserId = userId;
            OperationType = operationType;
            Amount = amount;
            BalanceBefore = balanceBefore;
            BalanceAfter = balanceAfter;
            ReceiverId = receiverId;
        }
    }

    public class PendingTransfer
    {
        public string TransferId { get; }
        public string FromUserId { get; }
        public string ToUserId { get; }
        public decimal Amount { get; }
        public DateTime CreatedAt { get; }

        public PendingTransfer(string fromUserId, string toUserId, decimal amount)
        {
            TransferId = Guid.NewGuid().ToString();
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Amount = amount;
            CreatedAt = DateTime.Now;
        }
    }

    public class Atm
    {
        private List<User> users = new();
        private List<TransactionInfo> operationHistory = new();
        private List<PendingTransfer> pendingTransfers = new();
        private User? currentUser;

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n1. Sign Up");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Delete User");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        SignUp();
                        break;
                    case "2":
                        if (Login())
                        {
                            PerformOperations();
                        }
                        break;
                    case "3":
                        DeleteUser();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        private void LogOperation(User user, OperationType operationType, decimal amount = 0, 
            decimal balanceBefore = 0, string? receiverId = null)
        {
            var transactionInfo = new TransactionInfo(
                user.UserId,
                operationType,
                amount,
                balanceBefore,
                user.Balance,
                receiverId
            );
            operationHistory.Add(transactionInfo);
        }

        private void SignUp()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter birth date (yyyy-MM-dd):");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
            {
                var newUser = new User(username, password, email, birthDate);
                users.Add(newUser);
                LogOperation(newUser, OperationType.UserCreation);
                Console.WriteLine("User registration successful.");
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }

        private void DeleteUser()
        {
            Console.WriteLine("Enter username to delete:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            
            User userToDelete = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                Console.WriteLine("User deleted successfully.");
            }
            else
            {
                Console.WriteLine("Username or password is incorrect.");
            }
        }

        private void MoneyTransfer()
        {
            Console.WriteLine("Enter receiver's username:");
            string receiverName = Console.ReadLine();
            
            var receiver = users.FirstOrDefault(u => u.Username == receiverName);
            if (receiver == null)
            {
                Console.WriteLine("Receiver not found.");
                return;
            }

            Console.WriteLine("Enter amount to transfer:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (currentUser.Balance >= amount)
                {
                    var pendingTransfer = new PendingTransfer(currentUser.UserId, receiver.UserId, amount);
                    pendingTransfers.Add(pendingTransfer);
                    Console.WriteLine($"Transfer pending. Transfer ID: {pendingTransfer.TransferId}");
                    Console.WriteLine("Receiver will need to accept the transfer.");
                }
                else
                {
                    Console.WriteLine("Insufficient funds for transfer.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        private void HandlePendingTransfers()
        {
            var userPendingTransfers = pendingTransfers
                .Where(pt => pt.ToUserId == currentUser.UserId)
                .ToList();

            if (!userPendingTransfers.Any())
            {
                Console.WriteLine("No pending transfers.");
                return;
            }

            foreach (var transfer in userPendingTransfers)
            {
                var sender = users.First(u => u.UserId == transfer.FromUserId);
                Console.WriteLine($"Transfer ID: {transfer.TransferId}");
                Console.WriteLine($"From: {sender.Username}");
                Console.WriteLine($"Amount: {transfer.Amount}");
                Console.WriteLine($"Date: {transfer.CreatedAt}");
            
                Console.WriteLine("Accept this transfer? (y/n)");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    if (sender.Balance >= transfer.Amount)
                    {
                        decimal senderBalanceBefore = sender.Balance;
                        decimal receiverBalanceBefore = currentUser.Balance;

                        sender.Balance -= transfer.Amount;
                        currentUser.Balance += transfer.Amount;

                        LogOperation(sender, OperationType.MoneyTransfer, transfer.Amount, senderBalanceBefore, currentUser.UserId);
                        LogOperation(currentUser, OperationType.MoneyReceive, transfer.Amount, receiverBalanceBefore);
                        
                        pendingTransfers.Remove(transfer);
                        Console.WriteLine("Transfer accepted and processed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Transfer failed - sender has insufficient funds.");
                    }
                }
                else
                {
                    pendingTransfers.Remove(transfer);
                    Console.WriteLine("Transfer rejected.");
                }
            }
        }

        private bool Login()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            
            currentUser = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            
            if (currentUser != null)
            {
                LogOperation(currentUser, OperationType.Login);
                Console.WriteLine("Login successful.");
                return true;
            }
            
            Console.WriteLine("Username or password is incorrect.");
            return false;
        }

        private void PerformOperations()
        {
            while (true)
            {
                Console.WriteLine("\n1. Check balance");
                Console.WriteLine("2. Make a deposit");
                Console.WriteLine("3. Withdraw Balance");
                Console.WriteLine("4. Transfer Money");
                Console.WriteLine("5. View Pending Transfers");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");
                
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CheckBalance();
                        break;
                    case "2":
                        MakeDeposit();
                        break;
                    case "3":
                        WithdrawBalance();
                        break;
                    case "4":
                        MoneyTransfer();
                        break;
                    case "5":
                        HandlePendingTransfers();
                        break;
                    case "6":
                        LogOperation(currentUser, OperationType.Logout);
                        return;
                }
                
                Console.WriteLine("\nDo you want to perform any other operations? (y/n)");
                if (Console.ReadLine()?.ToLower() != "y")
                {
                    LogOperation(currentUser, OperationType.Logout);
                    break;
                }
            }
        }

        private void CheckBalance()
        {
            decimal balanceBefore = currentUser.Balance;
            Console.WriteLine($"Your current balance is {currentUser.Balance}");
            LogOperation(currentUser, OperationType.BalanceCheck, 0, balanceBefore);
            currentUser.Transactions.Add(new Transaction(DateTime.Now, "Balance checked", currentUser.Balance));
        }

        private void MakeDeposit()
        {
            Console.WriteLine("Enter the deposit amount:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                decimal balanceBefore = currentUser.Balance;
                currentUser.Balance += amount;
                LogOperation(currentUser, OperationType.Deposit, amount, balanceBefore);
                currentUser.Transactions.Add(new Transaction(DateTime.Now, "Deposit made", amount));
                Console.WriteLine($"Your new balance is {currentUser.Balance}");
            }
            else
            {
                Console.WriteLine("Invalid deposit amount, please enter a positive number.");
            }
        }

        private void WithdrawBalance()
        {
            Console.WriteLine("Enter the withdrawal amount:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (amount <= currentUser.Balance)
                {
                    decimal balanceBefore = currentUser.Balance;
                    currentUser.Balance -= amount;
                    LogOperation(currentUser, OperationType.Withdrawal, amount, balanceBefore);
                    currentUser.Transactions.Add(new Transaction(DateTime.Now, "Withdrawal made", -amount));
                    Console.WriteLine($"Withdrawal successful. Your balance is now {currentUser.Balance}");
                }
                else
                {
                    Console.WriteLine("Insufficient balance.");
                }
            }
            else
            {
                Console.WriteLine("Invalid withdrawal amount, please enter a positive number.");
            }
        }
    }
}