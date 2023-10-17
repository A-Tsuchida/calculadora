module Display
open DataTypes


type DataHandler = Number -> unit
type DecimalHandler = unit -> unit
type SignalHandler = bool -> unit
type ErrorHandler = bool -> unit

type Display () as this =
  [<DefaultValue>] val mutable private number: Number List
  [<DefaultValue>] val mutable private decimal: Number List option
  [<DefaultValue>] val mutable private signal: bool
  [<DefaultValue>] val mutable private error: bool

  do
    this.error <- false
    this.signal <- false
    this.number <- List.Empty
    this.decimal <- None

  member public x.Add (value: Number) =
    let updateList (lst: Number list) =
      match lst, value with
      | Zero :: [], Zero -> lst
      | _, _ -> value :: lst

    match x.decimal with
    | Some lst -> x.decimal <- Some (updateList lst)
    | None -> x.number <- updateList x.number

    this.Print ()

  member public x.SetDecimal () =
    if this.decimal = None
    then this.decimal <- Some []

  member public x.SetSignal state =
    x.signal <- state

  member public x.SetError state =
    x.error <- state

  member private x.RenderDigit (digit: Number) =
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

  member private x.Print () =
    if this.error
    then printf "\u001B[32m[E] "
    else printf "\u001B[33m"
    if this.signal
    then printf "- "
    let rec ``process`` (list: Number List) =
      match list with
      | current :: remain -> printf "%s" (x.RenderDigit current)
                             ``process`` remain
      | [] -> ()
    x.number
    |> List.rev
    |> ``process``
    match x.decimal with
    | Some lst ->
      printf "."
      lst
      |> List.rev
      |> ``process``
    | None -> ()
    printfn "\u001B[39;49m"

  member public x.Clear () =
    x.number <- List.Empty
    x.decimal <- None
    x.signal <- false
    x.error <- false
    this.Print ()
