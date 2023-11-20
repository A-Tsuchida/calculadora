using Calculator;
using Calculator.DataTypes;

var dsp = new Display(8);
var cpu = new Cpu();
cpu.ClearEvent += dsp.Clear;
cpu.DataEvent += dsp.AddNumber;
cpu.DecimalEvent += dsp.SetDecimal;
cpu.ErrorEvent += dsp.SetError;
cpu.NegativeEvent += dsp.SetNegative;
cpu.NumberEvent += dsp.Add;

cpu.Process(new OpCode.Control(new Control.On()));
cpu.Process(new OpCode.Number(new Number.Nine()));
cpu.Process(new OpCode.Operation(new Operation.Division()));
cpu.Process(new OpCode.Number(new Number.Three()));
cpu.Process(new OpCode.Control(new Control.InvertSignal()));
cpu.Process(new OpCode.Control(new Control.Equal()));
