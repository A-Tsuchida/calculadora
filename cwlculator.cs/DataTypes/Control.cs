using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public abstract record Control
{
    public static readonly On ON                               = new();
    public static readonly Off OFF                             = new();
    public static readonly ClearEntry CLEARENTRY               = new();
    public static readonly MemoryRead MEMORYREAD               = new();
    public static readonly MemoryClear MEMORYCLEAR             = new();
    public static readonly MemorySum MEMORYSUM                 = new();
    public static readonly MemorySubtraction MEMORYSUBTRACTION = new();
    public static readonly Equal EQUAL                         = new();
    public static readonly Decimal DECIMAL                     = new();
    public static readonly InvertSignal INVERTSIGNAL           = new();

    public record On                : Control;
    public record Off               : Control;
    public record ClearEntry        : Control;
    public record MemoryRead        : Control;
    public record MemoryClear       : Control;
    public record MemorySum         : Control;
    public record MemorySubtraction : Control;
    public record Equal             : Control;
    public record Decimal           : Control;
    public record InvertSignal      : Control;
}
