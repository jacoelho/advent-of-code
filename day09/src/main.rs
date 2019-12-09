use day09::intcode::{IntCode, IntCodeState};
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    let values = contents
        .split(",")
        .filter_map(|s| s.parse().ok())
        .collect::<Vec<_>>();

    println!("part1: {:?}", part_1(&values));

    //println!("part2: {:?}", part_2(&values));

    Ok(())
}

fn part_1(values: &Vec<i64>) -> Option<i64> {
    let mut code = IntCode::new(values.clone());
    code.insert_input(1);

    match code.until_stops() {
        Ok(v) => Some(v),
        Err(_) => None,
    }
}

