using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public abstract record Operation
{
    public static readonly Multiply MULTIPLICATION = new();
    public static readonly Divide DIVISION         = new();
    public static readonly Sum SUM                 = new();
    public static readonly Subtraction SUBTRACTION = new();

    public record Multiply    : Operation;
    public record Divide      : Operation;
    public record Sum         : Operation;
    public record Subtraction : Operation;
}
