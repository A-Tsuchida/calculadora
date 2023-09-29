// For more information see https://aka.ms/fsharp-console-apps
open Calculator
open Cpu
open Keyboard
open Display
open Key
open DataTypes

let keyboard = Keyboard ()
keyboard.Add (Key ("1", KeyType.Number (Number.One)))

let calc = Calculator (Cpu (), keyboard, Display ())

