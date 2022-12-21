using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Classes
{
    public class VoidValue : Value
    {

        public VoidValue()
        {
        }

        public char asChar()
        {
            return new char();
        }

        public double asDouble()
        {
            return new double();
        }

        public int asNumber()
        {
            return new int();
        }

        public string asString()
        {
            return new string("");
        }

        public override string ToString()
        {
            return "void";
        }

        public ValueType type()
        {
            return ValueType.VOID;
        }
    }
}
