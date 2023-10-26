using Calculator.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator;
public class Keyboard
{
    private IEnumerable<Key> keys = [];

    private KeyHandler? keyHandler;

    public event KeyHandler KeyHandlerEvent
    {
        add
        {
            keys.AsParallel().ForAll(key => key.KeyHandlerEvent -= keyHandler);
            keyHandler = (KeyHandler?)Delegate.Combine(keyHandler, value);
            keys.AsParallel().ForAll(key => key.KeyHandlerEvent += keyHandler);
        }

        remove
        {
            keys.AsParallel().ForAll(key => key.KeyHandlerEvent -= keyHandler);
            keyHandler = (KeyHandler?)Delegate.Remove(keyHandler, value);
            keys.AsParallel().ForAll(key => key.KeyHandlerEvent += keyHandler);
        }
    }

    public void Add(Key key)
    {
        keys = keys.Append(key);
    }

    public void Remove(Key key)
    {
        keys = keys.Where(k => k != key);
        key.KeyHandlerEvent -= keyHandler;
    }

    public Key? GetKey(string symbol) => keys.FirstOrDefault(key => key.Symbol == symbol);
}
