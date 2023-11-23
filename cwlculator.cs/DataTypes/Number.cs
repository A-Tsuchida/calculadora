using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;

public abstract record Number : OpCode
{

    /// <summary>
    /// Used for when there's no actual number
    /// </summary>
    public record GhostZero : Zero;
    public record Zero      : Number;
    public record One       : Number;
    public record Two       : Number;
    public record Three     : Number;
    public record Four      : Number;
    public record Five      : Number;
    public record Six       : Number;
    public record Seven     : Number;
    public record Eight     : Number;
    public record Nine      : Number;
}
