using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;

namespace KWLC
{
#pragma warning disable CA1416
    public static class CodeGen
    {
        public static string fname;
        public static List<string> lines = new List<string>();
        public static int linecount = 0;

        public static void Generate()
        {
            File.WriteAllLines(fname, lines);
            Process.Start("ilassembly\\ilasm.exe", fname);
        }
    }
}
