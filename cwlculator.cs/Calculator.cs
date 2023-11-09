using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator;
public class Calculator
{
    private Cpu cpu;

    public IDisplay Display { get; }
    public IKeyboard Keyboard { get; }

    public Calculator(IDisplay display, IKeyboard keyboard, Cpu cpu)
    {
        Display = display;
        Keyboard = keyboard;

        this.cpu = cpu;
    }
}
