module Key
open DataTypes

type Key(face: string, kt: KeyType) as this =
  [<DefaultValue>] val mutable private onPress: KeyType -> unit

  do
    this.onPress <- fun _ -> ignore ()

  member public _.Face with get() = face
  member public _.Type with get() = kt

  member public x.SetPressListener (callback: KeyType -> unit) =
    x.onPress <- callback

  member public x.Press () = x.onPress kt
