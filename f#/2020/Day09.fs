module Day09

let slice size input =
    Array.windowed (size + 1) input
    |> Array.map (fun p -> Array.splitAt size p)

let windowAnySum lst n =
    let rec sum lst i n m =
        match lst with
        | [] -> None
        | (x :: xs) ->
            let diff = n - x
            match Map.tryFind diff m with
            | Some (j) -> Some(j, i)
            | _ -> sum xs (i + 1) n (Map.add x i m)

    sum lst 0 n Map.empty

let verify (window, target) =
    let res =
        windowAnySum (window |> Array.toList) (target |> Array.head)

    match res with
    | Some (_) -> None
    | None -> Some(target |> Array.head)

let findNotFollowing input = input |> Array.map verify

let Part1 (file: string array) =
    file
    |> Array.map (int64)
    |> slice 25
    |> Array.choose verify
    |> Array.head

let sumMinAndMax arr = (arr |> Array.min) + (arr |> Array.max)

let tryWindowSum (n: int64) (arr: int64 array) =
    let rec loop (low, high) sum =
        match sum with
        | sum when sum = n -> arr.[low..high]
        | sum when sum < n -> loop (low, high + 1) (sum + arr.[high + 1])
        | _ -> loop (low + 1, high) (sum - arr.[low])

    loop (0, 1) (arr.[0] + arr.[1])

let Part2 (file: string array) =
    file
    |> Array.map (int64)
    |> tryWindowSum 1309761972L
    |> sumMinAndMax
