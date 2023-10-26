using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;

public abstract record Number
{
    public static readonly Zero ZERO   = new();
    public static readonly One ONE     = new();
    public static readonly Two TWO     = new();
    public static readonly Three THREE = new();
    public static readonly Four FOUR   = new();
    public static readonly Five FIVE   = new();
    public static readonly Six SIX     = new();
    public static readonly Seven SEVEN = new();
    public static readonly Eight EIGHT = new();
    public static readonly Nine NINE   = new();

    public record Zero  : Number;
    public record One   : Number;
    public record Two   : Number;
    public record Three : Number;
    public record Four  : Number;
    public record Five  : Number;
    public record Six   : Number;
    public record Seven : Number;
    public record Eight : Number;
    public record Nine  : Number;
}
