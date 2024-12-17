using System;
using System.Globalization;
using System.Transactions;

namespace Money_Tracking
{

    public class TransactionHandler
    {
        //The txt. document for saving the data and a list object
        static string dataFilePath = "transactions.txt";
        static List<Transaction> transactions = new List<Transaction>();

        /*AddTransactions has a string type as input to differentiate between Income and Expense. Then it takes user input for different values 
         * and adds these to the list object as a new Transaction object */
        public void AddTransaction(string type)
        {

            bool validDate; //Bool to later help with validatin the user input date

            Console.Write($"Enter {type} name: ");
            string title = Console.ReadLine();

            Console.Write($"Enter {type} amount: ");
            bool number = decimal.TryParse(Console.ReadLine(), out decimal amount);
            while (number == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid amount. Please try again.");
                Console.ResetColor();
                number = decimal.TryParse(Console.ReadLine(), out decimal amountTwo);
                amount = amountTwo;
            }

            Console.Write($"Enter date of {type} (yyyy-MM-dd): ");
            validDate = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            while (validDate == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid date format. Please try again.");
                Console.ResetColor();
                validDate = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTwo);
                date = dateTwo;
            }

            if (type == "Expense") //If type is defined as "Expense" then use negative values for it
            {
                amount = -Math.Abs(amount);
            }

            transactions.Add(new Transaction { Type = type, Title = title, Amount = amount, Date = date }); //Add all the data to the new object
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{type} with name {title} added successfully!");
            Console.ResetColor();

            Pause(); //A method that waits for user key input and "Pauses" the program before continuing

        }

        public void EditTransaction() // EditTransaction is so the user can edit different transactions
        {

            ShowTransactions(); //First call ShowTransactions to show the transactions list to user
            bool keepAsking = true;
            bool validDate;

            while (keepAsking)
            {

                Console.Write("Enter the name of the transaction to edit (case sensitive): ");
                string typeName = Console.ReadLine();
                if (transactions.Any(s => s.Title == typeName)) //Check if transaction contains anything with the user input "Name"
                {
                    var transaction = transactions.Find(x => x.Title == typeName); //Used to change the "Name" later in code

                    Console.Write($"Enter new name (current is {transaction.Title}): ");
                    string newTitle = Console.ReadLine();
                    transaction.Title = newTitle; //Change the "Name"

                    Console.Write($"Enter new amount (current is {transaction.Amount}): ");
                    bool number = decimal.TryParse(Console.ReadLine(), out decimal newAmount);
                    while (number == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid amount. Please try again.");
                        Console.ResetColor();
                        number = decimal.TryParse(Console.ReadLine(), out decimal newAmountTwo);
                        newAmount = newAmountTwo;
                    }

                    if (number == true) //If its a correct number through tryparse (earlier in the code) then change the "Amount"
                    {
                        transaction.Amount = transaction.Type == "Expense" ? -Math.Abs(newAmount) : Math.Abs(newAmount);
                    }

                    //Check if date is entered correct, if its not ask user to try again
                    Console.Write($"Enter new date (current is {transaction.Date:yyyy-MM-dd}): ");
                    validDate = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate);
                    while (validDate == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid date format. Please try again.");
                        Console.ResetColor();
                        validDate = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDateTwo);
                        newDate = newDateTwo;
                    }
                    if (validDate == true)
                    {
                        transaction.Date = newDate;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Transaction updated successfully!");
                    Console.ResetColor();
                    keepAsking = false; //All user input was ok so set keppAsking to false and exit the while loop

                }
                else
                {
                    //If user input "Name" was incorrect set keepAsking to true and continue the while loop and ask for new user input for "Name"
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no transaction with that name. Please try again.");
                    Console.ResetColor();
                    keepAsking = true;
                }

            }

            Pause(); //After while loop paus again for user key input before continuing
        }

        /*RemoveTransactions looks for the user input "Name" for the item to remove, if it finds item with same name as user input it removes
          it, if not it asks for a new user input with the help of while loop*/
        public void RemoveTransaction()
        {

            ShowTransactions();
            bool keepAsking = true;
            while (keepAsking)
            {
                Console.WriteLine("Enter the name of the transaction to remove (case sensitive): ");
                string typeName = Console.ReadLine();
                if (transactions.Any(s => s.Title == typeName))
                {
                    transactions.RemoveAll(x => x.Title == typeName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Transaction removed successfully!");
                    Console.ResetColor();
                    keepAsking = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no transaction with that name. Please try again.");
                    Console.ResetColor();
                    keepAsking = true;
                }

            }

            Pause(); //After the while loop paus for user key input before continuing
        }

        /*ShowTransactions shows all transactions to user and can filter between "Income" and "Expense" and sort the table*/
        public void ShowTransactions()
        {

            Console.Clear();
            Console.WriteLine("Pick an option for which transactions to display:");
            Console.WriteLine("");
            Console.WriteLine("(1) All transactions");
            Console.WriteLine("(2) Incomes only");
            Console.WriteLine("(3) Expenses only");

            //Code to, with help of IEnumerable and switching, filter between "Income" and "Expense"
            string filterOption = Console.ReadLine();
            IEnumerable<Transaction> filteredTransactions = filterOption switch
            {
                "2" => transactions.Where(t => t.Type == "Income"),
                "3" => transactions.Where(t => t.Type == "Expense"),
                _ => transactions
            };

            Console.WriteLine("");
            Console.WriteLine("Pick an option on how to sort the transactions.");
            Console.WriteLine("");
            Console.WriteLine("In ascending order:");
            Console.WriteLine("(1) By name");
            Console.WriteLine("(2) By amount");
            Console.WriteLine("(3) By date");
            Console.WriteLine("");
            Console.WriteLine("In descending order:");
            Console.WriteLine("(4) By name");
            Console.WriteLine("(5) By amount");
            Console.WriteLine("(6) By date");

            //With help of filteredTransactions and switch sort the table by the users choosen input
            string sortOption = Console.ReadLine();
            filteredTransactions = sortOption switch
            {
                "1" => filteredTransactions.OrderBy(t => t.Title),
                "2" => filteredTransactions.OrderBy(t => Math.Abs(t.Amount)),
                "3" => filteredTransactions.OrderBy(t => t.Date),
                "4" => filteredTransactions.OrderByDescending(t => t.Title),
                "5" => filteredTransactions.OrderByDescending(t => Math.Abs(t.Amount)),
                "6" => filteredTransactions.OrderByDescending(t => t.Date),
                _ => filteredTransactions
            };

            //Code to print the table for the user to see
            Console.Clear();
            Console.WriteLine("Transactions:");
            Console.WriteLine("   | Type    | Name                  | Amount       | Date       ");
            Console.WriteLine("-----------------------------------------------------------------");

            int id = 1;
            foreach (var t in filteredTransactions)
            {
                Console.WriteLine($"{id++,2} | {t.Type,-7} | {t.Title,-21} | {t.Amount,12:C} | {t.Date:yyyy-MM-dd}");
            }

            if (filteredTransactions.Any() == false)
            {
                Console.WriteLine("There are no transactions to display.");
            }

            Pause(); //Again paus so the program doesnt continue and the user can see the table

        }

        /*SavedData saves the user input to the txt. document, it splits the text in parts with "|" so that the text will be easy to interpreted
         when later loaded back into program*/
        public void SaveData()
        {
            try //Use try catch to catch any error when saving
            {
                using (StreamWriter writer = new StreamWriter(dataFilePath))
                {
                    foreach (var transaction in transactions)
                    {
                        writer.WriteLine($"{transaction.Type}|{transaction.Title}|{transaction.Amount}|{transaction.Date:yyyy-MM-dd}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public decimal CalculateTotal() //CalulateTotal is just used to show the total amount of money on the start-screen
        {
            return transactions.Sum(t => t.Amount);
        }

        /*LoadData loads the txt. file back into the program when starting. It splits the text at "|" to split the different values
         * and then put them in their right place in an new Transaction class object wich is placed in the list*/
        public void LoadData()
        {
            try //Try catch to check for any error loading data
            {
                if (File.Exists(dataFilePath))
                {
                    foreach (var line in File.ReadAllLines(dataFilePath))
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 4 &&
                            DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) &&
                            decimal.TryParse(parts[2], out decimal amount))
                        {
                            transactions.Add(new Transaction
                            {
                                Type = parts[0],
                                Title = parts[1],
                                Amount = amount,
                                Date = date
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        public void Pause() //A method that waits for user key input and therefore "pauses" the program when used so the program have time to show the user its text. 
        {
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.WriteLine("");
        }
    }

}
