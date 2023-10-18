module Calculator

open Keyboard
open Cpu
open Display

type Calculator (cpu: Cpu, keyboard: Keyboard, display: Display) as this =

    do
        cpu.SetNumberHandler (Some display.Add)
        cpu.SetDecimalHandler (Some display.SetDecimal)
        cpu.SetErrorHandler (Some display.SetError)
        cpu.SetSignalHandler (Some display.SetSignal)
        cpu.SetClearHandler (Some display.Clear)
        keyboard.SetOperationHandler (Some cpu.Process)

    member public _.Keyboard
        with get () = this.Keyboard

    member public _.Display
        with get () = this.Display
