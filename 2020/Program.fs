open Day05

[<EntryPoint>]
let main argv =
    let input = "inputs/day05.txt"
    printfn "Part01: %A" ( input |> Day05.Part1)
    printfn "Part02: %A" (input |> Day05.Part2)
    0