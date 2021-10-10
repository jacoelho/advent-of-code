module Day12

module Action =
    type T =
        | North of int
        | South of int
        | East of int
        | West of int
        | Left of int
        | Right of int
        | Forward of int

    let fromString (str: string) =
        let action = str.[0]
        let value = str.[1..] |> int

        match action with
        | 'N' -> North(value) |> Some
        | 'S' -> South(value) |> Some
        | 'E' -> East(value) |> Some
        | 'W' -> West(value) |> Some
        | 'L' -> Left(value) |> Some
        | 'R' -> Right(value) |> Some
        | 'F' -> Forward(value) |> Some
        | _ -> None

module Longitude =
    type T =
        | North of int
        | South of int

    let fromInt i =
        if i < 0 then South(abs (i)) else North(i)

    let toInt log =
        match log with
        | North (v) -> v
        | South (v) -> -v

    let add a b = (a |> toInt) + (b |> toInt) |> fromInt

    let times factor c =
        match c with
        | North (v) -> North(v * factor)
        | South (v) -> South(v * factor)

module Latitude =
    type T =
        | East of int
        | West of int

    let fromInt i = if i < 0 then West(abs (i)) else East(i)

    let toInt lat =
        match lat with
        | East (v) -> v
        | West (v) -> -v

    let add a b = (a |> toInt) + (b |> toInt) |> fromInt

    let times factor c =
        match c with
        | East (v) -> East(v * factor)
        | West (v) -> West(v * factor)

module Direction =
    type T =
        | North
        | South
        | East
        | West

    let rotate d angle =
        match d, angle with
        | North, 90
        | North, -270 -> East
        | North, 180
        | North, -180 -> South
        | North, 270
        | North, -90 -> West
        | South, 90
        | South, -270 -> West
        | South, 180
        | South, -180 -> North
        | South, 270
        | South, -90 -> East
        | East, 90
        | East, -270 -> South
        | East, 180
        | East, -180 -> West
        | East, 270
        | East, -90 -> North
        | West, 90
        | West, -270 -> North
        | West, 180
        | West, -180 -> East
        | West, 270
        | West, -90 -> South
        | a, b -> failwithf "unexpected %A %A" a b

module Navigate =
    type Coordinate =
        { Longitude: Longitude.T
          Latitude: Latitude.T }

    let addLongitude c log =
        let { Longitude = logitude } = c

        { c with
              Longitude = Longitude.add logitude log }

    let addLatitude c lat =
        let { Latitude = latitude } = c

        { c with
              Latitude = Latitude.add latitude lat }

    let manhattanDistance c =
        (c.Latitude |> Latitude.toInt |> abs)
        + (c.Longitude |> Longitude.toInt |> abs)

    let take (action: Action.T) (direction: Direction.T, coord: Coordinate) =
        match action, direction, coord with
        | Action.Forward (value), Direction.North, _ -> (direction, addLongitude coord (value |> Longitude.North))
        | Action.Forward (value), Direction.South, _ -> (direction, addLongitude coord (value |> Longitude.South))
        | Action.Forward (value), Direction.East, _ -> (direction, addLatitude coord (value |> Latitude.East))
        | Action.Forward (value), Direction.West, _ -> (direction, addLatitude coord (value |> Latitude.West))
        | Action.North (value), _, _ -> (direction, addLongitude coord (value |> Longitude.North))
        | Action.South (value), _, _ -> (direction, addLongitude coord (value |> Longitude.South))
        | Action.East (value), _, _ -> (direction, addLatitude coord (value |> Latitude.East))
        | Action.West (value), _, _ -> (direction, addLatitude coord (value |> Latitude.West))
        | Action.Left (value), _, _ -> (Direction.rotate direction -value, coord)
        | Action.Right (value), _, _ -> (Direction.rotate direction value, coord)

    let rotate rotation (c: Coordinate) =
        let (x, y) =
            (c.Latitude |> Latitude.toInt, c.Longitude |> Longitude.toInt)

        let (x', y') =
            match rotation with
            | 90
            | -270 -> (y, -x)
            | 180
            | -180 -> (-x, -y)
            | -90
            | 270 -> (-y, x)
            | _ -> (x, y)

        { c with
              Longitude = y' |> Longitude.fromInt
              Latitude = x' |> Latitude.fromInt }

    let take2 (a: Action.T) (waypoint: Coordinate, ship: Coordinate) =
        let { Latitude = wayLat; Longitude = wayLog } = waypoint

        match a with
        | Action.Forward (value) ->
            let { Latitude = lat; Longitude = log } = ship
            (waypoint,
             { ship with
                   Latitude = (Latitude.add (wayLat |> Latitude.times value) lat)
                   Longitude = (Longitude.add (wayLog |> Longitude.times value) log) })
        | Action.North (value) -> (addLongitude waypoint (value |> Longitude.North), ship)
        | Action.South (value) -> (addLongitude waypoint (value |> Longitude.South), ship)
        | Action.West (value) -> (addLatitude waypoint (value |> Latitude.West), ship)
        | Action.East (value) -> (addLatitude waypoint (value |> Latitude.East), ship)
        | Action.Left (value) -> ((waypoint |> rotate -value), ship)
        | Action.Right (value) -> ((waypoint |> rotate value), ship)

let Part1 input =
    input
    |> Array.choose Action.fromString
    |> Array.fold (fun coord action -> coord |> Navigate.take action)
           (Direction.East,
            { Longitude = (0 |> Longitude.North)
              Latitude = (0 |> Latitude.East) })
    |> snd
    |> Navigate.manhattanDistance

let Part2 input =
    input
    |> Array.choose Action.fromString
    |> Array.fold (fun coord action -> coord |> Navigate.take2 action)
           ({ Longitude = (1 |> Longitude.North)
              Latitude = (10 |> Latitude.East) },
            { Longitude = (0 |> Longitude.North)
              Latitude = (0 |> Latitude.East) })
    |> snd
    |> Navigate.manhattanDistance
