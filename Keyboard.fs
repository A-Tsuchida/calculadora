module Keyboard
open Key

type Keyboard () as this =
  [<DefaultValue>] val mutable private keys: Key List

  do
    this.keys <- List.Empty

  member x.Add (key: Key) =
    if not (List.exists (fun (item: Key) -> item.Type = key.Type) x.keys) then
      key.SetPressListener (fun kt -> kt |> ignore)
      x.keys <- key::x.keys

  member x.GetKey (keyType) =
    List.tryFind (fun (item: Key) -> item.Type = keyType) x.keys