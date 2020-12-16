open Day16
open System.IO

let readInput file = file |> File.ReadAllLines

[<EntryPoint>]
let main argv =
    printfn "Part01: %A" ( argv.[0] |> readInput |> Day16.Part1)
    printfn "Part02: %A" ( argv.[0] |> readInput |> Day16.Part2)
    0