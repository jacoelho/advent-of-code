#[derive(Debug)]
enum Operation {
    Add,
    Mul,
    Stop,
}

impl From<usize> for Operation {
    fn from(item: usize) -> Self {
        match item {
            1 => Self::Add,
            2 => Self::Mul,
            99 => Self::Stop,
            v => panic!("invalid op: {}", v),
        }
    }
}

#[derive(Debug)]
pub struct IntCode {
    inner: Vec<usize>,
    cursor: usize,
}

impl IntCode {
    pub fn new(mut v: Vec<usize>) -> Self {
        // 1202 program alarm
        v[1] = 12;
        v[2] = 2;

        Self {
            inner: v,
            cursor: 0,
        }
    }

    pub fn new_with_noun(mut v: Vec<usize>, noun: usize, verb: usize) -> Self {
        v[1] = noun;
        v[2] = verb;

        Self {
            inner: v,
            cursor: 0,
        }
    }

    fn fetch_operation(&self) -> (Operation, usize, usize, usize) {
        let op = self.inner[self.cursor].into();

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
