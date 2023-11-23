using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public abstract record Operation : OpCode
{
    public record Multiplication : Operation;
    public record Division       : Operation;
    public record Sum            : Operation;
    public record Subtraction    : Operation;
}
