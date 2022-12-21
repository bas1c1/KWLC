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
        OBJECT,
        VOID,

        EQ,
        COMMA,
        LPAREN,
        RPAREN,
        DASHGT,
        DOT,
        DDOT,
        SEMCOL,
        GTGT,

        PRINT,
        EXEC,
        GETFARG,
        RETURN,
        NAMESPACE,

        BEGIN,
        WORD,
        MAIN,
        PROGRAM,
        FUN,
        END,

        EOF,
    }
}
