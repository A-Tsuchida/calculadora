using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public abstract class Control
{
    public class On                : Control;
    public class Off               : Control;
    public class ClearEntry        : Control;
    public class MemoryRead        : Control;
    public class MemoryClear       : Control;
    public class MemorySum         : Control;
    public class MemorySubtraction : Control;
    public class Equal             : Control;
    public class Decimal           : Control;
    public class InvertSignal      : Control;
}
