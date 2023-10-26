using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DataTypes;

public delegate void KeyHandler(KeyType key);
public delegate void NumberHandler(Number number);
public delegate void DecimalHandler();
public delegate void NegativeHandler();
public delegate void ErrorHandler();
public delegate void ClearHandler();
public delegate void DataHandler(bool isError, bool isNegative, Number[] integral, Number[] @decimal);
