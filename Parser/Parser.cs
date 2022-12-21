using KWLC.Classes;
using KWLC.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC
{
    public class Parser
    {
        private int pos = 0;
        private List<Token> tokens = new List<Token>();
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public void parse()
        {
            CodeGen.fname = "out.il";
            CodeGenFunctions.addInstr(".assembly HelloWorld {}");
            while(!get(ValueType.EOF))
            {
                instruction();
            }
            CodeGen.Generate();
        }

        public void instruction()
        {
            
            if (get(ValueType.PRINT))
            {
                consume(ValueType.DASHGT);
                Expression value = expression();
                if (value is ValueExpression)
                {
                    if (((ValueExpression)value).isConst)
                    {
                        ((ValueExpression)value).isWritable = false;
                    }
                }
                Value val = value.eval();
                ValueType valueType = val.type();
                if (((ValueExpression)value).isConst)
                {
                    valueType = Locals.locals_dict[Locals.getNameByIndex(val.asNumber())].type();
                }
                skip();
                switch (valueType) 
                {
                    case ValueType.STRING:
                        {
                            CodeGenFunctions.addInstr("call void [mscorlib]System.Console::WriteLine(string)");
                            break;
                        }
                    case ValueType.INT32:
                        {
                            CodeGenFunctions.addInstr("call void [mscorlib]System.Console::WriteLine(int32)");
                            break;
                        }
                    case ValueType.OBJ:
                        {
                            CodeGenFunctions.addInstr("call void [mscorlib]System.Console::WriteLine(object)");
                            break;
                        }
                }
            }
            else if (get(ValueType.MAIN))
            {
                CodeGenFunctions.addInstr(".method public static void Main() cil managed");
                CodeGenFunctions.addInstr("{");
                CodeGenFunctions.addInstr(".entrypoint");
                int stacksizeline = CodeGenFunctions.skipInstr(2);
                while (!get(ValueType.END))
                {
                    instruction();
                }
                CodeGenFunctions.addInstr("}");
                CodeGenFunctions.setInstr(stacksizeline, ".maxstack " + Locals.stack_size);
                CodeGenFunctions.setInstr(stacksizeline+1, ".locals init(");
                for (int i = 0; i < Locals.locals.Count; i++)
                {
                    local local = Locals.locals[i];
                    Console.WriteLine(local.name);
                    switch (local.type)
                    {
                        case ValueType.STRING:
                            {
                                CodeGenFunctions.changeInstr(stacksizeline+1,"string");
                                if (i+1 < Locals.locals.Count)
                                {
                                    CodeGenFunctions.changeInstr(stacksizeline+1, ",");
                                }
                                break;
                            }
                        case ValueType.INT32:
                            {
                                CodeGenFunctions.changeInstr(stacksizeline + 1, "int32");
                                if (i + 1 < Locals.locals.Count)
                                {
                                    CodeGenFunctions.changeInstr(stacksizeline + 1, ",");
                                }
                                break;
                            }
                        case ValueType.OBJ:
                            {
                                CodeGenFunctions.changeInstr(stacksizeline + 1, "object");
                                if (i + 1 < Locals.locals.Count)
                                {
                                    CodeGenFunctions.changeInstr(stacksizeline + 1, ",");
                                }
                                break;
                            }
                    }
                }
                CodeGenFunctions.changeInstr(stacksizeline+1, ")");
                Locals.stack_size = 0;
            }
            else if (get(ValueType.BEGIN))
            {
                CodeGenFunctions.addInstr("{");
            }
            else if (get(ValueType.END))
            {
                CodeGenFunctions.addInstr("}");
            }
            else if (get(ValueType.RETURN))
            {
                CodeGenFunctions.addInstr("ret");
            }
            else if (get(ValueType.EOF))
            {
                return;
            }
            else if (look(ValueType.INT32) && lookN(1, ValueType.WORD) && lookN(2, ValueType.EQ))
            {
                consume(ValueType.INT32);
                string name = consume(ValueType.WORD).getText();
                consume(ValueType.EQ);
                IntValue value = (IntValue)expression().eval();
                CodeGenFunctions.addInstr("stloc." + Locals.locals.Count);
                Locals.addLocal(Locals.locals.Count, value, name);
                skip();
            }
            else if (look(ValueType.STRING) && lookN(1, ValueType.WORD) && lookN(2, ValueType.EQ))
            {
                consume(ValueType.STRING);
                string name = consume(ValueType.WORD).getText();
                consume(ValueType.EQ);
                StringValue value = (StringValue)expression().eval();
                CodeGenFunctions.addInstr("stloc." + Locals.locals.Count);
                Locals.addLocal(Locals.locals.Count, value, name);
                skip();
            }
            else
            {
                throw new Exception("Unknown statement");
            }
        }

        public Expression expression()
        {
            if (look(ValueType.STRING))
            {
                ValueExpression valueExpression = new ValueExpression(tokens[pos].getText());
                valueExpression.isWritable = false;
                return valueExpression;
            }
            else if (look(ValueType.WORD))
            {
                string name = consume(ValueType.WORD).getText();
                pos--;
                if (Locals.locals_dict.ContainsKey(name))
                {
                    int index = Locals.getIndexByName(name);
                    ValueExpression valueExpression = new ValueExpression(index);
                    valueExpression.isConst = true;
                    valueExpression.isWritable = false;
                    valueExpression.isNeedInstr = false;
                    CodeGenFunctions.addInstr("ldloc." + index);
                    return valueExpression;
                }
                else
                {
                    throw new Exception("Unknown expression");
                }
            }
            else if (look(ValueType.INT32))
            {
                ValueExpression valueExpression = new ValueExpression(int.Parse(tokens[pos].getText()));
                valueExpression.isWritable = false;
                return valueExpression;
            }
            
            throw new Exception("Unknown expression");
        }

        private Token getcurr()
        {
            return tokens[pos];
        }

        private void skip()
        {
            pos++;
        }

        private void skipMany(int n)
        {
            pos += n;
        }

        private bool lookN(int index, ValueType type)
        {
            return tokens[pos+index].getType() == type;
        }

        private bool look(ValueType type)
        {
            return tokens[pos].getType() == type;
        }

        private bool get(ValueType type)
        {
            bool output = tokens[pos].getType() == type;
            if (tokens[pos].getType() != type) return false;
            skip();
            return true;
        }

        private Token consume(ValueType type)
        {
            Token current = tokens[pos];
            if (type != current.getType()) throw new Exception("Token " + current.getType() + " doesn't match " + type);
            skip();
            return current;
        }
    }
}
