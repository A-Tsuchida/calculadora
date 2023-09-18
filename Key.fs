module Key

type Number =
| One   = 1
| Two   = 2
| Three = 3
| Four  = 4
| Five  = 5
| Six   = 6
| Seven = 7
| Eigth = 8
| Nine  = 9
| Zero  = 0

type Operation =
| Multiply
| Divide
| Sum
| Subtraction

type KeyType =
| Operation
| Number

type Key(face: string, kt: KeyType) as this =
  [<DefaultValue>] val mutable private onPress: KeyType -> unit

  do
    this.onPress <- (fun kt -> kt |> ignore)

  member public _.Face with get() = face
  member public _.Type with get() = kt

  member public x.SetPressListener (callback: KeyType -> unit) =
    x.onPress <- callback

  member public x.Press () = x.onPress kt
