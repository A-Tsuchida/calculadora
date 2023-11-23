using Calculator.DataTypes;

namespace Calculator;

public class Cpu : ICpu
{
    protected bool isOn = false;

    private NumberHandler?   numberHandler;
    private DecimalHandler?  decimalHandler;
    private NegativeHandler? negativeHandler;
    private DataHandler?     dataHandler;
    private ErrorHandler?    errorHandler;
    private ClearHandler?    clearHandler;

    private Operation? operation;
    private bool resetOperation;

    protected IEnumerable<Number>  memoryIntegral = [ new Number.GhostZero() ];
    protected IEnumerable<Number>? memoryDecimal;
    protected bool isMemoryNegative;

    protected IEnumerable<Number>  accumulatorIntegral = [ new Number.GhostZero() ];
    protected IEnumerable<Number>? accumulatorDecimal;
    protected bool isAccumulatorNegative;

    protected IEnumerable<Number>  currentIntegral = [ new Number.GhostZero() ];
    protected IEnumerable<Number>? currentDecimal;
    protected bool isCurrentNegative;

    protected bool error;
    protected bool resetCurrent;

    public event NumberHandler NumberEvent
    {
        add    => numberHandler = (NumberHandler?)Delegate.Combine(numberHandler, value);
        remove => numberHandler = (NumberHandler?)Delegate.Remove(numberHandler, value);
    }

    public event DecimalHandler DecimalEvent
    {
        add    => decimalHandler = (DecimalHandler?)Delegate.Combine(decimalHandler, value);
        remove => decimalHandler = (DecimalHandler?)Delegate.Remove(decimalHandler, value);
    }

    public event NegativeHandler NegativeEvent
    {
        add    => negativeHandler = (NegativeHandler?)Delegate.Combine(negativeHandler, value);
        remove => negativeHandler = (NegativeHandler?)Delegate.Remove(negativeHandler, value);
    }

    public event DataHandler DataEvent
    {
        add    => dataHandler = (DataHandler?)Delegate.Combine(negativeHandler, value);
        remove => dataHandler = (DataHandler?)Delegate.Remove(negativeHandler, value);
    }

    public event ErrorHandler ErrorEvent
    {
        add    => errorHandler = (ErrorHandler?)Delegate.Combine(errorHandler, value);
        remove => errorHandler = (ErrorHandler?)Delegate.Remove(errorHandler, value);
    }

    public event ClearHandler ClearEvent
    {
        add    => clearHandler = (ClearHandler?)Delegate.Combine(clearHandler, value);
        remove => clearHandler = (ClearHandler?)Delegate.Remove(clearHandler, value);
    }

    public virtual void Process(OpCode key)
    {
        if (error && key is not Control.On)
            return;

        switch (key)
        {
            case Control co:
                if (!isOn && co is not Control.On)
                    return;
                ProcessControl(co);
                break;
            case Number nu:
                ProcessNumber(nu);
                break;
            case Operation op:
                ProcessOperation(op);
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

                    (accumulatorIntegral, accumulatorDecimal, isAccumulatorNegative) =
                    (currentIntegral,     currentDecimal,     isCurrentNegative) =
                    (memoryIntegral,      memoryDecimal,      isMemoryNegative) =
                        ans;
                }
                catch
                {
                    error = true;
                    errorHandler?.Invoke();
                }
                break;
            case Control.Equal:
                if (operation is null)
                    return;

                try
                {
                CompleteNumber
                    a = new(accumulatorIntegral, accumulatorDecimal, isAccumulatorNegative),
                    b = new(currentIntegral,     currentDecimal,     isCurrentNegative);

                var result = Calculate(this.operation, a, b);

                    (accumulatorIntegral, accumulatorDecimal, isAccumulatorNegative) = result;

                resetCurrent = true;

                clearHandler?.Invoke();
                dataHandler?.Invoke(result.Integral, result.Decimal, result.IsNegative);
                }
                catch
                {
                    error = true;
                    errorHandler?.Invoke();
                    // I could not return here but, eh, I don't want to
                    return;
                }

                break;
            case Control.Decimal:
                currentDecimal ??= [ new Number.GhostZero() ];
                break;
            case Control.InvertSignal:
                isCurrentNegative = !isCurrentNegative;
                negativeHandler?.Invoke();
                break;
        }
    }

    protected virtual void ProcessNumber(Number number)
    {
        if (resetCurrent)
        {
            Reset(ResetOption.Entry);
            clearHandler?.Invoke();
        }

        if (currentDecimal is null)
        {
            if (currentIntegral.ElementAt(0) is Number.GhostZero)
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
            currentIntegral = currentIntegral.ElementAt(0) is Number.GhostZero
                            ? ([ number ])
                            : currentIntegral.Append(number);
        }
        numberHandler?.Invoke(number);
    }

    protected virtual void ProcessOperation(Operation operation)
    {
        if (resetOperation) this.operation = null;

        if (this.operation is not null)
        {
            CompleteNumber
                a = new(accumulatorIntegral, accumulatorDecimal, isAccumulatorNegative),
                b = new(currentIntegral,     currentDecimal,     isCurrentNegative);

            try
            {
                var result = Calculate(this.operation, a, b);

                (currentIntegral, currentDecimal, isCurrentNegative) = result;

                resetCurrent = true;

                clearHandler?.Invoke();
                dataHandler?.Invoke(result.Integral, result.Decimal, result.IsNegative);
            }
            catch
            {
                error = true;
                errorHandler?.Invoke();
                // I could not return here but, eh, I don't want to
                return;
            }
        }

        accumulatorIntegral   = currentIntegral;
        accumulatorDecimal    = currentDecimal;
        isAccumulatorNegative = isCurrentNegative;
        resetCurrent          = true;

        this.operation = operation;
    }

    protected virtual CompleteNumber Calculate(Operation op, CompleteNumber a, CompleteNumber b)
    {
        return op switch
        {
            Operation.Sum => (CompleteNumber)((decimal)a + (decimal)b),
            Operation.Subtraction => (CompleteNumber)((decimal)a - (decimal)b),
            Operation.Multiplication => (CompleteNumber)((decimal)a * (decimal)b),
            Operation.Division => (CompleteNumber)((decimal)a / (decimal)b),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    protected virtual void Reset(ResetOption option)
    {
        if (option == ResetOption.All || option == ResetOption.Entry)
        {
            currentIntegral = [ new Number.GhostZero() ];
            currentDecimal = null;
            resetCurrent = false;
        }
        if (option == ResetOption.All || option == ResetOption.Memory)
        {
            memoryIntegral = [ new Number.GhostZero() ];
            memoryDecimal = null;
            isMemoryNegative = false;
        }
        if (option == ResetOption.All)
        {
            accumulatorIntegral = [ new Number.GhostZero() ];
            accumulatorDecimal = null;
            isAccumulatorNegative = false;
            error = false;
            operation = null;
            resetOperation = false;
        }
    }

    protected enum ResetOption : byte
    {
        All = 0,
        Entry = 1,
        Memory = 2,
    }
}
