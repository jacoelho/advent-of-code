module Day10

let tryAdapters input =
    let input' = input |> Seq.sort |> Seq.toList

    let rec loop (jolt, one, two, three) lst =
        match lst with
        | [] -> Some(jolt, one, two, three)
        | (x :: _) when x - jolt > 3 -> None
        | (x :: xs) when x - jolt = 3 -> loop (x, one, two, three + 1) xs
        | (x :: xs) when x - jolt = 2 -> loop (x, one, two + 1, three) xs
        | (x :: xs) when x - jolt = 1 -> loop (x, one + 1, two, three) xs
        | (_ :: xs) -> loop (jolt, one, two, three) xs

    loop (0, 0, 0, 1) input'

let Part1 (input: string array) =
    let result = input |> Array.map (int) |> tryAdapters

    match result with
    | Some (_, one, _, three) -> one * three
    | None -> -1

// looks like a variation of `Find total ways to reach the n'th stair' dynamic programming
let countAdaptersUsed input =
    let input' =
        input
        |> Array.append [| 0L
                           (3L + (input |> Array.max)) |]
        |> Array.sort

    let len = input' |> Array.length
    let counter = Array.create (len - 1) 0L

    counter.[0] <- 1L

    let rec loop stop i j =
        match j with
        | j when j >= stop -> ()
        | j when input'.[j] - input'.[i] <= 3L ->
            counter.[j] <- counter.[j] + counter.[i]
            loop stop i (j + 1)
        | _ -> ()

    for i in 0 .. len - 1 do
        loop (len - 1) i (i + 1)

    counter

let Part2 (input: string array) =
    input
    |> Array.map (int64)
    |> countAdaptersUsed
    |> Seq.last
