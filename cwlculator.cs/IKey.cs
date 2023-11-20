using Calculator.DataTypes;

namespace Calculator;
public interface IKey
{
    string Symbol { get; init; }
    OpCode Type { get; init; }

    event KeyHandler KeyHandlerEvent;

    bool Equals(Key? other);
    bool Equals(object? obj);
    void Press();
    string ToString();
}