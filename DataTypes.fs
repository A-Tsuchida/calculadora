module DataTypes

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
| Dot
| Equals

type KeyType =
| Operation of Operation
| Number of Number