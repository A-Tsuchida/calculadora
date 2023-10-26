using Calculator.DataTypes;

namespace Calculator;

class Cpu
{
    protected bool isOn = false;

    private NumberHandler? numberHandler;
    private DecimalHandler? decimalHandler;
    private NegativeHandler? negativeHandler;
    private ErrorHandler? errorHandler;
    private ClearHandler? clearHandler;

    private Operation? operation;

    protected IEnumerable<Number> memoryIntegral = [];
    protected IEnumerable<Number>? memoryDecimal;
    protected bool isMemoryNegative;

    protected IEnumerable<Number> accumulatorIntegral = [];
    protected IEnumerable<Number>? accumulatorDecimal;
    protected bool isAccumulatorNegative;

    protected IEnumerable<Number> currentIntegral = [];
    protected IEnumerable<Number>? currentDecimal;
    protected bool isCurrentNegative;

    protected bool error;
    protected bool resetCurrent;
}
