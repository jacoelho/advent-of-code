use std::convert::TryFrom;
use std::fmt;

#[derive(Debug)]
enum Operation {
    Add,
    Mul,
    Stop,
}

#[derive(Debug)]
struct TryFromOperationError(usize);

impl fmt::Display for TryFromOperationError {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        write!(f, "invalid operation: {}", self.0)
    }
}

impl TryFrom<usize> for Operation {
    type Error = TryFromOperationError;

    fn try_from(item: usize) -> Result<Self, Self::Error> {
        match item {
            1 => Ok(Self::Add),
            2 => Ok(Self::Mul),
            99 => Ok(Self::Stop),
            v => Err(TryFromOperationError(v)),
        }
    }
}

#[derive(Debug)]
pub struct IntCode {
    inner: Vec<usize>,
    cursor: usize,
}

impl IntCode {
    pub fn new(v: Vec<usize>) -> Self {
        // 1202 program alarm
        Self::new_with_noun_and_verb(v, 12, 2)
    }

    pub fn new_with_noun_and_verb(mut v: Vec<usize>, noun: usize, verb: usize) -> Self {
        v[1] = noun;
        v[2] = verb;

        Self {
            inner: v,
            cursor: 0,
        }
    }

    fn fetch_operation(&self) -> (Operation, usize, usize, usize) {
        let op = match Operation::try_from(self.inner[self.cursor]) {
            Ok(v) => v,
            Err(err) => panic!(err),
        };

        if let Operation::Stop = op {
            return (op, 0, 0, 0);
        }

        let lhs = self.inner[self.inner[self.cursor + 1]];
        let rhs = self.inner[self.inner[self.cursor + 2]];
        let pos = self.inner[self.cursor + 3];

        (op, lhs, rhs, pos)
    }
}

impl Iterator for IntCode {
    type Item = usize;

    fn next(&mut self) -> Option<Self::Item> {
        match self.fetch_operation() {
            (Operation::Add, lhs, rhs, pos) => self.inner[pos] = lhs + rhs,
            (Operation::Mul, lhs, rhs, pos) => self.inner[pos] = lhs * rhs,
            (Operation::Stop, _, _, _) => return None,
        };

        self.cursor += 4;

        Some(self.inner[0])
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_try_from() {
        match Operation::try_from(1) {
            Ok(Operation::Add) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Operation::try_from(2) {
            Ok(Operation::Mul) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Operation::try_from(99) {
            Ok(Operation::Stop) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Operation::try_from(5) {
            Err(TryFromOperationError(5)) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }
    }
}
