using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public record CompleteNumber(IEnumerable<Number> Integral, IEnumerable<Number>? Decimal, bool IsNegative)
{
    public (IEnumerable<Number> integral, IEnumerable<Number>? @decimal, bool isNegative) Deconstruct()
        => (Integral, Decimal, IsNegative);

    public static explicit operator decimal(CompleteNumber number)
    {
        var @int = number.Integral.Select(n => n switch
        {
            Number.One => 1,
            Number.Two => 2,
            Number.Three => 3,
            Number.Four => 4,
            Number.Five => 5,
            Number.Six => 6,
            Number.Seven => 7,
            Number.Eight => 8,
            Number.Nine => 9,
            _ => 0
        }).Aggregate((acc, cur) => acc + cur);
        var dec = number.Decimal?.Select(n => n switch
        {
            Number.One => 1,
            Number.Two => 2,
            Number.Three => 3,
            Number.Four => 4,
            Number.Five => 5,
            Number.Six => 6,
            Number.Seven => 7,
            Number.Eight => 8,
            Number.Nine => 9,
            _ => 0
        }).Aggregate((acc, cur) => acc + cur) ?? 0;

        return @int + dec / (number.Decimal?.Count() ?? 1);
    }

    public static explicit operator CompleteNumber(decimal number)
    {
        var parts = Math.Abs(number).ToString().Split('.');
        return new(
            parts[0].Select<char, Number>(ch => ch switch
            {
                '1' => new Number.One(),
                '2' => new Number.Two(),
                '3' => new Number.Two(),
                '4' => new Number.Two(),
                '5' => new Number.Two(),
                '6' => new Number.Two(),
                '7' => new Number.Two(),
                '8' => new Number.Two(),
                '9' => new Number.Two(),
                '0' => new Number.Two(),
                _ => throw new ArgumentOutOfRangeException()
            }),
            parts.Length == 1 ? null : parts[1].Select<char, Number>(ch => ch switch
            {
                '1' => new Number.One(),
                '2' => new Number.Two(),
                '3' => new Number.Two(),
                '4' => new Number.Two(),
                '5' => new Number.Two(),
                '6' => new Number.Two(),
                '7' => new Number.Two(),
                '8' => new Number.Two(),
                '9' => new Number.Two(),
                '0' => new Number.Two(),
                _ => throw new ArgumentOutOfRangeException()
            }),
            number < 0
        );
    }
}
