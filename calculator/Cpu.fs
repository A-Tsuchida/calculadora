module Cpu
open DataTypes
open Display

type Cpu () as this =
  [<DefaultValue>] val mutable private isOn: bool

  [<DefaultValue>] val mutable private onSendData: DataHandler option
  [<DefaultValue>] val mutable private onSendNumber: NumberHandler option
  [<DefaultValue>] val mutable private onSendDecimal: DecimalHandler option
  [<DefaultValue>] val mutable private onSendSignal: SignalHandler option
  [<DefaultValue>] val mutable private onSendError: ErrorHandler option
  [<DefaultValue>] val mutable private onSendClear: ClearHandler option

  [<DefaultValue>] val mutable private operation: Operation option

  [<DefaultValue>] val mutable private memory: Number list
  [<DefaultValue>] val mutable private memoryDecimal: Number list option
  [<DefaultValue>] val mutable private isMemoryNegative: bool

  [<DefaultValue>] val mutable private accumulator: Number list
  [<DefaultValue>] val mutable private accumulatorDecimal: Number list option
  [<DefaultValue>] val mutable private isAccumulatorNegative: bool
  [<DefaultValue>] val mutable private isError: bool

  [<DefaultValue>] val mutable private current: Number list
  [<DefaultValue>] val mutable private currentDecimal: Number list option
  [<DefaultValue>] val mutable private isCurrentNegative: bool

  [<DefaultValue>] val mutable private resetCurrent: bool

  do
    this.reset true

  member public _.SetErrorHandler handler =
    this.onSendError <- handler

  member public _.SetSignalHandler handler =
    this.onSendSignal <- handler

  member public _.SetNumberHandler handler =
    this.onSendNumber <- handler

  member public _.SetDecimalHandler handler =
    this.onSendDecimal <- handler
    
  member public _.SetClearHandler handler =
    this.onSendClear <- handler
    
  member public _.SetDataHandler handler =
    this.onSendData <- handler

  member public _.Process operation =
    match operation, this.isOn with
    | Number n, true ->
      if this.resetCurrent
      then
        this.current <- [n]
        this.currentDecimal <- None
        this.isCurrentNegative <- false
        this.resetCurrent <- false
        match this.onSendClear with
        | Some evt -> evt ()
        | None     -> ()
      else
        match this.currentDecimal with
        | Some dec -> this.currentDecimal <- Some (n :: dec)
        | None -> this.current <- n :: this.current
      match this.onSendNumber with
      | Some evt -> evt n
      | None     -> ()
    | Operation op,true ->
      match this.operation with
      | Some _ -> 
        this.Calculate this.operation
        match this.onSendData, this.onSendClear with
        | Some evt, Some clear ->
          clear ()
          evt this.isError this.isAccumulatorNegative (this.accumulator |> List.rev) (match this.accumulatorDecimal with | Some dec -> dec |> List.rev |> Some | None -> None)
        | _ -> ()
      | None ->
        this.accumulator <- this.current
        this.accumulatorDecimal <- this.currentDecimal
        this.resetCurrent <- true
      this.operation <- Some op
    | Control co, true ->
      match co with
      | On ->
        match this.onSendClear with
        | Some evt -> evt ()
        | None -> ()
        this.reset true
      | Off ->
        match this.onSendClear with
        | Some evt -> evt ()
        | None -> ()
        this.reset true
        this.isOn <- false
      | ClearEntry ->
          this.reset false
      | MemoryClear ->
        this.memory           <- [ Zero ]
        this.memoryDecimal    <- None
        this.isMemoryNegative <- false
      | MemoryRead ->
        this.accumulator           <- this.memory
        this.accumulatorDecimal    <- this.memoryDecimal
        this.isAccumulatorNegative <- this.isMemoryNegative
        this.resetCurrent <- true
      | MemorySum ->
          ()
      | MemorySubtraction ->
          ()
      | Equal ->
        this.Calculate this.operation
        this.operation <- None
        match this.onSendData, this.onSendClear with
        | Some evt, Some clear ->
          clear ()
          evt this.isError this.isAccumulatorNegative (this.accumulator |> List.rev) (match this.accumulatorDecimal with | Some dec -> dec |> List.rev |> Some | None -> None)
        | _ -> ()
      | Decimal ->
        this.currentDecimal <- Some List.empty
        match this.onSendDecimal with
        | Some evt -> evt ()
        | None     -> ()
      | InvertSignal ->
        this.isCurrentNegative <- not this.isCurrentNegative
        this.resetCurrent <- false
    | (co, false) when co = Control On -> this.isOn <- true
    | _, false -> ()

  member private _.Calculate (``type``: Operation Option) =
    match ``type`` with
    | Some op ->
      let a, b = this.ToDecimal this.accumulator this.accumulatorDecimal this.isAccumulatorNegative, this.ToDecimal this.current this.currentDecimal false
      if b = 0m && op = Divide
      then
        this.isError <- true
        match this.onSendError with
        | Some evt -> evt true
        | None -> ()
      else
        let result = match op with
                     | Multiply    -> a * b
                     | Divide      -> a / b
                     | Sum         -> a + b
                     | Subtraction -> a - b
                     |> this.trimDecimal
        this.operation <- Some op
        let parsed = this.ToNumberList result |> List.map (fun lst -> List.rev lst)
        this.isAccumulatorNegative <- result < 0m
        this.accumulator <- parsed[0]
        this.accumulatorDecimal <- if parsed.Length > 1
                                   then match parsed[1] with
                                        | []     -> None
                                        | [Zero] -> None
                                        | value  -> Some value
                                   else None
        this.resetCurrent <- true
        this.current <- this.accumulator
        this.currentDecimal <- this.accumulatorDecimal
    | None -> ()

  member private _.ToDecimal (``int``: Number list) (dec: Number list option) isNegative: decimal =
    let rec ``process`` isDecimal (number: Number list) =
      match number with
      | current :: remain ->
        let n = match current with
                | One   -> decimal 1
                | Two   -> decimal 2
                | Three -> decimal 3
                | Four  -> decimal 4
                | Five  -> decimal 5
                | Six   -> decimal 6
                | Seven -> decimal 7
                | Eigth -> decimal 8
                | Nine  -> decimal 9
                | Zero  -> decimal 0
        let next = ``process`` isDecimal remain
        match next, isDecimal with
        | Some m, true  -> m * 10m + n |> Some
        | Some m, false -> n + (m * decimal 10) |> Some
        | None, _       -> Some n
      | [] -> None

    let complete = ``process`` false ``int``
    let part = match dec with
               | Some value ->
                  match ``process`` true value with
                  | Some value2 -> value |> List.length |> (fun x -> value2 / (10. ** x |> decimal)) |> Some
                  | None        -> None
               | None -> None

    match complete, part with
    | Some i, Some d -> (i + d) * (if isNegative then -1m else 1m)
    | Some i, None -> i * (if isNegative then -1m else 1m)
    | None, _ -> raise (System.InvalidOperationException ("Integer part of the number must exist"))

  member private _.ToNumberList (number: decimal): Number list list =
    (number.ToString ()).Split ('.')
    |> Array.map (fun item ->
      item.ToCharArray()
      |> List.ofArray
      |> List.map (fun ch ->
                    match ch with
                    | '1' -> One
                    | '2' -> Two
                    | '3' -> Three
                    | '4' -> Four
                    | '5' -> Five
                    | '6' -> Six
                    | '7' -> Seven
                    | '8' -> Eigth
                    | '9' -> Nine
                    | '0' -> Zero
                    | _   -> raise (System.InvalidOperationException ("Invalid number"))))
    |> List.ofArray

  member private _.trimDecimal (number: decimal) =
    let numstr = (number |> string)
    if numstr.Contains('.')
    then
      numstr.TrimEnd('0') |> decimal
    else
      number

  member private _.reset (allData: bool) =
    if allData
    then
      this.operation          <- None
      this.accumulator        <- List.Empty
      this.accumulatorDecimal <- None
      this.isError            <- false
      this.memory             <- [ Zero ]
      this.memoryDecimal      <- None
      this.isMemoryNegative   <- false

    this.current        <- [ Zero ]
    this.currentDecimal <- None
    this.resetCurrent   <- false
