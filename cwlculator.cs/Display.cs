using Calculator.DataTypes;
using static System.Console;

namespace Calculator;
public class Display(int maxDigitCount) : IDisplay
{
    protected IEnumerable<Number> integral = [];
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
            if (@decimal.Last() is Number.Zero && n is Number.Zero)
                return;
            @decimal = @decimal.Append(n);
        }
        else
        {
            if (integral.Last() is Number.Zero && n is Number.Zero)
                return;
            integral = integral.Append(n);
        }
        Print();
    }

    public virtual void AddRange(IEnumerable<Number> numbers)
    {
        if (DigitCount >= maxDigitCount || !numbers.Any()) return;

        if (@decimal is not null)
        {
            @decimal = @decimal.Concat(numbers);
        }
        else
        {
            if (integral.Count() == 1 && integral.ElementAt(0) is Number.Zero)
            {
                if (numbers.All(n => n is Number.Zero)) return;
                integral = numbers;
            }
            else
            {
                integral = integral.Concat(numbers);
            }
        }
        Print();
    }

    public void SetDecimal()
    {
        if (@decimal is null)
        {
            @decimal = [];
            Print();
        }
    }

    public void SetNegative() => isNumberNegative = true;

    public void SetError() => hasError = true;

    protected virtual string RenderDigit(Number d) => d switch
    {
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

    protected void Print()
    {
        if (hasError)
            Write("\u001B[32m[E] ");
        else
            Write("\u001B[33m");

        if (isNumberNegative)
            Write("- ");

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
        integral = [];
        @decimal = null;
        isNumberNegative = false;
        hasError = false;
    }
}
