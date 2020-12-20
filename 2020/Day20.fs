module Day20

open System.Text.RegularExpressions

let toGrid (input: string seq) =
    input
    |> Seq.map (fun line -> line.ToCharArray())
    |> array2D

let toString m =
    let s =
        [ for rowIndex in [ 0 .. Array2D.length1 m - 1 ] do
            let row = m.[rowIndex, *]
            yield row |> Array.map string |> String.concat "" ]
        |> String.concat "\n"

    s + "\n"

let rotateGridClockwise grid =
    let height, width =
        Array2D.length1 grid, Array2D.length2 grid

    Array2D.init width height (fun row column -> Array2D.get grid (height - column - 1) row)

let flipGrid grid =
    let height, width =
        Array2D.length1 grid, Array2D.length2 grid

    Array2D.init height width (fun row column -> Array2D.get grid row (height - 1 - column))

let flipUpGrid grid =
    let height, width =
        Array2D.length1 grid, Array2D.length2 grid

    Array2D.init height width (fun row column -> Array2D.get grid (width - 1 - row) column)

let variations grid =
    [ 1 .. 3 ]
    |> List.fold (fun acc _ ->
        let h = List.head acc
        (h |> rotateGridClockwise) :: acc) [ grid ]
    |> List.collect (fun g -> [ g; (flipGrid g); (flipUpGrid g) ])

let fitLeft (m: char [,]) (m': char [,]) =
    let row = m.[*, 0]
    let row' = m'.[*, (m' |> Array2D.length2) - 1]

    row = row'

let fitRight (m: char [,]) (m': char [,]) = fitLeft m' m

let fitUp (m: char [,]) (m': char [,]) =
    let row = m.[0, *]
    let row' = m'.[(m' |> Array2D.length1) - 1, *]

    row = row'

let fitDown (m: char [,]) (m': char [,]) = fitUp m' m

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success
    then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let a =
    "#...##.#..
..#.#..#.#
.###....#.
###.##.##.
.###.#####
.##.#....#
#...######
.....#..##
#.####...#
#.##...##.".Split("\n")
    |> toGrid

let b =
    "..###..###
###...#.#.
..#....#..
.#.#.#..##
##...#.###
##.##.###.
####.#...#
#...##..#.
##..#.....
..##.#..#.".Split("\n")
    |> toGrid

let c =
    "#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...".Split("\n")
    |> toGrid

let parseInput input =
    input
    |> Sequence.partitionBy System.String.IsNullOrEmpty
    |> Seq.fold (fun acc el ->
        let h = List.head el

        match h with
        | Regex @"(\d+)" [ num  ] ->
            let tile = num |> int
            let grid = el |> List.skip 1 |> toGrid

            (tile,grid) :: acc
        | _ -> acc) List.empty
    |> Map.ofSeq

let Part1 input = input |> parseInput
