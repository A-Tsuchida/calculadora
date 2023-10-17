module Keyboard
open Key
open DataTypes

type Keyboard () as this =
  [<DefaultValue>] val mutable private keys: Key List
  [<DefaultValue>] val mutable private sendData: (KeyType -> unit) Option

  do
    this.keys <- List.Empty

  member public _.SetOperationHandler event =
    this.sendData <- event

  member public _.Add (key: Key) =
    if not (List.exists (fun (item: Key) -> item.Type = key.Type) this.keys) then
      key.SetPressListener this.sendData
      this.keys <- key::this.keys

  member public _.remove key =
    let rec ``process`` (key: Key) (list: Key List) =
      match list with
      | current :: remain ->
        if current.Type = key.Type
        then remain
        else current::(``process`` key remain)
      | [] -> List.empty
    this.keys <- ``process`` key this.keys

  member public _.GetKey (face) =
    List.tryFind (fun (item: Key) -> item.Face = face) this.keys
