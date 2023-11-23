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
        }).Aggregate((acc, cur) => acc * 10 + cur);
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
        }).Aggregate((acc, cur) => acc * 10 + cur) ?? 0;

        var ans = (decimal)@int;
        ans += dec / (decimal)Math.Pow(10, number.Decimal?.Count() ?? 0);
        if (number.IsNegative) ans *= -1;
        return ans;
    }

    public static explicit operator CompleteNumber(decimal number)
    {
        var parts = Math.Abs(number).ToString().Split('.');
        return new(
            parts[0].Select<char, Number>(ch => ch switch
            {
                '1' => new Number.One(),
                '2' => new Number.Two(),
                '3' => new Number.Three(),
                '4' => new Number.Four(),
                '5' => new Number.Five(),
                '6' => new Number.Six(),
                '7' => new Number.Seven(),
                '8' => new Number.Eight(),
                '9' => new Number.Nine(),
                '0' => new Number.Zero(),
                _ => throw new ArgumentOutOfRangeException()
            }),
            parts.Length == 1 ? null : parts[1].Select<char, Number>(ch => ch switch
            {
                '1' => new Number.One(),
                '2' => new Number.Two(),
                '3' => new Number.Three(),
                '4' => new Number.Four(),
                '5' => new Number.Five(),
                '6' => new Number.Six(),
                '7' => new Number.Seven(),
                '8' => new Number.Eight(),
                '9' => new Number.Nine(),
                '0' => new Number.Zero(),
                _ => throw new ArgumentOutOfRangeException()
            }),
            number < 0
        );
    }
}
