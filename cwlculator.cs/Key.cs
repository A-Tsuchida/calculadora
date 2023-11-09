using Calculator.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator;
public record Key(string Symbol, KeyType Type) : IKey
{
    private KeyHandler? keyHandler;

    public event KeyHandler KeyHandlerEvent
    {
        add    => keyHandler = (KeyHandler?)Delegate.Combine(keyHandler, value);
        remove => keyHandler = (KeyHandler?)Delegate.Remove(keyHandler, value);
    }

    public void Press() => keyHandler?.Invoke(Type);
}
