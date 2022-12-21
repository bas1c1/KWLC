using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Classes
{
    public class StringValue : Value
    {
        private string obj;

        public StringValue(string obj)
        {
            this.obj = obj;
        }

        public char asChar()
        {
            return char.Parse(obj);
        }

        public double asDouble()
        {
            return double.Parse(obj);
        }

        public int asNumber()
        {
            return int.Parse(obj);
        }

        public string asString()
        {
            return obj;
        }

        public ValueType type()
        {
            return ValueType.STRING;
        }
    }
}
