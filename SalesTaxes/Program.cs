﻿// Top Level statement used to load the sales input
using SalesTaxes.Core.Services;

var inputStr = new List<string>();

// writing basic instructions for the user
Console.WriteLine("Please enter row by row of the sale");
Console.WriteLine("");
Console.WriteLine("Valid format '[quantity] [nameOfTheProduct] at [price(00.00)]'");
Console.WriteLine("");
Console.WriteLine("");


// loading the sales rows
while (true)
{
    Console.WriteLine("Enter a valid sale row, [Type 'print' to print the receipt, 'exit' to leave]");
    var aux = Console.ReadLine();

    if (aux?.ToLower() == "exit")
        Environment.Exit(0);

    if (aux != null && aux.ToLower() != "print")
    {
        if (CoreService.ValidateString(aux))
        {
            Console.WriteLine("Enter the food category");
            Console.WriteLine("1 - Food, 2 - Medical, 3 - Books or enter empty answer for none");

            var type = Console.ReadLine();

            var imported = string.Empty;
            while (imported.ToLower() != "y" && imported.ToLower() != "n")
            {
                Console.WriteLine("Is the product imported? (Y/N)");

                imported = Console.ReadLine();
            }
            inputStr.Add($"{aux}|{type}|{imported}");
        }
        else
        {
            Console.WriteLine($"Invalid entered input:{aux} -> Valid format '[quantity] [nameOfTheProduct] at [price]'");
        }
    }
    else
        break;
}

// Clearing console to print the receipt
Console.Clear();

Console.WriteLine("----------Receipt----------");
Console.WriteLine("");

// Running the Core service to do all the work
CoreService.Run(inputStr.ToArray());