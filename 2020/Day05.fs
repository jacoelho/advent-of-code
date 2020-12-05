module Day05

open System.IO

module Direction =
    type T =
        | F
        | B
        | L
        | R

    let private fromChar ch =
        match ch with
        | 'F' -> Some F
        | 'B' -> Some B
        | 'L' -> Some L
        | 'R' -> Some R
        | _ -> None

    let fromString (str: string) =
        str.ToCharArray() |> Array.choose fromChar

let findSeat (seats: int seq) =
    Seq.fold (fun seats code ->
        let len = seats |> Seq.length

        match code with
        | Direction.F
        | Direction.L -> seats |> Seq.take (len >>> 1)
        | Direction.B
        | Direction.R -> seats |> Seq.skip (len >>> 1)) seats

let row seat =
    seat
    |> Array.toSeq
    |> Seq.take 7
    |> findSeat (seq { 0 .. 127 })
    |> Seq.head

let column seat =
    seat
    |> Array.toSeq
    |> Seq.skip 7
    |> findSeat (seq { 0 .. 7 })
    |> Seq.head

let seatID str =
    let directions = str |> Direction.fromString

    8 * (directions |> row) + (directions |> column)


let readInput file = file |> File.ReadAllLines

let Part1 file =
    file |> readInput |> Array.map seatID |> Array.max

let Part2 file =
    let seats = file |> readInput |> Array.map seatID |> Set.ofArray

    let allSeats =
        seq { (seats |> Set.minElement) .. (seats |> Set.maxElement) }
        |> Set.ofSeq

    Set.difference allSeats seats
    |> Set.toList
    |> List.head
