using Calculator.DataTypes;
using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator;
public class Display
{
    protected IEnumerable<Number> integral = [];
    protected IEnumerable<Number>? @decimal;
    protected bool isNumberNegative = false;
    protected bool hasError = false;

    public virtual void Add(Number n)
    {
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

    public virtual void Add(Number[] n)
    {
        if (@decimal is not null)
        {
            if (@decimal.Last() is Number.Zero && n is Number.Zero)
                return;
            @decimal = @decimal.Concat(n);
        }
        else
        {
            if (integral.Last() is Number.Zero && n is Number.Zero)
                return;
            integral = integral.Concat(n);
        }
        Print();
    }

    public void SetDecimal() => @decimal ??= [];

    public void SetNegative() => isNumberNegative = true;

    public void SetError() => hasError = true;

    protected string RenderDigit(Number d)
    {
        return d switch
        {
            Number.One   => "1",
            Number.Two   => "2",
            Number.Three => "3",
            Number.Four  => "4",
            Number.Five  => "5",
            Number.Six   => "6",
            Number.Seven => "7",
            Number.Eight => "8",
            Number.Nine  => "9",
            _ => throw ArgumentOutOfRangeException()
        };
    }

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
