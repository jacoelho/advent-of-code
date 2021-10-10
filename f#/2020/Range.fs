module Range

type Range = int * int * int * int

let contains ((a, b, c, d): Range) (value: int) =
    (value >= a && value <= b)
    || (value >= c && value <= d)

let forall (s: seq<Range>) (v: int) =
    s
    |> Seq.forall (fun e -> contains e v)

let exists (s: seq<Range>) (v: int) =
    s
    |> Seq.exists (fun e -> contains e v)