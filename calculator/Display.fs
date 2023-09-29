module Display
open DataTypes

type DataHandler = float -> unit
type SignalHandler = bool -> unit
type ErrorHandler = bool -> unit

type Display () as this =
  [<DefaultValue>] val mutable private number: Number List
  [<DefaultValue>] val mutable private hasDecimal: bool
  [<DefaultValue>] val mutable private decimal: Number List
  [<DefaultValue>] val mutable private signal: bool
  [<DefaultValue>] val mutable private error: bool

  do
    this.error <- false
    this.signal <- false
    this.number <- List.Empty
    this.hasDecimal <- false
    this.decimal <- List.Empty

  member public x.Add (value: Number) =
    if x.hasDecimal
    then
      x.decimal <- x.decimal @ [value]
    else
      x.number <- x.number @ [value]

  member public x.SetDecimal () =
    x.hasDecimal <- true

  member public x.SetSignal state =
    x.signal <- state

  member public x.SetError state =
    x.error <- state

  member private x.PrintDigit (digit: Number) =
    match digit with
    | One