use std::fs;

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

fn main() -> Result<(), std::io::Error> {
    let fuel: u32 = fs::read_to_string("test_data/input.txt")?
        .lines()
        .map(|mass| {
            let mass: u32 = mass.parse().unwrap();

            fuel_required(mass)
        })
        .sum();

    println!("fuel required: {}", fuel);

    Ok(())
}
