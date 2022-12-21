using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC.Classes
{
    public static class FunctionsLocals
    {
        public static Dictionary<string, local> locals = new Dictionary<string, local>();
        public static int getIndexByName(string name)
        {
            foreach (KeyValuePair<string, local> kvp in locals)
            {
                if (kvp.Value.name == name)
                {
                    return kvp.Value.index;
                }
            }
            return -1;
        }

        public static string getNameByIndex(int index)
        {
            foreach (KeyValuePair<string, local> kvp in locals)
            {
                if (kvp.Value.index == index)
                {
                    return kvp.Value.name;
                }
            }
            return string.Empty;
        }
    }
}
