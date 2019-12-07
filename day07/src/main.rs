use day07::intcode::{IntCode, IntCodeState};
use permutohedron::heap_recursive;
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
    let mut data = [0, 1, 2, 3, 4];
    let mut permutations = Vec::new();

    heap_recursive(&mut data, |permutation| {
        permutations.push(permutation.to_vec())
    });

    let mut highest_output = 0;

    for permutation in permutations {
        let mut amplifiers = [
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
        ];

        for idx in 0..5 {
            amplifiers[idx].insert_input(permutation[idx]);
        }

        let mut output = 0;
        for idx in 0..5 {
            amplifiers[idx].insert_input(output);

            match amplifiers[idx].until_stops() {
                Ok(v) => output = v,
                Err(_) => break,
            };
        }

        if output > highest_output {
            highest_output = output;
        }
    }

    Some(highest_output)
}

fn part_2(values: &Vec<i32>) -> Option<i32> {
    let mut data = [5, 6, 7, 8, 9];
    let mut permutations = Vec::new();

    heap_recursive(&mut data, |permutation| {
        permutations.push(permutation.to_vec())
    });

    let mut highest_output = 0;

    for permutation in permutations {
        let mut amplifiers = [
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
            IntCode::new(values.clone()),
        ];

        for idx in 0..5 {
            amplifiers[idx].insert_input(permutation[idx]);
        }

        let mut idx = 0;
        let mut output = 0;
        'feedback: loop {
            loop {
                match amplifiers[idx].process() {
                    IntCodeState::Stopped => break 'feedback,
                    IntCodeState::WaitingInput => amplifiers[idx].insert_input(output),
                    IntCodeState::Output(v) => {
                        output = v;
                        break;
                    }
                };
            }

            idx = (idx + 1) % 5;
        }

        if output > highest_output {
            highest_output = output;
        }
    }

    Some(highest_output)
}
