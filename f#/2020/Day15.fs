module Day15

let updateMap (key: 'a) (value: 'b) (m: Map<'a, 'b list>) =
    match m.TryFind key with
    | None -> m |> Map.add key (List.singleton value)
    | Some (x) -> m |> Map.add key (value :: x)

let game starting =
    let memory =
        starting
        |> Seq.toArray
        |> Array.mapi (fun i el -> (el, [ i + 1 ]))
        |> Map.ofArray

    let memorySequence =
        ((starting |> Seq.length) + 1, (starting |> Seq.last), memory)
        |> Seq.unfold (fun (turn, last, memory) ->
            match memory.TryFind last with
            | None -> Some(0, (turn + 1, 0, memory |> updateMap 0 turn))
            | Some (x :: y :: _) -> Some(x - y, (turn + 1, x - y, memory |> updateMap (x - y) turn))
            | Some ([_]) -> Some(0, (turn + 1, 0, memory |> updateMap 0 turn))
            | Some ([ ]) -> failwithf "unexpected %A" last)

    memorySequence |> Seq.append starting

let Part1 =
    "0,8,15,2,12,1,4".Split(",")
    |> Array.map (int)
    |> game
    |> Seq.take 2020
    |> Seq.last


let Part2 =
    "0,8,15,2,12,1,4".Split(",")
    |> Array.map (int)
    |> game
    |> Seq.take 30000000
    |> Seq.last