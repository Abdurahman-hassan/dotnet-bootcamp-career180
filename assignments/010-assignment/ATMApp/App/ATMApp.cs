using System;
using System.Collections.Generic;
using System.Linq;
using ATMApp.Domain.Entities;
using ATMApp.Domain.Entities.Interfaces;
using ATMApp.Domain.Enums;
using ATMApp.UI;
using ConsoleTables;

namespace ATMApp.App
{
    // Add these new enums and classes before the ATMApp class
    public enum TransferStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public class PendingTransfer
    {
        public long TransferId { get; set; }
        public long SenderAccountNumber { get; set; }
        public string SenderAccountName { get; set; }
        public long RecipientAccountNumber { get; set; }
        public string RecipientAccountName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public TransferStatus Status { get; set; }
    }

    public partial class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount> userAccountlist;
        private UserAccount SelectedAcccount;
        private List<Transaction> _ListOfTransactions;
        private List<PendingTransfer> _pendingTransfers;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen screen;

        public ATMApp()
        {
            screen = new AppScreen();
            _pendingTransfers = new List<PendingTransfer>();
        }

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(SelectedAcccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuOption();
            }
        }

        public void InitializeData()
        {
            userAccountlist = new List<UserAccount>
            {
                new UserAccount{Id = 1, FullName="Ahmed Abou Gabal", AccountNumber=123456, CardNumber = 321321 , CardPin=241023,
                AccountBalance= 10000m, isLocked=false},
                new UserAccount{Id = 2, FullName="Mr X", AccountNumber=12345678, CardNumber = 32132132 , CardPin=241023,
                AccountBalance= 5000m, isLocked=false},
                new UserAccount{Id = 3, FullName="Mrs Y", AccountNumber=123456789, CardNumber = 321321321 , CardPin=241023,
                AccountBalance= 700m, isLocked=true}
            };
            _ListOfTransactions = new List<Transaction>();
        }

        public void CheckUserCardNumAndPassword()
        {
            // check for credentials against the predefined list database
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in userAccountlist)
                {
                    SelectedAcccount = account;
                    if (inputAccount.CardNumber.Equals(SelectedAcccount.CardNumber))
                    {
                        SelectedAcccount.TotalLogin++;
                        if (inputAccount.CardPin.Equals(SelectedAcccount.CardPin))
                        {
                            SelectedAcccount = account;
                            if (SelectedAcccount.isLocked || SelectedAcccount.TotalLogin > 3)
                            {
                                //print a lock message on the screen to the screen
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                SelectedAcccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                    if (isCorrectLogin == false)
                    {
                        Utility.PrintMessage("\n invalid CardNumber or CardPIN", false);
                        SelectedAcccount.isLocked = SelectedAcccount.TotalLogin == 3;
                        if (SelectedAcccount.isLocked)
                        {
                            AppScreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();
                }
            }
        }

        private void ProcessMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var InternalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(InternalTransfer);
                    break;
                case (int)AppMenu.ViewTransactions:
                    ViewTransaction();
                    break;
                case (int)AppMenu.ViewPendingTransfers: // Add new menu option
                    ViewPendingTransfers();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogOutProgress();
                    Utility.PrintMessage("you have successfully logged out, please collect your ATM card");
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid option.", false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"your account balance is: {Utility.FormatAmount(SelectedAcccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\n Only multiples of 500 and 1000 are allowed\n");
            var transaction_amount = Validator.Convert<int>($"amount {AppScreen.Cur}");

            // simulate counting 
            Console.WriteLine("\n Checking and Counting bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            // some guard clauses
            if (transaction_amount < 0)
            {
                Utility.PrintMessage("Amount needs to be greater than 0, try again", false);
                return;
            }
            if (transaction_amount % 500 != 0)
            {
                Utility.PrintMessage("Enter Deposit Amount in multiples of 500 or 1000, try again", false);
                return;
            }
            if (PreviewBankNotesCount(transaction_amount) == false)
            {
                Utility.PrintMessage($"You have cancelled your action", false);
                return;
            }
            // bind transaction details to transaction objects
            InsertTransaction(SelectedAcccount.Id, TransactionType.Deposit, transaction_amount, "");

            // update account balance 
            SelectedAcccount.AccountBalance += transaction_amount;

            // print success message to the screen
            Utility.PrintMessage($"Your Deposit of {Utility.FormatAmount(transaction_amount)} was Successful.", true);
        }

        public void MakeWithdrawal()
        {
            var transactionAmount = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if (selectedAmount == -1)
            {
                MakeWithdrawal();
                return;
            }
            else if (selectedAmount != 0)
            {
                transactionAmount = selectedAmount;

            }
            else
            {
                transactionAmount = Validator.Convert<int>($"amount {AppScreen.Cur}");

            }
            //input  validation 
            if (transactionAmount <= 0)
            {
                Utility.PrintMessage("invalid amount, only amounts greater than zero are valid", false);
                return;
            }
            if (transactionAmount % 500 != 0)
            {

                Utility.PrintMessage("you can only withdraw amount of 500 and 1000, try again");
                return;
            }
            // business logic validations
            if (transactionAmount > SelectedAcccount.AccountBalance)
            {
                Utility.PrintMessage("withdraw failed, your balance is not sufficient for the input" +
                    $" {Utility.FormatAmount(transactionAmount)}", false);
                return;
            }
            if ((SelectedAcccount.AccountBalance - transactionAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"withdrawal failed, your account needs to have a minimum {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
            // bind withdrawal to transaction objects
            InsertTransaction(SelectedAcccount.Id, TransactionType.Withdrawal, -transactionAmount, "");
            // update account balance 
            SelectedAcccount.AccountBalance -= transactionAmount;
            // success message 
            Utility.PrintMessage($"you have successfully withdrawn" + $" {Utility.FormatAmount(transactionAmount)}", true);
        }

        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;
            Console.WriteLine("\n Summary");
            Console.WriteLine("------------");
            Console.WriteLine($"{AppScreen.Cur}1000 X {thousandNotesCount} = {1000 * thousandNotesCount}");
            Console.WriteLine($"{AppScreen.Cur}500 X {fiveHundredNotesCount} = {500 * fiveHundredNotesCount}");
            Console.WriteLine($"total amount : {Utility.FormatAmount(amount)}\n \n");

            int opt = Validator.Convert<int>("1 to Confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountID, TransactionType _TransType, decimal _TransAmount, string _Description)
        {
            // create a new transaction object
            var transaction = new Transaction()
            {
                TransactionID = Utility.GetTransactionID(),
                UserBankAccountID = _UserBankAccountID,
                TransactionDate = DateTime.Now,
                TransactionType = _TransType,
                TransactionAmount = _TransAmount,
                Description = _Description
            };
            // add transaction object to list 
            _ListOfTransactions.Add(transaction);
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("transferred amount cannot be equal or less than zero", false);
                return;
            }

            // Check sender's account balance
            if (internalTransfer.TransferAmount > SelectedAcccount.AccountBalance)
            {
                Utility.PrintMessage("TRANSFER FAILED!, you don't have enough balance" +
                    $" to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }

            // Check the minimum kept amount
            if ((SelectedAcccount.AccountBalance - internalTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"transfer failed, your account needs to have minimum " +
                    $"{Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            // Check receiver's account number
            var selectedBankAccountReceiver = (from UserAccount in userAccountlist
                                               where UserAccount.AccountNumber == internalTransfer.RecipientBankAccountNumber
                                               select UserAccount).FirstOrDefault();
            if (selectedBankAccountReceiver == null)
            {
                Utility.PrintMessage("transfer failed, receiver bank account number is invalid", false);
                return;
            }

            // Check receiver's name
            if (selectedBankAccountReceiver.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("transfer failed, receiver's bank account name does not match", false);
                return;
            }

            // Create a pending transfer
            var pendingTransfer = new PendingTransfer
            {
                TransferId = Utility.GetTransactionID(),
                SenderAccountNumber = SelectedAcccount.AccountNumber,
                SenderAccountName = SelectedAcccount.FullName,
                RecipientAccountNumber = internalTransfer.RecipientBankAccountNumber,
                RecipientAccountName = internalTransfer.RecipientBankAccountName,
                Amount = internalTransfer.TransferAmount,
                TransferDate = DateTime.Now,
                Status = TransferStatus.Pending
            };

            _pendingTransfers.Add(pendingTransfer);

            // Print message about pending approval
            Utility.PrintMessage($"Transfer of {Utility.FormatAmount(internalTransfer.TransferAmount)} to {internalTransfer.RecipientBankAccountName} is pending approval.", true);
        }

        private void ViewPendingTransfers()
        {
            var pendingForUser = _pendingTransfers
                .Where(t => t.RecipientAccountNumber == SelectedAcccount.AccountNumber && t.Status == TransferStatus.Pending)
                .ToList();

            if (pendingForUser.Count == 0)
            {
                Utility.PrintMessage("You have no pending transfers to approve.", true);
                return;
            }

            Console.WriteLine("\nPending Transfers:");
            Console.WriteLine("------------------");

            foreach (var transfer in pendingForUser)
            {
                Console.WriteLine($"\nTransfer ID: {transfer.TransferId}");
                Console.WriteLine($"From: {transfer.SenderAccountName}");
                Console.WriteLine($"Amount: {Utility.FormatAmount(transfer.Amount)}");
                Console.WriteLine($"Date: {transfer.TransferDate}");

                Console.Write("\nDo you want to accept this transfer? (1) Accept (2) Reject: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    ProcessTransferAcceptance(transfer);
                }
                else if (choice == "2")
                {
                    ProcessTransferRejection(transfer);
                }
                else
                {
                    Utility.PrintMessage("Invalid choice. Transfer remains pending.", false);
                }
            }
        }

        private void ProcessTransferAcceptance(PendingTransfer transfer)
        {
            // Find sender's account
            var senderAccount = userAccountlist.First(a => a.AccountNumber == transfer.SenderAccountNumber);

            // Process the transfer
            InsertTransaction(senderAccount.Id, TransactionType.Transfer, -transfer.Amount,
                $"Transfer to {SelectedAcccount.AccountNumber} ({SelectedAcccount.FullName})");

            InsertTransaction(SelectedAcccount.Id, TransactionType.Transfer, transfer.Amount,
                $"Transfer from {senderAccount.AccountNumber} ({senderAccount.FullName})");

            // Update balances
            senderAccount.AccountBalance -= transfer.Amount;
            SelectedAcccount.AccountBalance += transfer.Amount;

            // Update transfer status
            transfer.Status = TransferStatus.Accepted;

            Utility.PrintMessage($"Transfer of {Utility.FormatAmount(transfer.Amount)} has been accepted.", true);
        }

        private void ProcessTransferRejection(PendingTransfer transfer)
        {
            transfer.Status = TransferStatus.Rejected;
            Utility.PrintMessage($"Transfer of {Utility.FormatAmount(transfer.Amount)} has been rejected.", true);
        }

        public void ViewTransaction()
        {
            var filteredTransaction = _ListOfTransactions.Where(t => t.UserBankAccountID == SelectedAcccount.Id).ToList();
            // check if there is a transaction
            if (filteredTransaction.Count <= 0)
            {
                Utility.PrintMessage("you have no transactions yet", true);
            }
            else
            {
                var table = new ConsoleTable("ID", "Transaction Date", "Type", "Descriptions", "Amount " + AppScreen.Cur);
                foreach (var transaction in filteredTransaction)
                {
                    table.AddRow(transaction.TransactionID, transaction.TransactionDate
                        , transaction.TransactionType, transaction.Description, transaction.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"you have {filteredTransaction.Count} transaction(s).", true);
            }
        }
    }
}