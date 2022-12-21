using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Classes
{
    public class ObjectValue : Value
    {
        private object obj;

        public ObjectValue(object obj)
        {
            this.obj = obj;
        }

        public char asChar()
        {
            return (char)obj;
        }

        public double asDouble()
        {
            return (double)obj;
        }

        public int asNumber()
        {
            return (int)obj;
        }

        public string asString()
        {
            return obj.ToString();
        }

        public ValueType type()
        {
            return ValueType.OBJ;
        }
    }
}
