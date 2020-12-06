open Day06

[<EntryPoint>]
let main argv =
    let input = "inputs/day06.txt"
    printfn "Part01: %A" ( input |> Day06.Part1)
    printfn "Part02: %A" (input |> Day06.Part2)
    0