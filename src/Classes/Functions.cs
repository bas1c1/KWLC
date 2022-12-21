using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Classes
{
    public struct function
    {
        public List<local> args;
        public string name;
        public ValueType type;
    }
    public static class Functions
    {
        public static List<function> functions = new List<function>();
        public static function getFunctionByName(string name)
        {
            foreach (function local in functions)
            {
                if (local.name == name)
                {
                    return local;
                }
            }
            return new function();
        }
    }
}
