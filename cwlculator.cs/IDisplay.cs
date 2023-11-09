using Calculator.DataTypes;

namespace Calculator;
public interface IDisplay
{
    int DigitCount { get; }
    int MaxDigitCount { get; }

    void Add(Number n);
    void AddRange(IEnumerable<Number> numbers);
    void Clear();
    void SetDecimal();
    void SetError();
    void SetNegative();
}