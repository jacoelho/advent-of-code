module Day01

open System
open System.IO

let readInput file =
    file
    |> File.ReadAllLines
    |> Array.map Int32.Parse
    |> Array.toList

let rec pairs input =
    seq {
        match input with
        | x :: xs ->
            for e in xs do
                yield [ x; e ]
            yield! pairs xs
        | _ -> ()
    }

let rec triplets input =
    seq {
        match input with
        | x :: xs ->
            for pair in pairs (xs) do
                yield x :: pair
            yield! triplets xs
        | _ -> ()
    }

let day01 input combinationFun =
    input
    |> combinationFun
    |> Seq.filter (fun el -> List.sum el = 2020)
    |> Seq.head
    |> Seq.fold (*) 1

let day01Part1 input = day01 input pairs

let day01Part2 input = day01 input triplets
