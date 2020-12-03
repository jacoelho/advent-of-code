open Day03

[<EntryPoint>]
let main argv =
    let input = "inputs/day03.txt"
    printfn "Part01: %A" ( input |> Day03.Part1)
    printfn "Part02: %A" (input |> Day03.Part2)
    0