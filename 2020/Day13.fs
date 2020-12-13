module Day13

let parseInput (input: string array) =
    let earliest = input.[0] |> int

    let bus =
        input.[1].Split(',')
        |> Array.filter (fun v -> v <> "x")
        |> Array.map (int)

    (earliest, bus)

let service input =
    let (earliest, shuttles) = input |> parseInput

    let times =
        shuttles
        |> Array.fold (fun state bus ->
            let next = bus + earliest - (earliest % bus)

            (bus, next) :: state) List.empty

    (earliest, times |> List.sortBy (fun (_, y) -> y))

let Part1 input =
    let (earliest, shuttles) = input |> service
    let (id, time) = shuttles |> List.head

    id * (time - earliest)

let parseInputPart2 (input: string array) =
    input.[1].Split(',')
    |> Array.mapi (fun idx el -> if el <> "x" then Some(idx |> int64, el |> int64) else None)
    |> Array.choose id

let search input =
    let rec loop timestamp multiplier lst =
        match lst with
        | [] -> timestamp
        | ((offset, bus) :: xs) when (timestamp + offset) % bus = 0L -> loop timestamp (multiplier * bus) xs
        | _ -> loop (timestamp + multiplier) multiplier lst

    loop 0L (input |> Array.head |> snd) (input |> Array.toList |> List.skip 1)


let Part2 input = input |> parseInputPart2 |> search
