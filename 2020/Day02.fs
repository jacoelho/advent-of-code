module Day02

open System
open System.IO

let policyToTuple (str: string) =
    let parts = str.Split("-") |> Array.map Int32.Parse
    (parts.[0], parts.[1])

let inSequence (a: int, b: int) = fun x -> x >= a && x <= b

let inPos (a: int, b: int) =
    fun (x: string) -> (fun c -> (x.[a - 1] = c) <> (x.[b - 1] = c))

let charCount (char: char) = Seq.filter ((=) char) >> Seq.length

let readInput file = file |> File.ReadAllLines |> Array.toSeq

let verifyPasswordWithCount (input: string) =
    let parts = input.Split " "
    let policy = parts.[0] |> policyToTuple |> inSequence
    let letter = parts.[1].[0]

    parts.[2] |> charCount letter |> policy

let verifyPasswordCharInPos (input: string) =
    let parts = input.Split " "
    let policy = parts.[0] |> policyToTuple |> inPos
    let letter = parts.[1].[0]

    (parts.[2] |> policy) letter

let Part1 file =
    file
    |> readInput
    |> Seq.map verifyPasswordWithCount
    |> Seq.filter id
    |> Seq.length

let Part2 file =
    file
    |> readInput
    |> Seq.map verifyPasswordCharInPos
    |> Seq.filter id
    |> Seq.length
