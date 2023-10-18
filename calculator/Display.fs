module Display
open DataTypes


type DataHandler = bool -> bool -> Number list -> Number list option -> unit
type NumberHandler = Number -> unit
type DecimalHandler = unit -> unit
type SignalHandler = bool -> unit
type ErrorHandler = bool -> unit
type ClearHandler = unit -> unit

type Display () as this =
  [<DefaultValue>] val mutable private number: Number list
  [<DefaultValue>] val mutable private decimal: Number list option
  [<DefaultValue>] val mutable private signal: bool
  [<DefaultValue>] val mutable private error: bool

  do
    this.error <- false
    this.signal <- false
    this.number <- List.Empty
    this.decimal <- None

  member public _.Add (value: Number) =
    let updateList (lst: Number list) =
      match lst, value with
      | Zero :: [], Zero -> lst
      | _, _ -> value :: lst

    match this.decimal with
    | Some lst -> this.decimal <- Some (updateList lst)
    | None -> this.number <- updateList this.number

    this.Print ()

  member public _.Set error signal number decimal =
    this.error <- error
    this.signal <- signal
    this.number <- number
    this.decimal <- decimal
    this.Print ()

  member public _.SetDecimal () =
    if this.decimal = None
    then this.decimal <- Some []

  member public _.SetSignal state =
    this.signal <- state

  member public _.SetError state =
    this.error <- state

  member private _.RenderDigit (digit: Number) =
    match digit with
    | One -> "1"
    | Two -> "2"
    | Three -> "3"
    | Four -> "4"
    | Five -> "5"
    | Six -> "6"
    | Seven -> "7"
    | Eigth -> "8"
    | Nine -> "9"
    | Zero -> "0"

  member private _.Print () =
    if this.error
    then printf "\u001B[32m[E] "
    else printf "\u001B[33m"
    if this.signal
    then printf "- "
    let rec ``process`` (list: Number List) =
      match list with
      | current :: remain -> printf "%s" (this.RenderDigit current)
                             ``process`` remain
      | [] -> ()
    this.number
    |> List.rev
    |> ``process``
    match this.decimal with
    | Some lst ->
      printf "."
      lst
      |> List.rev
      |> ``process``
    | None -> ()
    printfn "\u001B[39;49m"

  member public _.Clear () = this.Set false false List.Empty None
