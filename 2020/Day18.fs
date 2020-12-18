module Day18

open FParsec

let ws = spaces
let str_ws s = pstring s >>. ws
let number = pint64 .>> ws

let calculate parser s =
    match run parser s with
    | Success (v, _, _) -> v
    | Failure (msg, err, _) ->
        printf "%s" msg
        failwith msg

let calculatorPart1 =
    let opp =
        new OperatorPrecedenceParser<int64, unit, unit>()

    let expr = opp.ExpressionParser
    opp.TermParser <- number <|> between (str_ws "(") (str_ws ")") expr

    opp.AddOperator(InfixOperator("+", ws, 1, Associativity.Left, (+)))
    opp.AddOperator(InfixOperator("*", ws, 1, Associativity.Left, (*)))

    calculate (ws >>. expr .>> eof)

let calculatorPart2 =
    let opp =
        new OperatorPrecedenceParser<int64, unit, unit>()

    let expr = opp.ExpressionParser
    opp.TermParser <- number <|> between (str_ws "(") (str_ws ")") expr

    opp.AddOperator(InfixOperator("+", ws, 2, Associativity.Left, (+)))
    opp.AddOperator(InfixOperator("*", ws, 1, Associativity.Left, (*)))

    calculate (ws >>. expr .>> eof)


let Part1 (input: string array) = input |> Array.sumBy calculatorPart1

let Part2 (input: string array) = input |> Array.sumBy calculatorPart2
