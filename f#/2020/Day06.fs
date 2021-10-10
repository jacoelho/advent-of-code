module Day06

open System
open System.IO

let partitionBy (func: 'T -> bool) (sequence: 'T seq): 'T list seq =
    seq {
        use en = sequence.GetEnumerator()
        let more = ref true
        while !more do
            let wasGood = ref true

            let sublist =
                [ while !wasGood && en.MoveNext() do
                    if not (func en.Current) then yield en.Current else wasGood := false ]

            if List.isEmpty sublist then more := false else yield sublist
    }

let readInput file = file |> File.ReadAllLines

let ToCharArray (s: string): char [] = s.ToCharArray()

let Part1 file =
    file
    |> readInput
    |> partitionBy String.IsNullOrEmpty
    |> Seq.map (fun group ->
        group
        |> Seq.map (ToCharArray >> Set.ofArray)
        |> Seq.reduce (+))
    |> Seq.sumBy Set.count

let Part2 file =
    file
    |> readInput
    |> partitionBy String.IsNullOrEmpty
    |> Seq.map (fun group ->
        group
        |> Seq.map (ToCharArray >> Set.ofArray)
        |> Seq.reduce Set.intersect)
    |> Seq.sumBy Set.count
