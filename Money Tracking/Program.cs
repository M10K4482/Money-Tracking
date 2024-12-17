using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Money_Tracking
{
    class Program
    {
        static void Main(string[] args)
        {

            //Create an object getMethods and call LoadData to load data from txt. file
            TransactionHandler getMethods = new TransactionHandler();
            getMethods.LoadData();

            //Show the menu text and let user select an option. Certain numbers calls the apropriate methods for that choosen option.
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to TrackMoney");
                Console.WriteLine($"Your balance is: {getMethods.CalculateTotal():C}");
                Console.WriteLine("");
                Console.WriteLine("Pick an option:");
                Console.WriteLine("(1) Add new income");
                Console.WriteLine("(2) Add new expense");
                Console.WriteLine("(3) Edit transaction");
                Console.WriteLine("(4) Remove transaction");
                Console.WriteLine("(5) Show transactions");
                Console.WriteLine("(6) Save and Quit");
                Console.WriteLine("");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        getMethods.AddTransaction("Income"); //Send with "Income" to differentiate between Income and Expense
                        break;
                    case "2":
                        getMethods.AddTransaction("Expense"); //Send with "Expense" to differentiate between Income and Expense
                        break;
                    case "3":
                        getMethods.EditTransaction();
                        break;
                    case "4":
                        getMethods.RemoveTransaction();
                        break;
                    case "5":
                        getMethods.ShowTransactions();
                        break;
                    case "6":
                        getMethods.SaveData();
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Your data was saved. Thank you and bye!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ResetColor();
                        getMethods.Pause();
                        break;
                }
            }
        }
    }
}
