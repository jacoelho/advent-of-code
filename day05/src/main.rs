use day05::IntCode;
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    let values = contents
        .split(",")
        .filter_map(|s| s.parse().ok())
        .collect::<Vec<_>>();

    println!("part1: {:?}", part_1(&values));

    println!("part2: {:?}", part_2(&values));

    Ok(())
}

fn part_1(values: &Vec<i32>) -> Option<i32> {
    let code = IntCode::new(values.clone(), 1);

    code.into_iter().collect::<Vec<_>>().last().cloned()
}

fn part_2(values: &Vec<i32>) -> Option<i32> {
    let code = IntCode::new(values.clone(), 5);

    code.into_iter().collect::<Vec<_>>().last().cloned()
}
