use std::convert::TryFrom;

#[derive(Debug)]
pub enum Direction {
    Up,
    Down,
    Left,
    Right,
}

impl TryFrom<char> for Direction {
    type Error = &'static str;

    fn try_from(code: char) -> Result<Self, Self::Error> {
        match code {
            'U' => Ok(Self::Up),
            'D' => Ok(Self::Down),
            'L' => Ok(Self::Left),
            'R' => Ok(Self::Right),
            _ => Err("invalid direction"),
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parse_direction() {
        match Direction::try_from('D') {
            Ok(Direction::Down) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Direction::try_from('U') {
            Ok(Direction::Up) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Direction::try_from('L') {
            Ok(Direction::Left) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Direction::try_from('R') {
            Ok(Direction::Right) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Direction::try_from('C') {
            Err(_) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }
    }
}
