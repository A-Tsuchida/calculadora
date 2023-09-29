module Calculator

open Keyboard
open Cpu
open Display

type Calculator (cpu: Cpu, keyboard: Keyboard, display: Display) as this =

    do
        cpu.SetDataHandler (Some display.SetNumber)
        cpu.SetErrorHandler (Some display.SetError)
        cpu.SetSignalHandler (Some display.SetSignal)
        keyboard.SetDataHandler (Some cpu.DoAction)

    member public _.Keyboard
        with get () = this.Keyboard

    member public _.Display
        with get () = this.Display
