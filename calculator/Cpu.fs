module Cpu
open DataTypes
open Display

type Cpu () as this =
  [<DefaultValue>] val mutable private isOn: bool

  [<DefaultValue>] val mutable private onSendNumber: DataHandler Option
  [<DefaultValue>] val mutable private onSendDecimal: DecimalHandler Option
  [<DefaultValue>] val mutable private onSendSignal: SignalHandler Option
  [<DefaultValue>] val mutable private onSendError: ErrorHandler Option
  [<DefaultValue>] val mutable private onSendClear: (unit -> unit) Option

  [<DefaultValue>] val mutable private operation: Operation Option

  [<DefaultValue>] val mutable private accumulator: Number List
  [<DefaultValue>] val mutable private accumulatorDecimal: Number List option
  [<DefaultValue>] val mutable private isAccumulatorNegative: bool

  [<DefaultValue>] val mutable private current: Number List
  [<DefaultValue>] val mutable private currentDecimal: Number List option

  [<DefaultValue>] val mutable private resetCurrent: bool

  do
    this.operation <- None
    this.accumulator <- List.Empty
    this.accumulatorDecimal <- None
    this.current <- List.empty
    this.currentDecimal <- None
    this.resetCurrent <- false

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

  member public _.Process operation =
    match operation with
    | Number n ->
      if this.isOn then
        if this.resetCurrent
        then
          this.current <- [n]
          this.currentDecimal <- None
          this.resetCurrent <- false
          match this.onSendClear with
          | Some evt -> evt ()
          | None -> ()
        else
          match this.currentDecimal with
          | Some dec -> this.currentDecimal <- Some (n :: dec)
          | None -> this.current <- n :: this.current
        match this.onSendNumber with
        | Some evt -> evt n
        | None -> ()
    | Operation op ->
      if this.isOn then
        match this.operation with
        | Some _ -> this.Calculate (Some op)
        | None ->
          this.operation <- Some op
          this.accumulator <- this.current
          this.accumulatorDecimal <- this.currentDecimal
          this.resetCurrent <- true
    | Control co ->
      match co with
      | On ->
        if this.isOn
        then
          match this.onSendClear with
          | Some evt -> evt ()
          | None -> ()
        else
          this.isOn <- true
      | Off -> 
        if this.isOn then
          ()
      | ClearError -> 
        if this.isOn then
          ()
      | Memory -> 
        if this.isOn then
          ()
      | MemoryReadClear -> 
        if this.isOn then
          ()
      | MemorySum -> 
        if this.isOn then
          ()
      | MemorySubtraction -> 
        if this.isOn then
          ()
      | Equal ->
        if this.isOn then
          this.Calculate this.operation
          this.operation <- None
          match this.onSendNumber, this.onSendDecimal, this.onSendClear with
          | Some evt, Some evt2, Some clear ->
            clear ()
            if this.isAccumulatorNegative then evt2 ()
            this.accumulator |> List.rev |> List.iter (fun item -> evt item)
            match this.accumulatorDecimal with
            | Some dec ->
              evt2 ()
              dec |> List.rev |> List.iter (fun item -> evt item)
            | None () -> ()
          | _ -> ()
      | Decimal ->
        if this.isOn then
          this.currentDecimal <- Some List.empty
          match this.onSendDecimal with
          | Some evt -> evt ()
          | None -> ()

  member private _.Calculate (``type``: Operation Option) =
    match ``type`` with
    | Some op ->
      let a, b = this.ToDecimal this.accumulator this.accumulatorDecimal this.isAccumulatorNegative, this.ToDecimal this.current this.currentDecimal false
      let result = match op with
                   | Multiply -> a * b
                   | Divide -> a / b
                   | Sum -> a + b
                   | Subtraction -> a - b
                   |> this.trimDecimal
      this.operation <- Some op
      let parsed = this.ToNumberList result
      this.isAccumulatorNegative <- result < 0m
      this.accumulator <- parsed[0]
      this.accumulatorDecimal <- if parsed.Length > 1
                                 then match parsed[1] with
                                      | [] -> None
                                      | [Zero] -> None
                                      | value -> Some value
                                 else None
      this.resetCurrent <- true
      this.current <- this.accumulator
      this.currentDecimal <- this.accumulatorDecimal
    | None -> ()

  member private _.ToDecimal (``int``: Number List) (dec: Number List option) isNegative: decimal =
    let rec ``process`` isDecimal (number: Number List) =
      match number with
      | current :: remain ->
        let n = match current with
                | One -> decimal 1
                | Two -> decimal 2
                | Three -> decimal 3
                | Four -> decimal 4
                | Five -> decimal 5
                | Six -> decimal 6
                | Seven -> decimal 7
                | Eigth -> decimal 8
                | Nine -> decimal 9
                | Zero -> decimal 0
        let next = ``process`` isDecimal remain
        match next, isDecimal with
        | Some m, true ->
          m * 10m + n |> Some
        | Some m, false -> n + (m * decimal 10) |> Some
        | None, _ -> Some n
      | [] -> None

    let complete = ``process`` false ``int``
    let part = match dec with
               | Some value ->
                  match ``process`` true value with
                  | Some value2 -> value |> List.length |> (fun x -> value2 / (10. ** x |> decimal)) |> Some
                  | None -> None
               | None -> None

    match complete, part with
    | Some i, Some d -> (i + d) * (if isNegative then -1m else 1m)
    | Some i, None -> i * (if isNegative then -1m else 1m)
    | None, _ -> raise (System.InvalidOperationException ("Integer part of the number must exist"))

  member private _.ToNumberList (number: decimal): Number List List =
    (number.ToString ()).Split ('.')
    |> Array.map (fun item ->
      item.ToCharArray()
      |> List.ofArray
      |> List.rev
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
                    | _ -> raise (System.InvalidOperationException ("Invalid number"))))
    |> List.ofArray

  member private _.trimDecimal (number: decimal) = (number |> string).TrimEnd('0') |> decimal
