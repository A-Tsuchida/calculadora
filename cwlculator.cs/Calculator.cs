using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator;
public class Calculator
{
    private readonly ICpu cpu;

    public IDisplay Display { get; }
    public IKeyboard Keyboard { get; }

    public Calculator(IDisplay iDisplay, IKeyboard iKeyboard, ICpu iCpu)
    {
        Display = iDisplay;
        Keyboard = iKeyboard;

        cpu = iCpu;

        cpu.ClearEvent += Display.Clear;
        cpu.DataEvent += Display.AddNumber;
        cpu.DecimalEvent += Display.SetDecimal;
        cpu.ErrorEvent += Display.SetError;
        cpu.NegativeEvent += Display.SetNegative;
        cpu.NumberEvent += Display.Add;

        Keyboard.KeyHandlerEvent += cpu.Process;
    }
}
