use std::fs;

type Result<T> = ::std::result::Result<T, ::std::io::Error>;

fn main() -> Result<()> {
    println!("{:?}", part1(25, 6));

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
        .map(|layer| layer.iter().filter(|&v| *v == 0).count())
        .collect::<Vec<_>>();

    let min_zero_layer = match zeros_count.iter().enumerate().map(|(x, y)| (y, x)).min() {
        Some((_count, index)) => index,
        None => panic!("not found"),
    };

    let ones_count = layers[min_zero_layer].iter().filter(|&v| *v == 1).count();
    let twos_count = layers[min_zero_layer].iter().filter(|&v| *v == 2).count();

    Ok(ones_count * twos_count)
}
