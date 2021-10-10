use std::collections::{HashMap, VecDeque};
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    let orbit_map = contents
        .split("\n")
        .map(|c| c.split(")").collect::<Vec<_>>());

    let orbits = orbit_map.fold(HashMap::new(), |mut state, route| {
        (*state.entry(route[0].to_string()).or_insert(Vec::new())).push(route[1].to_string());
        (*state.entry(route[1].to_string()).or_insert(Vec::new())).push(route[0].to_string());

        state
    });

    println!("{:?}", part1(&orbits));

    println!("{:?}", part2(&orbits));

    Ok(())
}

fn part1(orbits: &HashMap<String, Vec<String>>) -> usize {
    traverse_orbits("COM", &orbits).values().sum::<usize>()
}

fn part2(orbits: &HashMap<String, Vec<String>>) -> Option<usize> {
    match traverse_orbits("YOU", &orbits).get("SAN") {
        Some(v) => Some(v - 2),
        None => None,
    }
}

fn traverse_orbits<'a>(
    start: &'a str,
    orbits: &'a HashMap<String, Vec<String>>,
) -> HashMap<&'a str, usize> {
    let mut to_visit = VecDeque::new();
    let mut visited: HashMap<&str, usize> = HashMap::new();

    to_visit.push_back((start, 0));

    while !to_visit.is_empty() {
        let (orbit, distance) = match to_visit.pop_back() {
            Some((a, b)) => (a, b),
            None => break,
        };

        if visited.contains_key(orbit) {
            continue;
        }

        visited.insert(orbit, distance);

        if let Some(children) = orbits.get(orbit) {
            for child in children {
                to_visit.push_back((child, distance + 1));
            }
        }
    }

    visited
}
