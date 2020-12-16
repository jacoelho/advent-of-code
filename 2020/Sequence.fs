module Sequence

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