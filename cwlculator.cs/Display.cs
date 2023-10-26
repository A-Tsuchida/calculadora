using Calculator.DataTypes;
using System;
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

    public void Add(Number n)
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

    public void Add(Number[] n)
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

    public void SetDecimal()
    {
        @decimal = [];
    }
}
