using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public abstract record Control : OpCode
{
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
