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
        private string namespacestr = "Application.";
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public void parse()
        {
            CodeGen.fname = "out.il";
            CodeGenFunctions.addInstr(".assembly Application {}");
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
                    try
                    {
                        valueType = Locals.locals_dict[Locals.getNameByIndex(val.asNumber())].type();
                    }
                    catch
                    {
                        valueType = FunctionsLocals.locals[FunctionsLocals.getNameByIndex(val.asNumber())].value.type();
                    }
                }
                skip();
                CodeGenFunctions.addInstr($"call void [mscorlib]System.Console::WriteLine({valueType.ToString().ToLower()})");
            }
            else if (get(ValueType.PROGRAM))
            {
                CodeGenFunctions.addInstr(@".class private auto ansi beforefieldinit Application.Program
       extends[mscorlib]System.Object
{");
            }
            else if (get(ValueType.NAMESPACE))
            {
                namespacestr += consume(ValueType.WORD).getText() + "::";
            }
            else if (look(ValueType.WORD) && lookN(1, ValueType.LPAREN))
            {
                List<local> funargs = new List<local>();
                string name = consume(ValueType.WORD).getText();
                function function = Functions.getFunctionByName(name);
                string funstr = $"call {function.type.ToString().ToLower()} {namespacestr + name}(";
                consume(ValueType.LPAREN);
                while (!get(ValueType.RPAREN))
                {
                    local local = new local();
                    ValueExpression val = (ValueExpression)expression();
                    Value evalueted = val.eval();
                    ValueType valueType = evalueted.type();
                    if (val.isConst)
                    {
                        try
                        {
                            valueType = Locals.locals_dict[Locals.getNameByIndex(evalueted.asNumber())].type();
                        }
                        catch
                        {
                            valueType = FunctionsLocals.locals[FunctionsLocals.getNameByIndex(evalueted.asNumber())].value.type();
                        }
                    }
                    local.type = valueType;
                    local.value = evalueted;
                    funargs.Add(local);
                    funstr += valueType.ToString().ToLower();
                    if (!get(ValueType.COMMA))
                    {
                        skip();
                        consume(ValueType.RPAREN);
                        break;
                    }
                    else
                    {
                        funstr += ",";
                    }
                }
                for (int i = 0; i < funargs.Count; i++)
                {
                    local temp = funargs[i];
                    temp.index = function.args[i].index;
                    temp.name = function.args[i].name;
                    temp.type = temp.value.type();
                    function.args[i] = temp;
                }
                CodeGenFunctions.addInstr(funstr+")");
                Console.WriteLine(tokens[pos].getText());
            }
            else if (get(ValueType.FUN))
            {
                List<local> args = new List<local>();
                ValueType type = getS().getType();
                string typeS = type.ToString().ToLower();
                string name = consume(ValueType.WORD).getText();
                int methodinstr = CodeGenFunctions.addInstr($".method public static {typeS} {name}(");
                consume(ValueType.LPAREN);
                while (!get(ValueType.RPAREN))
                {
                    local local = new local();
                    local.index = Locals.locals.Count;
                    local.type = tokens[pos].getType();
                    string typestr = tokens[pos].getText().ToLower();
                    local.value = new ObjectValue(0);
                    skip();
                    local.name = tokens[pos].getText();
                    skip();
                    FunctionsLocals.locals.Add(local.name, local);
                    args.Add(local);
                    int currinstr = CodeGenFunctions.changeInstr(methodinstr, $"{typestr} {local.name}");
                    if (!get(ValueType.COMMA))
                    {
                        consume(ValueType.RPAREN);
                        break;
                    }
                    else
                    {
                        CodeGenFunctions.changeInstr(currinstr, ",");
                    }
                }
                CodeGenFunctions.changeInstr(methodinstr, ") cil managed");
                function function = new function();
                function.args = args;
                function.name = name;
                function.type = type;
                Functions.functions.Add(function);
                consume(ValueType.BEGIN);
                CodeGenFunctions.addInstr("{");
                int stacksizeline = CodeGenFunctions.skipInstr(2);
                while (!get(ValueType.END))
                {
                    instruction();
                }
                CodeGenFunctions.addInstr("}");
                CodeGenFunctions.setInstr(stacksizeline, ".maxstack " + (Locals.stack_size+1));
                CodeGenFunctions.setInstr(stacksizeline + 1, ".locals init(");
                for (int i = 0; i < Locals.locals.Count; i++)
                {
                    local local = Locals.locals[i];
                    Console.WriteLine(local.name);
                    CodeGenFunctions.changeInstr(stacksizeline + 1, local.value.ToString());
                    if (i + 1 < Locals.locals.Count)
                    {
                        CodeGenFunctions.changeInstr(stacksizeline + 1, ",");
                    }
                }
                CodeGenFunctions.changeInstr(stacksizeline + 1, ")");
                if (CodeGenFunctions.getInstr(stacksizeline+1) == ".locals init()")
                {
                    CodeGenFunctions.setInstr(stacksizeline + 1, string.Empty);
                }
                Locals.stack_size = 0;
                Locals.locals = new List<local>();
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
                    CodeGenFunctions.changeInstr(stacksizeline + 1, local.value.ToString());
                    if (i + 1 < Locals.locals.Count)
                    {
                        CodeGenFunctions.changeInstr(stacksizeline + 1, ",");
                    }
                }
                CodeGenFunctions.changeInstr(stacksizeline+1, ")");
                Locals.stack_size = 0;
                Locals.locals = new List<local>();
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
                Locals.addLocal(Locals.locals.Count, value, name, ValueType.INT32);
                skip();
            }
            else if (look(ValueType.STRING) && lookN(1, ValueType.WORD) && lookN(2, ValueType.EQ))
            {
                consume(ValueType.STRING);
                string name = consume(ValueType.WORD).getText();
                consume(ValueType.EQ);
                StringValue value = (StringValue)expression().eval();
                CodeGenFunctions.addInstr("stloc." + Locals.locals.Count);
                Locals.addLocal(Locals.locals.Count, value, name, ValueType.STRING);
                skip();
            }
            else
            {
                Console.WriteLine(tokens[pos].getText() + " " + tokens[pos].getType());
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
            else if (get(ValueType.GETFARG))
            {
                string name = consume(ValueType.WORD).getText();
                pos--;
                if (FunctionsLocals.locals.ContainsKey(name))
                {
                    int index = FunctionsLocals.locals[name].index;
                    ValueExpression valueExpression = new ValueExpression(index);
                    valueExpression.isConst = true;
                    valueExpression.isWritable = false;
                    valueExpression.isNeedInstr = false;
                    CodeGenFunctions.addInstr("ldarg." + index);
                    return valueExpression;
                }
                else
                {
                    throw new Exception("Unknown expression");
                }
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

        private Token getS()
        {
            Token current = tokens[pos];
            skip();
            return current;
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
