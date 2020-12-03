use std::collections::HashMap;
use std::collections::VecDeque;

pub fn number_to_vec(n: u32) -> Vec<u32> {
    let mut digits = VecDeque::new();
    let mut n = n;

    while n > 9 {
        digits.push_front(n % 10);
        n = n / 10;
    }

    digits.push_front(n);

    digits.into()
}

pub fn is_increasing_sequence(digits: &Vec<u32>) -> bool {
    digits
        .iter()
        .zip(digits.iter().skip(1))
        .all(|(x, y)| y >= x)
}

pub fn contains_consecutive(digits: &Vec<u32>) -> bool {
    digits
        .iter()
        .zip(digits.iter().skip(1))
        .any(|(x, y)| x == y)
}

pub fn contains_pair(digits: &Vec<u32>) -> bool {
    let frequency: HashMap<u32, u32> = digits
        .iter()
        .zip(digits.iter().skip(1))
        .filter(|(x, y)| x == y)
        .fold(HashMap::new(), |mut state, (a, _)| {
            let count = state.entry(*a).or_insert(0);
            *count += 1;

            state
        });

    frequency.values().any(|count| *count == 1)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_numbers() {
        assert_eq!(number_to_vec(0), vec![0]);
        assert_eq!(number_to_vec(1234), vec![1, 2, 3, 4]);
    }

    #[test]
    fn test_consecutive() {
        assert_eq!(contains_consecutive(&vec![1, 2, 3]), false);
        assert_eq!(contains_consecutive(&vec![1, 1, 1]), true);
        assert_eq!(contains_consecutive(&vec![1, 2, 3, 2]), false);
        assert_eq!(contains_consecutive(&vec![1, 2, 3, 3, 1, 2]), true);
    }

    #[test]
    fn test_increasing() {
        assert_eq!(is_increasing_sequence(&vec![1, 2, 3]), true);
        assert_eq!(is_increasing_sequence(&vec![1, 2, 1, 3]), false);
        assert_eq!(is_increasing_sequence(&vec![2, 2, 2]), true);
    }

    #[test]
    fn test_matching() {
        let digits = vec![1, 1, 2, 2, 3, 3];

        assert_eq!(contains_pair(&digits), true);
    }
}
