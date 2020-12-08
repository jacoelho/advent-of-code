module Day08

open System.IO

module Instruction =
    type T =
        | Nop of int
        | Acc of int
        | Jmp of int

    let fromString (str: string) =
        match str.Split(" ") with
        | [| "nop"; value |] -> Some(Nop(value |> int))
        | [| "acc"; value |] -> Some(Acc(value |> int))
        | [| "jmp"; value |] -> Some(Jmp(value |> int))
        | _ -> None

type ConsoleResult =
    | Loop of int
    | Ok of int

let console (bootCode: Instruction.T array) =
    let rec loop acc ip seen =
        if (seen |> Set.contains ip) then
            Loop(acc)
        elif (ip >= bootCode.Length) then
            Ok(acc)
        else
            let updatedSeen = seen |> Set.add ip
            match bootCode.[ip] with
            | Instruction.Nop (_) -> loop acc (ip + 1) updatedSeen
            | Instruction.Acc (value) -> loop (acc + value) (ip + 1) updatedSeen
            | Instruction.Jmp (value) -> loop acc (ip + value) updatedSeen

    loop 0 0 Set.empty

let readInput file = file |> File.ReadAllLines

let createCodeVariation (idx: int) (op: Instruction.T) (code: Instruction.T array) =
    let cpy = code |> Array.copy
    cpy.[idx] <- op
    cpy

let switchCorrupted (op: Instruction.T) =
    match op with
    | Instruction.Nop (value) -> Instruction.Jmp(value)
    | Instruction.Jmp (value) -> Instruction.Nop(value)
    | _ -> op

let bruteForce (code: Instruction.T array) =
    seq {
        for codePos in 0 .. code.Length - 1 do
            match code.[codePos] with
            | Instruction.Acc (_) -> ()
            | instruction ->
                let tryCorrupted =
                    createCodeVariation codePos (instruction |> switchCorrupted) code
                    |> console

                match tryCorrupted with
                | Ok (value) -> yield Some(value)
                | Loop (_) -> ()
    }

let Part1 file =
    file
    |> readInput
    |> Array.choose Instruction.fromString
    |> console

let Part2 file =
    file
    |> readInput
    |> Array.choose Instruction.fromString
    |> bruteForce
    |> Seq.choose id
    |> Seq.head
