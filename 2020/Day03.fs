module Day03

open System.IO

let readInput file =
    file
    |> File.ReadAllLines
    |> Array.map (fun line -> line.ToCharArray())

let constrain limit value = value % limit

let isTree c = c = '#'

let navigate (initialX: int, initialY: int) (x: int, y: int) (maze: char [] []) =
    let constrainToWidth = maze.[0].Length |> constrain
    let length = maze.Length

    (initialX, initialY)
    |> Seq.unfold (fun (a, b) ->
        if b < length then
            let element = maze.[b].[a |> constrainToWidth]
            Some(element, (a + x, b + y))
        else
            None)

let arborealCount (x: int, y: int) maze =
    maze
    |> navigate (0, 0) (x, y)
    |> Seq.filter isTree
    |> Seq.length

let Part1 file =
    file |> readInput |> arborealCount (3, 1)

let Part2 file =
    let maze = file |> readInput

    let slopes =
        [| (1, 1)
           (3, 1)
           (5, 1)
           (7, 1)
           (1, 2) |]

    slopes
    |> Array.map (fun slope -> arborealCount slope maze)
    |> Array.reduce (*)
