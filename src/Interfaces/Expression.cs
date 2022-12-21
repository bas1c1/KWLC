using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC
{
    public interface Expression
    {
        public Value eval();
    }
}
