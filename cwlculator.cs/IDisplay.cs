using Calculator.DataTypes;

namespace Calculator;
public interface IDisplay
{
    int DigitCount { get; }
    int MaxDigitCount { get; }

    void Add(Number n);
    void AddNumber(IEnumerable<Number> integral, IEnumerable<Number>? @decimal, bool isNegative);
    void Clear();
    void SetDecimal();
    void SetError();
    void SetNegative();
}