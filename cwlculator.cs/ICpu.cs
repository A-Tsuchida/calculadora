using Calculator.DataTypes;

namespace Calculator;
public interface ICpu
{
    event ClearHandler ClearEvent;
    event DataHandler DataEvent;
    event DecimalHandler DecimalEvent;
    event ErrorHandler ErrorEvent;
    event NegativeHandler NegativeEvent;
    event NumberHandler NumberEvent;

    void Process(OpCode key);
}