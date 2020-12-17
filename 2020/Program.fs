open Day17
open System.IO

let readInput file = file |> File.ReadAllLines

[<EntryPoint>]
let main argv =
    printfn "Part01: %A" ( argv.[0] |> readInput |> Day17.Part1)
    printfn "Part02: %A" ( argv.[0] |> readInput |> Day17.Part2)
    0