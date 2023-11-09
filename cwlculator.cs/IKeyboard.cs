using Calculator.DataTypes;

namespace Calculator;
public interface IKeyboard
{
    event KeyHandler KeyHandlerEvent;

    /// <exception cref="ArgumentException">Key with <see cref="IKey.Symbol"/> already exists in the <see cref="IKeyboard"/></exception>
    void Add(IKey key);
    /// <exception cref="InvalidOperationException"><see cref="IKey"/> not found</exception>
    IKey GetKey(string symbol);
    void Remove(IKey key);
}