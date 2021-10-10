use crate::direction::Direction;
use std::convert::TryFrom;

#[derive(Debug)]
pub struct Wire {
    direction: Direction,
    length: f32,
}

impl Wire {
    pub fn new(direction: Direction, length: f32) -> Self {
        Self { direction, length }
    }

    pub fn len(&self) -> f32 {
        self.length
    }

    pub fn direction(&self) -> &Direction {
        &self.direction
    }
}

impl std::str::FromStr for Wire {
    type Err = String;

    fn from_str(code: &str) -> Result<Self, Self::Err> {
        let direction = match code.chars().nth(0) {
            Some(v) => Direction::try_from(v)?,
            None => return Err("no direction found".to_string()),
        };

        let length: f32 = match &code[1..].parse() {
            Ok(v) => *v,
            Err(e) => return Err(e.to_string()),
        };

        Ok(Self {
            direction: direction,
            length: length,
        })
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parse_wire() {
        match "U1213".parse::<Wire>() {
            Ok(Wire {
                direction: Direction::Up,
                length: 1213.0,
            }) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match "D0".parse::<Wire>() {
            Ok(Wire {
                direction: Direction::Down,
                length: 0.0,
            }) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match "L1231".parse::<Wire>() {
            Ok(Wire {
                direction: Direction::Left,
                length: 1231.0,
            }) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match "D132131".parse::<Wire>() {
            Ok(Wire {
                direction: Direction::Down,
                length: 132131.0,
            }) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match "132131".parse::<Wire>() {
            Err(_) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match "D".parse::<Wire>() {
            Err(_) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }
    }
}
