using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC
{
    public enum ValueType
    {
        STRING,
        INT32,
        OBJ,

        EQ,
        COMMA,
        LPAREN,
        RPAREN,
        DASHGT,
        DOT,
        SEMCOL,
        GTGT,

        PRINT,
        EXEC,
        RETURN,

        BEGIN,
        WORD,
        MAIN,
        FUN,
        END,

        EOF,
    }
}
