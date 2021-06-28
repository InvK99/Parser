using System;
using System.IO;
using System.Windows.Forms;

namespace Parser
{
    class Program
    {
        private static Parser parser = new Parser();

        public static void Main(string[] args)
        {
            do
            {
                try
                {
                    RelaodGrammerFiles();
                    Console.WriteLine(@"Select an option:
    1. Toggle Verbose mode
    2. Parse an input text
    3. Parse a text file
    4. View a parse tree
    5. Reload grammer file(s)
    
    Type 'exit' to exit the application");

                    string input = Console.ReadLine();

                    if (input.ToLower() == "exit")
                    {
                        return;
                    }
                    else if (input == "1")
                    {
                        ToggleVerboseMode();
                        Console.Write("Press any key to view menu");
                        Console.ReadKey();
                    }
                    else if (input == "2")
                    {
                        ParseText();
                        Console.Write("Press any key to view menu");
                        Console.ReadKey();
                    }
                    else if (input == "3")
                    {
                        ParseFile();
                        Console.Write("Press any key to view menu");
                        Console.ReadKey();
                    }
                    else if (input == "4")
                    {
                        ShowParseTree();
                        //Console.Write("Press any key to view menu");
                        //Console.ReadKey();
                    }
                    else if (input == "5")
                    {
                        RelaodGrammerFiles();
                        //Console.Write("Press any key to view menu");
                        //Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.ReadKey();
                }
            }
            while (true);
        }

        private static void ShowParseTree()
        {
            frmParseTree frm = new frmParseTree();
            frm.Parser = parser;
            frm.ShowDialog();
        }

        public static void ToggleVerboseMode()
        {
            parser.Verbose = !parser.Verbose;
            Console.WriteLine("Verbose mode " + (parser.Verbose ? "on" : "off"));
        }

        public static void RelaodGrammerFiles()
        {
            parser.ClearGrammer();
            string[] files = Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath));
            foreach (string str in files)
            {
                if (str.ToLower().EndsWith(".grm"))
                    parser.AddGrammer(File.ReadAllText(str));
            }
        }

        public static void ParseText()
        {
            Console.Write("Type in a text to parse: ");
            string text = Console.ReadLine();
            if (parser.Parse(text))
            {
                Console.WriteLine("Success '" + text + "'");
            }
            else
            {
                Console.WriteLine("Failed!!! '" + text + "'");
                Console.WriteLine("^".PadLeft(parser.MaximumPoint + 12));
            }
            if (parser.Verbose)
                Console.WriteLine(parser.Branch.Print(0));
            File.WriteAllText("parse_tree.xml", parser.Branch.Print(0));
        }

        private static void ParseFile()
        {
            Console.Write("Enter file path to parse: ");
            string text = File.ReadAllText(Console.ReadLine());
            if (parser.Parse(text))
            {
                Console.WriteLine("Success '" + text + "'");
            }
            else
            {
                Console.WriteLine("Failed!!! '" + text + "'");
                Console.WriteLine("^".PadLeft(parser.MaximumPoint + 12));
            }
            if (parser.Verbose)
                Console.WriteLine(parser.Branch.Print(0));
            File.WriteAllText("parse_tree.xml", parser.Branch.Print(0));
        }
    }
}
