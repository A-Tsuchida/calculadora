﻿open Calculator
open Cpu
open Keyboard
open Display
open Key
open DataTypes

let keyboard = Keyboard ()
keyboard.Add (Key ("1", KeyType.Number (Number.One)))

let calc = Calculator (Cpu (), keyboard, Display ())

let display = Display ()
let cpu = Cpu ()
cpu.SetNumberHandler (Some display.Add)
cpu.SetDecimalHandler (Some display.SetDecimal)
cpu.SetErrorHandler (Some display.SetError)
cpu.SetSignalHandler (Some display.SetSignal)
cpu.SetClearHandler (Some display.Clear)

Control On |> cpu.Process
Number Five |> cpu.Process
Operation Sum |> cpu.Process
Number Five |> cpu.Process
Operation Multiply |> cpu.Process
Number Two |> cpu.Process
Control Equal |> cpu.Process
