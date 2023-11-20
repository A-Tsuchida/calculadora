using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public record OpCode()
{
    public record Operation(DataTypes.Operation Value) : OpCode;

    public record Number(DataTypes.Number Value)       : OpCode;

    public record Control(DataTypes.Control Value)     : OpCode;
}
