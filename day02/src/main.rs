use day02::IntCode;
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    let values = contents
        .split(",")
        .map(|v| v.parse().unwrap())
        .collect::<Vec<_>>();

    println!("part1: {:?}", part_1(&values));

    println!("part2: {:?}", part_2(&values));

    Ok(())
}

fn part_1(values: &Vec<usize>) -> Option<usize> {
    let code = IntCode::new(values.clone());

    code.into_iter().collect::<Vec<_>>().last().cloned()
}

fn part_2(values: &Vec<usize>) -> Option<usize> {
    for noun in 0..100 {
        for verb in 0..100 {
            if let Some(19690720) = IntCode::new_with_noun_and_verb(values.clone(), noun, verb)
                .into_iter()
                .collect::<Vec<_>>()
                .last()
            {
                return Some(100 * noun + verb);
            }
        }
    }

    None
}
