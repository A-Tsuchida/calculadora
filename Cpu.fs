module Cpu
open DataTypes
open Display

type Cpu () as this =
  [<DefaultValue>] val mutable private onSendData: DataHandler Option
  [<DefaultValue>] val mutable private onSendSignal: SignalHandler Option
  [<DefaultValue>] val mutable private onSendError: ErrorHandler Option
  [<DefaultValue>] val mutable private operation: Operation
  [<DefaultValue>] val mutable private accumulator: float
  [<DefaultValue>] val mutable private current: float
  [<DefaultValue>] val mutable private resetCurrent: bool
  [<DefaultValue>] val mutable private decimalCount: int

  do
    this.operation <- Equals
    this.accumulator <- 0
    this.current <- 0
    this.resetCurrent <- false
    this.decimalCount <- 0

  member public _.SetErrorHandler handler =
    this.onSendError <- handler

  member public _.SetSignalHandler handler =
    this.onSendSignal <- handler

  member public _.SetDataHandler handler =
    this.onSendData <- handler

  member public _.DoAction operation =
    match operation with
    | Number n ->
      if this.resetCurrent
      then
        this.current <- float n
        this.decimalCount <- 0
        this.resetCurrent <- false
      else if this.decimalCount = 0
      then
        this.current <- this.current * 10. + float n
      else
        this.current <- this.current + (float n) * (10. ** -this.decimalCount)
        this.decimalCount <- this.decimalCount + 1
      match this.onSendData with
      | Some evt -> evt this.current
      | None -> ()
    | Operation op ->
      if op = Dot
      then
        if this.resetCurrent
        then
          this.current <- 0
          this.resetCurrent <- false
        this.decimalCount <- 1
      else
        this.Calculate op

  member private _.Calculate ``type`` =
    match ``type`` with
    | Multiply ->
      this.accumulator <- this.accumulator * this.current
      this.resetCurrent <- true
      this.operation <- Multiply
    | Divide ->
      this.accumulator <- this.accumulator / this.current
      this.resetCurrent <- true
      this.operation <- Divide
    | Sum ->
      this.accumulator <- this.accumulator + this.current
      this.resetCurrent <- true
      this.operation <- Sum
    | Subtraction ->
      this.accumulator <- this.accumulator - this.current
      this.resetCurrent <- true
      this.operation <- Subtraction
    | Dot ->
      raise (System.ArgumentOutOfRangeException "Invalid Operation for current state")
    | Equals ->
      this.Calculate this.operation

