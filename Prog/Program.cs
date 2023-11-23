using Calculator;
using Calculator.DataTypes;

var calculator = new Calculator.Calculator(new Display(8), new Keyboard(), new Cpu());

calculator.Keyboard.Add(new Key("1", new Number.One()));
calculator.Keyboard.Add(new Key("2", new Number.Two()));
calculator.Keyboard.Add(new Key("3", new Number.Three()));
calculator.Keyboard.Add(new Key("4", new Number.Four()));
calculator.Keyboard.Add(new Key("5", new Number.Five()));
calculator.Keyboard.Add(new Key("6", new Number.Six()));
calculator.Keyboard.Add(new Key("7", new Number.Seven()));
calculator.Keyboard.Add(new Key("8", new Number.Eight()));
calculator.Keyboard.Add(new Key("9", new Number.Nine()));
calculator.Keyboard.Add(new Key("0", new Number.Zero()));

calculator.Keyboard.Add(new Key("/", new Operation.Division()));
calculator.Keyboard.Add(new Key("*", new Operation.Multiplication()));
calculator.Keyboard.Add(new Key("-", new Operation.Subtraction()));
calculator.Keyboard.Add(new Key("+", new Operation.Sum()));

calculator.Keyboard.Add(new Key("On", new Control.On()));
calculator.Keyboard.Add(new Key("MR", new Control.MemoryRead()));
calculator.Keyboard.Add(new Key("M+", new Control.MemorySum()));
calculator.Keyboard.Add(new Key("M-", new Control.MemorySubtraction()));
calculator.Keyboard.Add(new Key("MC", new Control.MemoryClear()));
calculator.Keyboard.Add(new Key(".",  new Control.Decimal()));
calculator.Keyboard.Add(new Key("CE", new Control.ClearEntry()));
calculator.Keyboard.Add(new Key("=", new Control.Equal()));
calculator.Keyboard.Add(new Key("+/-", new Control.InvertSignal()));

calculator.Keyboard.GetKey("On").Press();
calculator.Keyboard.GetKey("5").Press();
calculator.Keyboard.GetKey("+").Press();
calculator.Keyboard.GetKey("5").Press();
calculator.Keyboard.GetKey("/").Press();
calculator.Keyboard.GetKey("0").Press();
calculator.Keyboard.GetKey("=").Press();
calculator.Keyboard.GetKey("5").Press();
calculator.Keyboard.GetKey("5").Press();
calculator.Keyboard.GetKey("On").Press();
calculator.Keyboard.GetKey("5").Press();
calculator.Keyboard.GetKey("5").Press();
calculator.Keyboard.GetKey("5").Press();
