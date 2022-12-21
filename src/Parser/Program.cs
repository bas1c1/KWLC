using System;
using System.IO;

namespace KWLC
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lexer = new Lexer(File.ReadAllText(args[0]));
            foreach (Token token in lexer.lex())
            {
                Console.WriteLine(token.getText() + " - " + token.getType());
            }
            Parser parser = new Parser(lexer.lex());
            parser.parse();
            Console.ReadKey();
            Console.Clear();
        }
    }
}
