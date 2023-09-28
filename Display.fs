module Display

type DataHandler = float -> unit
type SignalHandler = bool -> unit
type ErrorHandler = bool -> unit

type Display () as this =
  [<DefaultValue>] val mutable private number: float
  [<DefaultValue>] val mutable private signal: bool
  [<DefaultValue>] val mutable private error: bool

  do
    this.error <- false
    this.signal <- false
    this.number <- 0.

  member public x.SetNumber value =
    x.number <- value

  member public x.SetSignal state =
    x.signal <- state

  member public x.SetError state =
    x.error <- state