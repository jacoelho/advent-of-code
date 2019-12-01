use std::fs::File;
use std::io::{BufRead, BufReader, Error};

fn fuel_required(mass: u32) -> u32 {
    let fuel = mass / 3 - 2;

    if fuel > 5 {
        fuel + fuel_required(fuel)
    } else {
        fuel
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_14() {
        assert_eq!(fuel_required(14), 2);
    }

    #[test]
    fn test_1969() {
        assert_eq!(fuel_required(1969), 966);
    }

    #[test]
    fn test_100756() {
        assert_eq!(fuel_required(100756), 50346);
    }
}

fn main() -> Result<(), Error> {
    let file = File::open("test_data/input.txt")?;
    let reader = BufReader::new(file);

    let fuel: u32 = reader
        .lines()
        .map(|line| {
            if let Ok(value) = line {
                let mass: u32 = value.parse().unwrap();

                fuel_required(mass)
            } else {
                0
            }
        })
        .sum();

    println!("fuel required: {}", fuel);

    Ok(())
}
