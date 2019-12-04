use day04;

fn main() {
    println!("part 1: {}", part_1(172_851, 675_869));
    println!("part 2: {}", part_2(172_851, 675_869));
}

fn part_1(initial: u32, end: u32) -> usize {
    (initial..=end)
        .into_iter()
        .filter(|element| {
            let digits = day04::number_to_vec(*element);

            day04::is_increasing_sequence(&digits) && day04::contains_consecutive(&digits)
        })
        .count()
}

fn part_2(initial: u32, end: u32) -> usize {
    (initial..=end)
        .into_iter()
        .filter(|element| {
            let digits = day04::number_to_vec(*element);

            day04::is_increasing_sequence(&digits) && day04::contains_pair(&digits)
        })
        .count()
}
