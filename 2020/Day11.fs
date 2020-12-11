module Day11

let keys (m: Map<'Key, 'T>) =
    Map.fold (fun keys key _ -> key :: keys) [] m

let values (m: Map<'Key, 'T>) =
    Map.fold (fun values _ value -> value :: values) [] m

module Seat =
    type T =
        | Empty
        | Occupied
        | Floor

    let fromString str =
        match str with
        | '#' -> Some(Occupied)
        | 'L' -> Some(Empty)
        | '.' -> Some(Floor)
        | _ -> None

    let layout input =
        input
        |> Seq.mapi (fun i -> Seq.mapi (fun j x -> (i, j), fromString x))
        |> Seq.concat
        |> Seq.filter (fun (_, el) -> el.IsSome)
        |> Seq.map (fun (coords, el) -> (coords, Option.get el))
        |> Seq.toList
        |> Map.ofList


    let neighboursCoordinates (x, y) =
        seq {
            for i in -1 .. 1 do
                for j in -1 .. 1 do
                    let x' = x + i
                    let y' = y + j

                    if (x', y') <> (x, y) then yield (x', y')
        }


    let adjacentOccupied (x, y) (m: Map<int * int, T>) =
        (x, y)
        |> neighboursCoordinates
        |> Seq.fold (fun acc coords ->
            match m.TryFind coords with
            | Some (Occupied) -> Occupied :: acc
            | _ -> acc) List.empty

    let visibleOccupied (x, y) (m: Map<int * int, T>) =
        let rec loop (stepX, stepY) (x', y') =
            let updated = (x' + stepX, y' + stepY)

            match m.TryFind updated with
            | Some (Occupied) -> List.singleton Occupied
            | Some (Empty) -> []
            | Some (_) -> loop (stepX, stepY) updated
            | _ -> []

        [ (-1, 0)
          (1, 0)
          (0, -1)
          (0, 1)
          (-1, -1)
          (1, 1)
          (-1, 1)
          (1, -1) ]
        |> List.collect (fun step -> loop step (x, y))

    let rulesPart1 coords (m: Map<int * int, T>) (m': Map<int * int, T>) =
        match m.[coords] with
        | Floor -> m'
        | Empty ->
            if m |> adjacentOccupied coords |> Seq.isEmpty
            then m' |> Map.add coords Occupied
            else m'
        | Occupied ->
            if m |> adjacentOccupied coords |> Seq.length >= 4
            then m' |> Map.add coords Empty
            else m'

    let rulesPart2 coords (m: Map<int * int, T>) (m': Map<int * int, T>) =
        match m.[coords] with
        | Floor -> m'
        | Empty ->
            if m |> visibleOccupied coords |> Seq.isEmpty
            then m' |> Map.add coords Occupied
            else m'
        | Occupied ->
            if m |> visibleOccupied coords |> Seq.length >= 5
            then m' |> Map.add coords Empty
            else m'

    let step rules (m: Map<int * int, T>) =
        let rec loop m' lst =
            match lst with
            | [] -> m'
            | (x :: xs) -> loop (rules x m m') xs

        loop m (m |> keys)

    let print (m: Map<int * int, T>) =
        let rec loop row lst =
            match lst with
            | [] -> printfn ""
            | ((y, x) :: xs) ->
                if y > row then printfn ""
                printf
                    "%s"
                    (match m.[(y, x)] with
                     | Floor -> "."
                     | Empty -> "L"
                     | Occupied -> "#")
                loop y xs

        loop 0 (m |> keys |> List.sort)

    let countOccupied (m: Map<int * int, T>) =
        m
        |> values
        |> List.sumBy (fun el ->
            match el with
            | Occupied -> 1
            | _ -> 0)

    let untilStabilizes rules (m: Map<int * int, T>) =
        let rec loop steps m' =
            let res = m' |> step rules
            if res = m' then (steps, m' |> countOccupied, m') else loop (steps + 1) res

        loop 0 m

let toLayout (input: string array) =
    input
    |> Array.map (fun line -> line.ToCharArray())
    |> Seat.layout

let Part1 (input: string array) =
    let (_, occupied, _) =
        input
        |> toLayout
        |> Seat.untilStabilizes Seat.rulesPart1

    occupied

let Part2 (input: string array) =
    let (_, occupied, _) =
        input
        |> toLayout
        |> Seat.untilStabilizes Seat.rulesPart2

    occupied
