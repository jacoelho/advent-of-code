module Day19

open Sequence

type Element =
    | Match of char
    | Reference of int

and Rules =
    | Rule of Element
    | Conjunction of Rules list
    | Disjunction of Rules * Rules

let rec fromString (str: string) =
    match str.Trim() with
    | v when v.Contains('"') ->
        let value = v.Split('"')
        value.[1] |> char |> Match |> Rule
    | v when v.Contains('|') ->
        let values = v.Split('|')
        Disjunction(fromString values.[0], fromString values.[1])
    | _ ->
        str.Split(' ')
        |> Array.filter ((<>) "")
        |> Array.map (int >> Reference >> Rule)
        |> Array.toList
        |> Conjunction

let check (rules: Map<int, Rules>) (str: string) =
    let rec loop (r: Rules) (chs: char list) =
        match r, chs with
        | Conjunction ([]), [] -> true
        | Conjunction ([]), _ -> false
        | Rule (Match (v)), [ c ] -> v = c
        | _, [] -> false
        | Rule (Reference (v)), x -> loop rules.[v] x
        | Disjunction (y, z), x -> loop y x || loop z x
        | Conjunction (Rule (Reference (y)) :: ys), c -> loop ((rules.[y]) :: ys |> Conjunction) c
        | Conjunction (Rule (Match (y)) :: ys), x :: xs -> if x = y then loop (ys |> Conjunction) xs else false
        | Conjunction (Conjunction (y) :: ys), x -> loop (Conjunction(y @ ys)) x
        | Conjunction (Disjunction (y, y') :: ys), x ->
            loop (Disjunction(Conjunction(y :: ys), Conjunction(y' :: ys))) x
        | x, y ->
            printfn "%A %A" x y
            failwithf "unexpected"

    loop rules.[0] (str.ToCharArray() |> Array.toList)


let tryInt (str: string) =
    match System.Int32.TryParse str with
    | true, int -> Some int
    | _ -> None

let rules (input: string seq) =
    input
    |> Seq.fold (fun acc (line: string) ->
        let elements = line.Split(':')

        acc
        |> Map.add (elements.[0] |> int) (elements.[1] |> fromString)) Map.empty<int, Rules>

let Part1 input =
    let input' =
        input
        |> Sequence.partitionBy System.String.IsNullOrEmpty
        |> Seq.toArray

    let m = input'.[0] |> rules

    input'.[1]
    |> Seq.filter (fun line -> check m line)
    |> Seq.length

let Part2 input =
    let input' =
        input
        |> Sequence.partitionBy System.String.IsNullOrEmpty
        |> Seq.toArray

    let m =
        input'.[0]
        @ [ "8: 42 | 42 8"
            "11: 42 31 | 42 11 31" ]
        |> rules

    input'.[1]
    |> Seq.filter (fun line -> check m line)
    |> Seq.length
