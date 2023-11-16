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

    protected IEnumerable<Number> memoryIntegral = [ new GhostZero() ];
    protected IEnumerable<Number>? memoryDecimal;
    protected bool isMemoryNegative;

    protected IEnumerable<Number> accumulatorIntegral = [ new GhostZero() ];
    protected IEnumerable<Number>? accumulatorDecimal;
    protected bool isAccumulatorNegative;

    protected IEnumerable<Number> currentIntegral = [ new GhostZero() ];
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
                if (!isOn && co is not { Value: Control.On})
                    return;
                ProcessControl(co.Value);
                break;
            case KeyType.Number nu:
                ProcessNumber(nu.Value);
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

    protected virtual void ProcessControl(Control control)
    {
        switch (control)
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
            case Control.MemorySubtraction:
                try
                {
                var ans = Calculate(
                    control is Control.MemorySum ? new Operation.Sum() : new Operation.Subtraction(),
                        new(memoryIntegral,  memoryDecimal,  isMemoryNegative),
                    new(currentIntegral, currentDecimal, isCurrentNegative)
                );
                accumulatorIntegral   = currentIntegral   = memoryIntegral   = ans.Integral;
                accumulatorDecimal    = currentDecimal    = memoryDecimal    = ans.Decimal;
                isAccumulatorNegative = isCurrentNegative = isMemoryNegative = ans.IsNegative;
                }
                catch
                {
                    error = true;
                    errorHandler?.Invoke();
                }
                break;
            case Control.Equal:
                break;
            case Control.Decimal:
                currentDecimal ??= [ new GhostZero() ];
                break;
            case Control.InvertSignal:
                isCurrentNegative = !isCurrentNegative;
                break;
        }
    }

    protected virtual void ProcessNumber(Number number)
    {
        if (currentDecimal is null)
        {
            if (currentIntegral.ElementAt(0) is GhostZero)
            {
                if (number is Number.Zero)
                    return;
                
                    currentIntegral = [ number ];
                resetCurrent = false;
            }
            else
            {
                currentIntegral = currentIntegral.Append(number);
                resetCurrent    = false;
            }
        }
        else
        {
            currentIntegral = currentIntegral.ElementAt(0) is GhostZero
                            ? ([ number ])
                            : currentIntegral.Append(number);
        }
        numberHandler?.Invoke(number);
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
            currentIntegral = [ new GhostZero() ];
            currentDecimal = null;
            resetCurrent = false;
        }
        if (option == ResetOption.All || option == ResetOption.Memory)
        {
            memoryIntegral = [ new GhostZero() ];
            memoryDecimal = null;
            isMemoryNegative = false;
        }
        if (option == ResetOption.All)
        {
            accumulatorIntegral = [ new GhostZero() ];
            accumulatorDecimal = null;
            isAccumulatorNegative = false;
            error = false;
            operation = null;
        }
    }

    /// <summary>
    /// Used for when there's no actual number
    /// </summary>
    protected class GhostZero : Number.Zero { }

    protected enum ResetOption : byte
    {
        All = 0,
        Entry = 1,
        Memory = 2,
    }
}
