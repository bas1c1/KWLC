using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Classes
{
    public class IntValue : Value
    {
        private int obj;

        public IntValue(int obj)
        {
            this.obj = obj;
        }

        public char asChar()
        {
            return (char)obj;
        }

        public double asDouble()
        {
            return obj;
        }

        public int asNumber()
        {
            return obj;
        }

        public string asString()
        {
            return obj.ToString();
        }

        public override string ToString()
        {
            return "int32";
        }

        public ValueType type()
        {
            return ValueType.INT32;
        }
    }
}
