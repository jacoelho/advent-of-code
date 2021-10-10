module Day01

let rec pairs (input: int array) =
    seq {
        for i in 0 .. input.Length - 1 do
            for j in i + 1 .. input.Length - 1 do
                yield (input.[i], input.[j])
    }

let rec triplets (input: int array) =
    seq {
        for i in 0 .. input.Length - 1 do
            for j in i + 1 .. input.Length - 1 do
                for k in j + 1 .. input.Length - 1 do
                    yield (input.[i], input.[j], input.[k])
    }

let Part1 (input: string array) =
    let (a, b) =
        input
        |> Array.map (int)
        |> pairs
        |> Seq.find (fun (x, y) -> x + y = 2020)

    a * b

let Part2 (input: string array) =
    let (a, b, c) =
        input
        |> Array.map (int)
        |> triplets
        |> Seq.find (fun (x, y, z) -> x + y + z = 2020)

    a * b * c
