module Day11

let keys (m: Map<'Key, 'T>) =
    Map.fold (fun keys key _ -> key :: keys) [] m

let values (m: Map<'Key, 'T>) =
    Map.fold (fun values _ value -> value :: values) [] m

let neighboursCoordinates (x, y) =
    seq {
        for i in -1 .. 1 do
            for j in -1 .. 1 do
                let x' = x + i
                let y' = y + j

                if (x', y') <> (x, y) then yield (x', y')
    }

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

    let adjacentOccupied (x, y) (m: Map<int * int, T>) =
        (x, y)
        |> neighboursCoordinates
        |> Seq.fold (fun acc coords ->
            match m.TryFind coords with
            | Some (Occupied) -> Occupied :: acc
            | _ -> acc) List.empty

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

    let step rules (m: Map<int * int, T>)  =
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

    let untilStabilizes rules (m: Map<int * int, T>)  =
        let rec loop steps m' =
            let res = m' |> step rules
            if res = m' then (steps, m' |> countOccupied, m') else loop (steps + 1) res

        loop 0 m

let step0 = "L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL"

let step1 = "#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##"

let step2 = "#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##"

let step3 = "#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##"

let step4 = "#.#L.L#.##
#LLL#LL.L#
L.L.L..#..
#LLL.##.L#
#.LL.LL.LL
#.LL#L#.##
..L.L.....
#L#LLLL#L#
#.LLLLLL.L
#.#L#L#.##"

let step5 = "#.#L.L#.##
#LLL#LL.L#
L.#.L..#..
#L##.##.L#
#.#L.LL.LL
#.#L#L#.##
..L.L.....
#L#L##L#L#
#.LLLLLL.L
#.#L#L#.##"

let toLayout (input: string array) =
    input
    |> Array.map (fun line -> line.ToCharArray())
    |> Seat.layout

let step0Layout = step0.Split("\n") |> toLayout
let step1Layout = step1.Split("\n") |> toLayout
let step2Layout = step2.Split("\n") |> toLayout
let step3Layout = step3.Split("\n") |> toLayout
let step4Layout = step4.Split("\n") |> toLayout
let step5Layout = step5.Split("\n") |> toLayout

let foo = step0.Split("\n") |> toLayout

let Part1 (input: string array) =
    let (_, occupied, _) =
        input |> toLayout |> Seat.untilStabilizes Seat.rulesPart1

    occupied
