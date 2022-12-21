using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Classes
{
    public struct local
    {
        public int index;
        public string name;
        public ValueType type;
        public Value value;
    }
    public static class Locals
    {
        public static List<local> locals = new List<local>();
        public static long stack_size = 0;
        public static Dictionary<string, Value> locals_dict = new Dictionary<string, Value>();

        public static void addLocal(int index, Value value, string name= "")
        {
            local local = new local();
            local.name = name;
            local.index = index;
            local.type = value.type();
            local.value = value;
            locals.Add(local);
            stack_size = locals.Count;
            if (name != "")
                locals_dict.Add(name, value);
        }

        public static int getIndexByName(string name)
        {
            foreach (local local in locals)
            {
                if (local.name == name)
                {
                    return local.index;
                }
            }
            return -1;
        }

        public static string getNameByIndex(int index)
        {
            foreach (local local in locals)
            {
                if (local.index == index)
                {
                    return local.name;
                }
            }
            return string.Empty;
        }
    }

    /*public class NonStaticLocals 
    {
        public List<local> locals = new List<local>();
        public long stack_size = 0;

        public NonStaticLocals()
        {
        }
    }*/
}
