using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC
{
    public class Lexer
    {
        private int pointer = 0;
        private string code = string.Empty;
        private List<Token> tokens = new List<Token>();
        private string opstr = ">;-+/*,=<():";
        private const string EOF = ""; 

        private static Dictionary<string, ValueType> opdict = new Dictionary<string, ValueType>()
        {
            { "->", ValueType.DASHGT },
            { ",", ValueType.COMMA },
            { ";", ValueType.SEMCOL },
            { ">>", ValueType.GTGT },
            { "=", ValueType.EQ },
            { "(", ValueType.LPAREN },
            { ")", ValueType.RPAREN },
            { ":", ValueType.DDOT }
        };

        public Lexer(string code)
        {
            this.code = code;
        }

        public List<Token> lex()
        {
            for (; pointer < code.Length; pointer++)
            {
                if (check())
                {
                    break;
                }
                if (char.IsLetter(code[pointer]) || code[pointer] == '_')
                {
                    lexWord();
                }
                else if (char.IsDigit(code[pointer]))
                {
                    lexNum();
                }
                else if (code[pointer] == '\"')
                {
                    lexString();
                }
                else if (opstr.IndexOf(code[pointer]) != -1)
                {
                    lexOperator();
                }
            }
            tokens.Add(new Token(ValueType.EOF, EOF));
            return tokens;
        }

        private string lexWord()
        {
            string buffer = string.Empty;
            for (; pointer < code.Length && (char.IsLetterOrDigit(code[pointer]) || code[pointer] == '_'); pointer++)
            {
                buffer += code[pointer];
            }
            pointer--;
            switch (buffer)
            {
                case "namespace":
                    {
                        tokens.Add(new Token(ValueType.NAMESPACE, buffer));
                        return buffer;
                    }
                case "void":
                    {
                        tokens.Add(new Token(ValueType.VOID, buffer));
                        return buffer;
                    }
                case "int32":
                    {
                        tokens.Add(new Token(ValueType.INT32, buffer));
                        return buffer;
                    }
                case "string":
                    {
                        tokens.Add(new Token(ValueType.STRING, buffer));
                        return buffer;
                    }
                case "object":
                    {
                        tokens.Add(new Token(ValueType.OBJECT, buffer));
                        return buffer;
                    }
                case "print":
                    {
                        tokens.Add(new Token(ValueType.PRINT, buffer));
                        return buffer;
                    }
                case "return":
                    {
                        tokens.Add(new Token(ValueType.RETURN, buffer));
                        return buffer;
                    }
                case "__program__":
                    {
                        tokens.Add(new Token(ValueType.PROGRAM, buffer));
                        return buffer;
                    }
                case "__main__":
                    {
                        tokens.Add(new Token(ValueType.MAIN, buffer));
                        return buffer;
                    }
                case "gfa":
                    {
                        tokens.Add(new Token(ValueType.GETFARG, buffer));
                        return buffer;
                    }
                case "fun":
                    {
                        tokens.Add(new Token(ValueType.FUN, buffer));
                        return buffer;
                    }
                case "begin":
                    {
                        tokens.Add(new Token(ValueType.BEGIN, buffer));
                        return buffer;
                    }
                case "end":
                    {
                        tokens.Add(new Token(ValueType.END, buffer));
                        return buffer;
                    }
            }
            tokens.Add(new Token(ValueType.WORD, buffer));
            return buffer;
        }

        private double lexNum()
        {
            string buffer = string.Empty;
            for (; pointer < code.Length && char.IsDigit(code[pointer]); pointer++)
            {
                buffer += code[pointer];
            }
            pointer--;
            double num = double.Parse(buffer);
            tokens.Add(new Token(ValueType.INT32, buffer));
            return num;
        }

        private string lexString()
        {

            pointer++;
            if (check())
            {
                return null;
            }
            string buffer = string.Empty;
            
            for (; pointer < code.Length && code[pointer] != '\"'; pointer++)
            {
                buffer += code[pointer];
            }
            tokens.Add(new Token(ValueType.STRING, buffer));
            return buffer;
        }

        private string lexOperator()
        {
            string buffer = string.Empty;
            for (; pointer < code.Length; pointer++)
            {
                string sub = buffer + code[pointer];
                if (opdict.ContainsKey(sub))
                {
                    tokens.Add(new Token(opdict[sub], sub));
                    return buffer;
                }
                buffer += code[pointer];
            }
            return buffer;
        }

        private bool check()
        {
            return pointer >= code.Length;
        }
    }
}
