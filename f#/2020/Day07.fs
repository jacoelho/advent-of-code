module Day07

open System.IO
open System.Text.RegularExpressions

let lineRegex =
    Regex "^(?<bag>[\w ]+) bags contain (?<rest>.*)\.$"

let qtyBagRegex = Regex "(?<qty>\d+) (?<bag>[\w ]+) bag"

let parseQuantityBag line =
    let matches = qtyBagRegex.Match line
    if matches.Success then
        let qty = matches.Groups.["qty"].Value |> int
        let bag = matches.Groups.["bag"].Value
        Some(qty, bag)
    else
        None

let parseBagSpecification line =
    let matches = lineRegex.Match line
    if matches.Success then
        let bag = matches.Groups.["bag"].Value

        let rest =
            matches.Groups.["rest"].Value.Split(",")
            |> Array.choose parseQuantityBag
            |> Set.ofArray

        Some(bag, rest)
    else
        None

let containsBagColor color m =
    m
    |> Map.filter (fun _ bag -> bag |> Set.exists (fun (_, c) -> c = color))
    |> Map.toSeq
    |> Seq.map fst

let rec containedOnBags color m =
    let bags = m |> containsBagColor color |> Set.ofSeq

    if bags.IsEmpty then
        bags
    else
        bags
        |> Set.fold (fun acc color ->
            acc
            + Set.ofList [ color ]
            + containedOnBags color m) Set.empty

let rec countNestedBags bagColor (m: Map<'a, Set<int * 'a>>) =
    let bags = m.[bagColor]
    if bags.IsEmpty then
        0
    else
        bags
        |> Set.fold (fun acc (qty, color) -> acc + qty + (qty * (countNestedBags color m))) 0


let readInput file = file |> File.ReadAllLines

let countParents s =
    s
    |> Array.choose parseBagSpecification
    |> Map.ofArray
    |> containedOnBags "shiny gold"
    |> Set.count

let calculateNested s =
    s
    |> Array.choose parseBagSpecification
    |> Map.ofArray
    |> countNestedBags "shiny gold"

let Part1 file = file |> readInput |> countParents

let Part2 file = file |> readInput |> calculateNested
