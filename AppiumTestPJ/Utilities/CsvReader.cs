using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace AppiumTestExample.Utilities
{
    public static class CsvReader
    {
        public static List<Dictionary<string, string>> ReadAccountsFromCsv(string csvFilePath)
        {
            var accounts = new List<Dictionary<string, string>>();
            try
            {
                Console.WriteLine($"Reading CSV file from: {csvFilePath}");
                var lines = File.ReadAllLines(csvFilePath);
                if (lines.Length <= 1)
                {
                    Console.WriteLine("CSV file is empty or only contains header.");
                    return accounts;
                }

                for (int i = 1; i < lines.Length; i++)
                {
                    var columns = lines[i].Split(',');
                    if (columns.Length == 4)
                    {
                        accounts.Add(new Dictionary<string, string>
                        {
                            { "email", columns[0].Trim() },
                            { "username", columns[1].Trim() },
                            { "password", columns[2].Trim() },
                            { "confrimpassword", columns[3].Trim() }
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Skipping invalid CSV row {i + 1}: {lines[i]}");
                    }
                }
                Console.WriteLine($"Loaded {accounts.Count} accounts from CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
                Assert.Fail($"Failed to read CSV file: {ex.Message}");
            }
            return accounts;
        }
    }
}