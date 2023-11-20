using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;
public abstract class Operation
{
    public class Multiplication : Operation;
    public class Division       : Operation;
    public class Sum            : Operation;
    public class Subtraction    : Operation;
}
