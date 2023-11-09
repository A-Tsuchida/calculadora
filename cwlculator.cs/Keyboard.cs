using Calculator.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator;
public class Keyboard : IKeyboard
{
    private IEnumerable<IKey> keys = [];

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

    public void Add(IKey key)
    {
        if (keys.Any(keys => keys.Symbol == key.Symbol))
            throw new ArgumentException($"There is already a key with the symbol '{key.Symbol}' in {nameof(Keyboard)}.", nameof(key));

        keys = keys.Append(key);
    }

    public void Remove(IKey key)
    {
        keys = keys.Where(k => !k.Equals(key));
        key.KeyHandlerEvent -= keyHandler;
    }

    public IKey GetKey(string symbol) => keys.First(key => key.Symbol == symbol);
}
