using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BudgetTrackerApp
{
    public class Program
    {
        static List<Transaction> transactions = LoadTransactions();
        const string filePath = "transactions.json";

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== Budget Tracker ===");
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View Summary");
                Console.WriteLine("3. Exit");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddTransaction(); break;
                    case "2": ViewSummary(); break;
                    case "3": SaveTransactions(); return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }

        }

        public static void AddTransaction()
        {
            // Date input with validation
            DateTime date;
            while (true)
            {
                Console.Write("Date (yyyy-mm-dd): ");
                string dateInput = Console.ReadLine();
                if (DateTime.TryParse(dateInput, out date))
                    break;
                Console.WriteLine("❌ Invalid date format. Please try again.");
            }

            // Description
            string description = ReadRequiredString("Description: ");

            // Amount input with validation
            decimal amount;
            while (true)
            {
                Console.Write("Amount: ");
                string amountInput = Console.ReadLine();
                if (decimal.TryParse(amountInput, out amount))
                    break;
                Console.WriteLine("❌ Invalid amount. Please enter a number.");
            }

            // Category
            string category = ReadRequiredString("Category: ");

            // Type (Income or Expense)
            string type;
            while (true)
            {
                Console.Write("Type (Income/Expense): ");
                type = Console.ReadLine()?.Trim().ToLower() ?? "";
                if (type == "income" || type == "expense")
                    break;
                Console.WriteLine("❌ Please enter 'Income' or 'Expense'.");
            }

            // Add to list
            transactions.Add(new Transaction
            {
                Date = date,
                Description = description,
                Amount = amount,
                Category = category,
                Type = type
            });

            Console.WriteLine("✅ Transaction added successfully!");
        }
        public static string ReadRequiredString(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));

            return input.Trim();
        }



        public static void ViewSummary()
        {
            var income = transactions.Where(t => t.Type.ToLower() == "income").Sum(t => t.Amount);
            var expense = transactions.Where(t => t.Type.ToLower() == "expense").Sum(t => t.Amount);
            var balance = income - expense;

            Console.WriteLine($"\nTotal Income: ₹{income}");
            Console.WriteLine($"Total Expense: ₹{expense}");
            Console.WriteLine($"Balance: ₹{balance}");
        }

        public static void SaveTransactions()
        {
            var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine("💾 Transactions saved.");
        }

        public static List<Transaction> LoadTransactions()
        {
            if (!File.Exists(filePath)) return new List<Transaction>();
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }
        public class Transaction
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public string Category { get; set; }
            public string Type { get; set; }
        }
    }
}
