use day07::intcode::{IntCode, IntCodeState};
use permutohedron::heap_recursive;
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    //let contents = "3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0";

    let values = contents
        .split(",")
        .filter_map(|s| s.parse().ok())
        .collect::<Vec<_>>();

    //println!("{:?}", part_1(&values));

    println!("part2: {:?}", part_2(&values));

    Ok(())
}

fn part_1(values: &Vec<i32>) -> Option<i32> {
    let mut data = [0, 1, 2, 3, 4];
    let mut possible = Vec::new();
    heap_recursive(&mut data, |permutation| possible.push(permutation.to_vec()));

    let mut solutions = possible
        .into_iter()
        .filter_map(|setting| run_simulation(setting, &values))
        .collect::<Vec<_>>();

    solutions.sort();

    solutions.last().cloned()
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

        for (idx, setting) in permutation.into_iter().enumerate() {
            amplifiers[idx].insert_input(setting);
        }

        amplifiers[0].insert_input(0);

        let mut idx = 0;
        let mut output = 0;
        'outer: loop {
            loop {
                match amplifiers[idx].process() {
                    IntCodeState::Stopped => {
                        if output > highest_output {
                            highest_output = output;
                        }

                        break 'outer;
                    }
                    IntCodeState::WaitingInput => amplifiers[idx].insert_input(output),
                    IntCodeState::Output(v) => {
                        output = v;
                        break;
                    }
                };
            }

            idx = (idx + 1) % 5;
        }
    }

    Some(highest_output)
}

fn run_simulation(sequence_vec: Vec<i32>, values: &Vec<i32>) -> Option<i32> {
    let mut input = 0;

    for setting in sequence_vec.into_iter() {
        let mut amplifier = IntCode::new_with_inputs(values.clone(), setting, input);

        match amplifier.until_stops() {
            Ok(v) => input = v,
            Err(e) => {
                println!("{}", e);
                return None;
            }
        }
    }

    Some(input)
}
