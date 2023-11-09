using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public record KeyType()
{
    public record Operation(DataTypes.Operation Value) : KeyType;

    public record Number(DataTypes.Number Value)       : KeyType;

    public record Control(DataTypes.Control Value)     : KeyType;

}
