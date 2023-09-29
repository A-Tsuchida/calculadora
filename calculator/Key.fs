module Key
open DataTypes

type Key(face: string, kt: KeyType) =
  [<DefaultValue>] val mutable private onPress: (KeyType -> unit) Option

  member public _.Face with get() = face
  member public _.Type with get() = kt

  member public x.SetPressListener callback =
    x.onPress <- callback

  member public x.Press () = 
    match x.onPress with
    | Some evt -> evt kt
    | None -> ()
