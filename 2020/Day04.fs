module Day04

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

let tryParseInt (s: string) =
    match Int32.TryParse s with
    | true, v -> Some v
    | false, _ -> None

module Height =
    type T =
        | Cm of int
        | In of int

    let fromString (s: string) =
        if s.Length < 2 then
            None
        else
            let numeric = tryParseInt s.[..s.Length - 3]

            if s.EndsWith("cm") then
                match numeric with
                | Some (v) when v >= 150 && v <= 193 -> Some(Cm(v))
                | _ -> None
            elif s.EndsWith("in") then
                match numeric with
                | Some (v) when v >= 59 && v <= 76 -> Some(In(v))
                | _ -> None
            else
                None

module EyeColor =
    type T =
        | Amb
        | Blu
        | Brn
        | Gry
        | Grn
        | Hzl
        | Oth

    let fromString (s: string) =
        match s with
        | "amb" -> Some(Amb)
        | "blu" -> Some(Blu)
        | "brn" -> Some(Brn)
        | "gry" -> Some(Gry)
        | "grn" -> Some(Grn)
        | "hzl" -> Some(Hzl)
        | "oth" -> Some(Oth)
        | _ -> None

module HairColor =
    open System.Text.RegularExpressions

    type T = Hexadecimal of string

    let private re = Regex "^#[0-9a-f]{6}$"

    let fromString (s: string) =
        if re.IsMatch s then Some(Hexadecimal s) else None

module BirthYear =
    type T = Year of int

    let fromString (s: string) =
        match tryParseInt s with
        | Some v when v >= 1920 && v <= 2002 -> Some(Year(v))
        | _ -> None

module IssueYear =
    type T = Year of int

    let fromString (s: string) =
        match tryParseInt s with
        | Some v when v >= 2010 && v <= 2020 -> Some(Year(v))
        | _ -> None

module ExpirationYear =
    type T = Year of int

    let fromString (s: string) =
        match tryParseInt s with
        | Some v when v >= 2020 && v <= 2030 -> Some(Year(v))
        | _ -> None

module Passport =
    type T =
        { BirthYear: BirthYear.T option
          IssueYear: IssueYear.T option
          ExpirationYear: ExpirationYear.T option
          Height: Height.T option
          HairColor: HairColor.T option
          EyeColor: EyeColor.T option
          ID: string option
          CountryID: string option }

    let empty =
        { BirthYear = None
          IssueYear = None
          ExpirationYear = None
          Height = None
          HairColor = None
          EyeColor = None
          ID = None
          CountryID = None }

    let private idFromString (s: string) =
        match s with
        | v when v |> Seq.forall Char.IsDigit && s.Length = 9 -> Some(v)
        | _ -> None

    let isValid p =
        match p with
        | { T.BirthYear = Some (_); T.IssueYear = Some (_); T.ExpirationYear = Some (_); T.Height = Some (_);
            T.HairColor = Some (_); T.EyeColor = Some (_); T.ID = Some (_) } -> true
        | _ -> false

    let fromString (str: string) =
        let segments =
            str.Split(" ")
            |> Array.map (fun line -> line.Split(":"))

        (empty, segments)
        ||> Array.fold (fun passport el ->
                match el with
                | [| "byr"; value |] ->
                    { passport with
                          BirthYear = value |> BirthYear.fromString }
                | [| "iyr"; value |] ->
                    { passport with
                          IssueYear = value |> IssueYear.fromString }
                | [| "eyr"; value |] ->
                    { passport with
                          ExpirationYear = value |> ExpirationYear.fromString }
                | [| "hgt"; value |] ->
                    { passport with
                          Height = value |> Height.fromString }
                | [| "hcl"; value |] ->
                    { passport with
                          HairColor = value |> HairColor.fromString }
                | [| "ecl"; value |] ->
                    { passport with
                          EyeColor = value |> EyeColor.fromString }
                | [| "pid"; value |] ->
                    { passport with
                          ID = value |> idFromString }
                | [| "cid"; value |] ->
                    { passport with
                          CountryID = Some(value) }
                | _ -> passport)

let processData input =
    input
    |> partitionBy String.IsNullOrEmpty
    |> Seq.map
        ((fun el -> el |> String.concat " ")
         >> Passport.fromString)
    |> Seq.filter Passport.isValid

let readInput file = file |> File.ReadAllLines

let Part1 file =
    file |> readInput |> processData |> Seq.length
