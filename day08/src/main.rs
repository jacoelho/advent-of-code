use std::fmt::Write;
use std::fs;

type Result<T> = ::std::result::Result<T, ::std::io::Error>;

fn main() -> Result<()> {
    println!("{:?}", part1(25, 6));

    println!("{:?}", part2(25, 6));

    Ok(())
}

fn part1(wide: usize, tall: usize) -> Result<usize> {
    let contents = fs::read_to_string("test_data/input.txt")?
        .split("")
        .filter_map(|s| s.parse::<usize>().ok())
        .collect::<Vec<_>>();

    let layers = contents.chunks(wide * tall).collect::<Vec<_>>();

    let zeros_count = layers
        .iter()
        .map(|layer| layer.iter().filter(|&v| *v == 0).count());

    let min_zero_layer = match zeros_count.enumerate().map(|(x, y)| (y, x)).min() {
        Some((_count, index)) => index,
        None => panic!("not found"),
    };

    let ones_count = layers[min_zero_layer].iter().filter(|&v| *v == 1).count();
    let twos_count = layers[min_zero_layer].iter().filter(|&v| *v == 2).count();

    Ok(ones_count * twos_count)
}

fn part2(wide: usize, tall: usize) -> Result<usize> {
    let contents = fs::read_to_string("test_data/input.txt")?
        .split("")
        .filter_map(|s| s.parse::<usize>().ok())
        .collect::<Vec<_>>();

    let layers = contents.chunks(wide * tall).collect::<Vec<_>>();

    let mut image = Vec::new();
    for idx in 0..(wide * tall) {
        image.push(pixel_colour(
            layers.iter().map(|layer| layer[idx]).collect::<Vec<_>>(),
        ));
    }

    for layer in image.chunks(wide).collect::<Vec<_>>() {
        println!("{:?}", join(layer));
    }

    Ok(1)
}

fn pixel_colour(pixels: Vec<usize>) -> char {
    for pixel in pixels {
        match pixel {
            0 => return ' ',
            1 => return '$',
            2 => continue,
            _ => panic!("unknown colour"),
        }
    }

    ' '
}

fn join<T: ::std::fmt::Display>(a: &[T]) -> String {
    a.iter().fold(String::new(), |mut s, n| {
        write!(s, "{} ", n).ok();
        s
    })
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn pixel_colour_test() {
        assert_eq!(pixel_colour(vec![0, 1, 2, 0]), 0);
        assert_eq!(pixel_colour(vec![2, 1, 2, 0]), 1);
        assert_eq!(pixel_colour(vec![2, 2, 1, 0]), 1);
        assert_eq!(pixel_colour(vec![2, 2, 2, 0]), 0);
    }
}
