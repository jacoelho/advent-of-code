module Day17

type State =
    | Active
    | Inactive

let fromChar (c: char) =
    match c with
    | '#' -> Some(Active)
    | '.' -> Some(Inactive)
    | _ -> None

let keys (m: Map<'Key, 'T>) =
    Map.fold (fun keys key _ -> key :: keys) [] m

let neighboursCoordinates3D (x, y, z) =
    seq {
        for i in -1 .. 1 do
            for j in -1 .. 1 do
                for k in -1 .. 1 do
                    let x' = x + i
                    let y' = y + j
                    let z' = z + k

                    if (x', y', z') <> (x, y, z) then yield (x', y', z')
    }

let neighboursCoordinates4D (x, y, z, w) =
    seq {
        for i in -1 .. 1 do
            for j in -1 .. 1 do
                for k in -1 .. 1 do
                    for l in -1 .. 1 do
                        let x' = x + i
                        let y' = y + j
                        let z' = z + k
                        let w' = w + l

                        if (x', y', z', w') <> (x, y, z, w) then yield (x', y', z', w')
    }

let adjacentActive3D coord (m: Map<int * int * int, State>) =
    coord
    |> neighboursCoordinates3D
    |> Seq.sumBy (fun coords ->
        match m.TryFind coords with
        | Some (Active) -> 1
        | _ -> 0)

let adjacentActive4D coord (m: Map<int * int * int * int, State>) =
    coord
    |> neighboursCoordinates4D
    |> Seq.sumBy (fun coords ->
        match m.TryFind coords with
        | Some (Active) -> 1
        | _ -> 0)

let pocketDimension3D (input: string array) =
    input
    |> Array.map (fun line -> line.ToCharArray() |> Array.choose fromChar)
    |> Seq.mapi (fun i -> Seq.mapi (fun j x -> (i, j, 0), x))
    |> Seq.concat
    |> Map.ofSeq

let pocketDimension4D (input: string array) =
    input
    |> Array.map (fun line -> line.ToCharArray() |> Array.choose fromChar)
    |> Seq.mapi (fun i -> Seq.mapi (fun j x -> (i, j, 0, 0), x))
    |> Seq.concat
    |> Map.ofSeq

let updateState3D coords (m: Map<int * int * int, State>) (m': Map<int * int * int, State>) =
    let activeNeighbours = m |> adjacentActive3D coords

    match m.TryFind coords with
    | Some (Active) ->
        if activeNeighbours = 2 || activeNeighbours = 3
        then m'
        else m' |> Map.remove coords
    | _ when activeNeighbours = 3 -> m' |> Map.add coords Active
    | _ -> m'

let updateState4D coords (m: Map<int * int * int * int, State>) (m': Map<int * int * int * int, State>) =
    let activeNeighbours = m |> adjacentActive4D coords

    match m.TryFind coords with
    | Some (Active) ->
        if activeNeighbours = 2 || activeNeighbours = 3
        then m'
        else m' |> Map.remove coords
    | _ when activeNeighbours = 3 -> m' |> Map.add coords Active
    | _ -> m' |> Map.add coords Inactive

let step3D (m: Map<int * int * int, State>) =
    let rec loop m' lst =
        match lst with
        | [] -> m'
        | (x :: xs) -> loop (updateState3D x m m') xs

    let visit =
        m
        |> keys
        |> List.map neighboursCoordinates3D
        |> List.fold (fun acc neighbours -> acc + (neighbours |> Set.ofSeq)) Set.empty

    loop m (visit |> Set.toList)

let step4D (m: Map<int * int * int * int, State>) =
    let rec loop m' lst =
        match lst with
        | [] -> m'
        | (x :: xs) -> loop (updateState4D x m m') xs

    let visit =
        m
        |> keys
        |> List.map neighboursCoordinates4D
        |> List.fold (fun acc neighbours -> acc + (neighbours |> Set.ofSeq)) Set.empty

    loop m (visit |> Set.toList)

let Part1 input =
    input
    |> pocketDimension3D
    |> step3D
    |> step3D
    |> step3D
    |> step3D
    |> step3D
    |> step3D
    |> Map.fold (fun acc _ value ->
        match value with
        | Active -> acc + 1
        | _ -> acc) 0

let Part2 input =
    input
    |> pocketDimension4D
    |> step4D
    |> step4D
    |> step4D
    |> step4D
    |> step4D
    |> step4D
    |> Map.fold (fun acc _ value ->
        match value with
        | Active -> acc + 1
        | _ -> acc) 0
