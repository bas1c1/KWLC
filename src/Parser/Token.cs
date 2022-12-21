using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLC
{
    public class Token
    {
        private ValueType type;
        private string text;
        public Token(ValueType type, string text)
        {
            this.type = type;
            this.text = text;
        }

        public ValueType getType()
        {
            return type;
        }

        public void setType(ValueType type)
        {
            this.type = type;
        }

        public string getText()
        {
            return text;
        }

        public void setText(string text)
        {
            this.text = text;
        }
    }
}
