module Day14

let toString (chars: char seq) =
    string
        (Seq.fold (fun (sb: System.Text.StringBuilder) (c: char) -> sb.Append(c)) (System.Text.StringBuilder()) chars)

let values (m: Map<'Key, 'T>) =
    Map.fold (fun values _ value -> value :: values) [] m

let toBits (value: int64) =
    System.Convert.ToString(value, 2).PadLeft(36, '0')

let fromBits (bits: string) = System.Convert.ToInt64(bits, 2)

module Instruction =
    let fromArray (input: string array) =
        let rec loop mask result (lst: string list) =
            match lst with
            | [] -> result
            | (x :: xs) when x.Contains("mask") ->
                let mask = x.Split(" = ").[1]
                loop mask result xs
            | (x :: xs) ->
                let instruction = x.Split([| '['; ']'; ' ' |])
                loop
                    mask
                    ((instruction.[1] |> int64, mask, instruction.[4] |> int64 |> toBits)
                     :: result)
                    xs

        loop "" List.empty (input |> Array.toList)
        |> List.rev

    let apply mask value =
        Seq.zip mask value
        |> Seq.map (fun (m: char, b: char) ->
            match m with
            | 'X' -> b
            | _ -> m)
        |> toString

    let apply2 mask address =
        Seq.zip mask address
        |> Seq.map (fun (m: char, b: char) ->
            match m with
            | '0' -> b
            | '1' -> '1'
            | _ -> m)
        |> toString

let Part1 input =
    input
    |> Instruction.fromArray
    |> Seq.fold (fun memory (pos, mask, value) ->
        let result = Instruction.apply mask value
        memory |> Map.add pos result) Map.empty<int64, string>
    |> values
    |> List.sumBy fromBits

let replaceFloating replaces input =
    let rec loop replaces' lst result =
        match replaces', lst with
        | _, [] -> result
        | (y :: ys), (x :: xs) when x = 'X' -> loop ys xs (y :: result)
        | _, (x :: xs) -> loop replaces' xs (x :: result)

    loop replaces input List.empty |> List.rev

let floating input =
    let count =
        input |> (Array.filter ((=) 'X') >> Array.length)


    [ for i in 0 .. (pown 2 count) - 1 do
        let bits =
            System.Convert.ToString(i, 2).PadLeft(count, '0').ToCharArray()
            |> Array.toList

        yield
            (replaceFloating bits (input |> Array.toList)
             |> toString
             |> fromBits) ]

let Part2 input =
    input
    |> Instruction.fromArray
    |> Seq.fold (fun memory (pos, mask, value) ->
        let pos' = Instruction.apply2 mask (pos |> toBits)

        pos'.ToCharArray()
        |> floating
        |> List.fold (fun m el -> m |> Map.add el value) memory) Map.empty<int64, string>
    |> values
    |> List.sumBy fromBits
