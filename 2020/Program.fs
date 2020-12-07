open Day07

[<EntryPoint>]
let main argv =
    let input = "inputs/day07.txt"
    printfn "Part01: %A" ( input |> Day07.Part1)
    printfn "Part02: %A" (input |> Day07.Part2)
    0