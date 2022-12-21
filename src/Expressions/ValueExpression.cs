using KWLC.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Expressions
{
    public class ValueExpression : Expression
    {
        public bool isConst = false;
        public bool isNeedInstr = true;
        public bool isWritable = true;
        private Value value;
        public ValueExpression(Value value)
        {
            this.value = value;
        }

        public ValueExpression(double value)
        {
            this.value = new IntValue((int)value);
        }

        public ValueExpression(string value)
        {
            this.value = new StringValue(value);
        }

        public Value eval()
        {
            if (isNeedInstr)
            {
                switch (value.type())
                {
                    case ValueType.INT32:
                        {
                            CodeGenFunctions.addInstr("ldc.i4 " + value.asNumber().ToString());
                            break;
                        }
                    case ValueType.STRING:
                        {
                            CodeGenFunctions.addInstr("ldstr \"" + value.asString() + "\"");
                            break;
                        }
                }
            }
            if (isWritable)
            {
                Locals.addLocal(Locals.locals.Count, value,valueType:value.type());
                Locals.stack_size++;
            }
            return value;
        }
    }
}
