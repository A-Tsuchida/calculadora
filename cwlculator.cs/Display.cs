using Calculator.DataTypes;
using static System.Console;

namespace Calculator;
public class Display(int maxDigitCount) : IDisplay
{
    protected IEnumerable<Number> integral = [ new Number.GhostZero() ];
    protected IEnumerable<Number>? @decimal;
    protected bool isNumberNegative = false;
    protected bool hasError = false;

    public int MaxDigitCount => maxDigitCount;
    public int DigitCount => integral.Count() + @decimal?.Count() ?? 0;

    public virtual void Add(Number n)
    {
        if (DigitCount >= maxDigitCount) return;

        if (@decimal is not null)
        {
            @decimal = @decimal.ElementAt(0) is Number.GhostZero
                     ? ([ n ])
                     : @decimal.Append(n);
        }
        else
        {
            if (integral.ElementAt(0) is not Number.GhostZero || n is not Number.Zero)
                integral = integral.ElementAt(0) is Number.GhostZero
                         ? ([n])
                         : integral.Append(n);
        }
        Print();
    }

    public virtual void AddNumber(IEnumerable<Number> integral, IEnumerable<Number>? @decimal, bool isNegative)
    {
        this.integral = integral.Take(MaxDigitCount);
        this.@decimal = @decimal?.Take(MaxDigitCount - this.integral.Count());
        isNumberNegative = isNegative;
        Print();
    }

    public void SetDecimal()
    {
        if (@decimal is null)
        {
            @decimal = [ new Number.GhostZero() ];
            Print();
        }
    }

    public void SetNegative()
    {
        isNumberNegative = true;
        Print();
    }

    public void SetError()
    {
        hasError = true;
        Print();
    }

    protected virtual string RenderDigit(Number d) => d switch
    {
        Number.GhostZero or Number.Zero => "0",
        Number.One => "1",
        Number.Two => "2",
        Number.Three => "3",
        Number.Four => "4",
        Number.Five => "5",
        Number.Six => "6",
        Number.Seven => "7",
        Number.Eight => "8",
        Number.Nine => "9",
        _ => throw new ArgumentOutOfRangeException($"'{d}' is not a valid digit.", (Exception?)null)
    };

    protected virtual void Print()
    {
        if (hasError)
            Write("\u001B[31m[E] ");
        else
            Write("\u001B[32m");

        if (isNumberNegative)
            Write("-");

        Write(string.Join("", integral.Select(n => RenderDigit(n))));

        if (@decimal is not null)
        {
            Write(".");
            Write(string.Join("", @decimal.Select(n => RenderDigit(n))));
        }

        WriteLine("\u001B[39;49m");
    }

    public virtual void Clear()
    {
        integral = [ new Number.GhostZero() ];
        @decimal = null;
        isNumberNegative = false;
        hasError = false;
    }
}
