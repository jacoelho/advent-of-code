use day07::intcode::IntCode;
use permutohedron::heap_recursive;
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    let values = contents
        .split(",")
        .filter_map(|s| s.parse().ok())
        .collect::<Vec<_>>();

    println!("{:?}", part_1(&values));

    //println!("part2: {:?}", part_2(&values));

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
    let mut possible = Vec::new();
    heap_recursive(&mut data, |permutation| possible.push(permutation.to_vec()));

    let mut solutions = possible
        .into_iter()
        .filter_map(|setting| run_simulation(setting, &values))
        .collect::<Vec<_>>();

    solutions.sort();

    solutions.last().cloned()
}

fn run_simulation(sequence_vec: Vec<i32>, values: &Vec<i32>) -> Option<i32> {
    let mut input = 0;

    for setting in sequence_vec.into_iter() {
        let amplifier = IntCode::new(values.clone(), setting, input)
            .into_iter()
            .collect::<Vec<_>>()
            .last()
            .cloned();

        match amplifier {
            Some(output) => input = output,
            None => return None,
        }
    }

    Some(input)
}
