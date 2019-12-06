use std::collections::HashMap;
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    let orbit_map = contents
        .split("\n")
        .map(|c| c.split(")").collect::<Vec<_>>());

    let orbits = orbit_map.fold(HashMap::new(), |mut state, route| {
        let child = state.entry(route[0].to_string()).or_insert(Vec::new());
        child.push(route[1].to_string());

        state
    });

    println!("{:?}", part_1("COM".to_string(), 0, &orbits));

    Ok(())
}

fn part_1(orbit: String, weight: usize, h: &HashMap<String, Vec<String>>) -> usize {
    match h.get(&orbit) {
        None => weight,
        Some(v) => {
            let mut sum = weight;

            for child in v.iter() {
                sum += part_1(child.to_string(), weight + 1, h)
            }

            sum
        }
    }
}
