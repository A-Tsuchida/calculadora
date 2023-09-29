module DataTypes

type Number =
| One
| Two
| Three
| Four
| Five
| Six
| Seven
| Eigth
| Nine
| Zero

type Control =
| On
| Off
| ClearError
| Memory
| MemoryReadClear
| MemorySum
| MemorySubtraction
| Equal
| Decimal

type Operation =
| Multiply
| Divide
| Sum
| Subtraction

type KeyType =
| Operation of Operation
| Number of Number
| Control of Control