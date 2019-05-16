using System.Collections.Generic;

namespace SchemeSharp
{
    public class Context
    {
        public Context Parent;
        public Dictionary<Symbol, Value> LocalValues;

        public Context(Context parent)
        {
            Parent = parent;
            LocalValues = new Dictionary<Symbol, Value>();
        }

        public void DefineValue(Symbol symbol, Value value)
        {
            LocalValues.Add(symbol, value);
        }

        public bool SetValue(Symbol symbol, Value value)
        {
            if (LocalValues.ContainsKey(symbol))
            {
                LocalValues[symbol] = value;
                return true;
            }
            else if (Parent != null)
            {
                return Parent.SetValue(symbol, value);
            }
            else
            {
                return false;
            }
        }

        public Value GetValue(Symbol symbol)
        {
            if (LocalValues.ContainsKey(symbol))
            {
                return LocalValues[symbol];
            }
            else
            {
                return null;
            }
        }

        public Context Fork()
        {
            return new Context(this);
        }
    }
}