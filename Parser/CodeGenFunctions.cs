using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC
{
    public static class CodeGenFunctions
    {
        public static ulong lines = 0;

        public static void addInstr(string line)
        {
            CodeGen.lines.Add(line);
            CodeGen.linecount += 1;
        }

        public static void setInstr(int index, string line)
        {
            CodeGen.lines[index] = line;
        }

        public static string getInstr(int index)
        {
            return CodeGen.lines[index];
        }

        public static void changeInstr(int index, string line)
        {
            CodeGen.lines[index] += line;
        }

        public static int skipInstr(int count)
        {
            int temp = CodeGen.linecount;
            for (int i = 0; i <= count; i++) {
                CodeGen.lines.Add(string.Empty);
                CodeGen.linecount += 1;
            }
            return temp;
        }
    }
}
