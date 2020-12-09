open Day09
open System.IO

let readInput file = file |> File.ReadAllLines

[<EntryPoint>]
let main argv =
    printfn "Part01: %A" ( argv.[0] |> readInput |> Day09.Part1)
    printfn "Part02: %A" ( argv.[0] |> readInput |> Day09.Part2)
    0