module Day16

open Range

let values (m: Map<'Key, 'T>) =
    Map.fold (fun values _ value -> value :: values) [] m

let processData input =
    input
    |> Sequence.partitionBy System.String.IsNullOrEmpty

let parseSeatLine (str: string) =
    let intervalRe =
        System.Text.RegularExpressions.Regex "^(?<label>[\w ]+): (?<a>\d+)-(?<b>\d+) or (?<c>\d+)-(?<d>\d+)$"

    let matches = intervalRe.Match str
    if matches.Success then
        let label = matches.Groups.["label"].Value
        let a = matches.Groups.["a"].Value |> int
        let b = matches.Groups.["b"].Value |> int
        let c = matches.Groups.["c"].Value |> int
        let d = matches.Groups.["d"].Value |> int

        Some((label, Range(a, b, c, d)))
    else
        None

let parseConstraints input =
    input |> Seq.choose parseSeatLine |> Map.ofSeq

let parseTicket (input: string seq) =
    let ticket = input |> Seq.skip 1 |> Seq.head

    ticket.Split(',') |> Array.map int

let parseNearby (input: string seq) =
    input
    |> Seq.skip 1
    |> Seq.map (fun el -> el.Split(',') |> Array.map int)

let parseInput input =
    let values = input |> processData |> Seq.toArray

    (values.[0] |> parseConstraints, values.[1] |> parseTicket, values.[2] |> parseNearby)

let Part1 input =
    let (contraints, _, nearby) = input |> parseInput

    let contraints' = contraints |> values |> Range.exists

    nearby
    |> Seq.fold Seq.append Seq.empty
    |> Seq.filter (contraints' >> not)
    |> Seq.sum

let validTickets constraints tickets =
    let contraints' = constraints |> values |> Range.exists

    tickets
    |> Seq.filter (fun el -> el |> Array.forall contraints')

let validForAll constraints (tickets: seq<int array>) =
    tickets
    |> Seq.forall (fun ticket ->
        ticket
        |> Array.forall ((fun el -> Range.exists constraints el)))

let validForPosition rule idx (tickets: seq<int array>) =
    tickets
    |> Seq.forall (fun ticket -> Range.contains rule ticket.[idx])

let possiblePositions pos (tickets: seq<int array>) rule =
    pos
    |> Seq.filter (fun i -> validForPosition rule i tickets)

let Part2 input =
    let (contraints, ticket, nearby) = input |> parseInput

    let tickets =
        validTickets contraints nearby
        |> Seq.append (Seq.singleton ticket)

    // remove the only column with 1 solution until no columns left works in this case
    let rec loop result positions rules =
        match positions with
        | [] -> result
        | _ ->
            let (ruleName, idx) =
                rules
                |> List.map (fun (ruleName, range) -> (ruleName, possiblePositions positions tickets range))
                |> Seq.filter (fun (_, el) -> el |> Seq.length = 1)
                |> Seq.map (fun (rule, el) -> (rule, el |> Seq.head))
                |> Seq.head

            loop ((ruleName, idx) :: result) (positions |> List.filter ((<>) idx)) rules

    let result =
        loop
            List.empty
            (seq { 0 .. (tickets |> Seq.head |> Array.length) - 1 }
             |> Seq.toList)
            (contraints |> Map.toList)

    result
    |> List.filter (fun (el, i) -> el.Contains("departure"))
    |> List.map (fun (_, i) -> ticket.[i] |> int64)
    |> List.reduce (*)
