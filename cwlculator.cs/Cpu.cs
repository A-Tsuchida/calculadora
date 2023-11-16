using Calculator.DataTypes;

namespace Calculator;

public class Cpu
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

    public event NumberHandler NumberEvent
    {
        add => numberHandler = (NumberHandler?)Delegate.Combine(numberHandler, value);
        remove => numberHandler = (NumberHandler?)Delegate.Remove(numberHandler, value);
    }

    public event DecimalHandler DecimalEvent
    {
        add => decimalHandler = (DecimalHandler?)Delegate.Combine(decimalHandler, value);
        remove => decimalHandler = (DecimalHandler?)Delegate.Remove(decimalHandler, value);
    }

    public event NegativeHandler NegativeEvent
    {
        add => negativeHandler = (NegativeHandler?)Delegate.Combine(negativeHandler, value);
        remove => negativeHandler = (NegativeHandler?)Delegate.Remove(negativeHandler, value);
    }

    public event ErrorHandler ErrorEvent
    {
        add => errorHandler = (ErrorHandler?)Delegate.Combine(errorHandler, value);
        remove => errorHandler = (ErrorHandler?)Delegate.Remove(errorHandler, value);
    }

    public event ClearHandler ClearEvent
    {
        add => clearHandler = (ClearHandler?)Delegate.Combine(clearHandler, value);
        remove => clearHandler = (ClearHandler?)Delegate.Remove(clearHandler, value);
    }

    public virtual void Process(KeyType key)
    {
        if (error && key is not KeyType.Control { Value: Control.On })
            return;

        switch (key)
        {
            case KeyType.Control co:
                if (!isOn && co.Value is not Control.On)
                    return;
                switch (co.Value)
                {
                    case Control.On:
                        if (!isOn) isOn = true;

                        clearHandler?.Invoke();
                        Reset(ResetOption.All);
                        break;
                    case Control.Off:
                        isOn = false;
                        break;
                    case Control.ClearEntry:
                        Reset(ResetOption.Entry);
                        break;
                    case Control.MemoryRead:
                        accumulatorIntegral = memoryIntegral;
                        accumulatorDecimal = memoryDecimal;
                        isAccumulatorNegative = isMemoryNegative;
                        resetCurrent = true;
                        break;
                    case Control.MemoryClear:
                        Reset(ResetOption.Memory);
                        break;
                    case Control.MemorySum:

                        break;
                    case Control.MemorySubtraction:
                        break;
                    case Control.Equal:
                        break;
                    case Control.Decimal:
                        break;
                    case Control.InvertSignal:
                        break;
                }
                break;
            case KeyType.Number nu:
                break;
            case KeyType.Operation op:
                if (accumulatorIntegral.Any() && operation is not null)
                {
                    var a = new CompleteNumber(currentIntegral, currentDecimal, isCurrentNegative);
                    var b = new CompleteNumber(accumulatorIntegral, accumulatorDecimal, isAccumulatorNegative);
                    try
                    {
                        var result = Calculate(operation, a, b);
                        accumulatorIntegral = result.Integral;
                        accumulatorDecimal = result.Decimal;
                        isAccumulatorNegative = result.IsNegative;
                        resetCurrent = true;
                        clearHandler?.Invoke();

                        result.Integral.All(num => { numberHandler?.Invoke(num); return true; });
                        if (result.Decimal is not null)
                        {
                            decimalHandler?.Invoke();
                            result.Decimal.All(num => { numberHandler?.Invoke(num); return true; });
                        }
                        if (result.IsNegative)
                            negativeHandler?.Invoke();
                    }
                    catch
                    {
                        errorHandler?.Invoke();
                    }
                }
                else
                {
                    accumulatorIntegral = currentIntegral;
                    accumulatorDecimal = currentDecimal;
                    isAccumulatorNegative = isCurrentNegative;
                    resetCurrent = true;
                    operation = op.Value;
                }
                break;
        }
    }

    protected virtual CompleteNumber Calculate(Operation op, CompleteNumber a, CompleteNumber b)
    {
        return op switch
        {
            Operation.Sum => (CompleteNumber)((decimal)a + (decimal)b),
            Operation.Subtraction => (CompleteNumber)((decimal)a - (decimal)b),
            Operation.Multiply => (CompleteNumber)((decimal)a * (decimal)b),
            Operation.Divide => (CompleteNumber)((decimal)a / (decimal)b),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    protected virtual void Reset(ResetOption option)
    {
        if (option == ResetOption.All || option == ResetOption.Entry)
        {
            currentIntegral = [];
            currentDecimal = null;
            resetCurrent = false;
        }
        if (option == ResetOption.All || option == ResetOption.Memory)
        {
            memoryIntegral = [];
            memoryDecimal = null;
            isMemoryNegative = false;
        }
        if (option == ResetOption.All)
        {
            accumulatorIntegral = [];
            accumulatorDecimal = null;
            isAccumulatorNegative = false;
            error = false;
            operation = null;
        }
    }

    protected enum ResetOption : byte
    {
        All = 0,
        Entry = 1,
        Memory = 2,
    }
}
