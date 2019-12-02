#[derive(Debug)]
pub enum Operation {
    Add,
    Mul,
    Stop,
}

#[derive(Debug)]
pub struct IntCode {
    inner: Vec<usize>,
    cursor: usize,
}

impl IntCode {
    pub fn new(mut v: Vec<usize>) -> Self {
        //1202 program alarm
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
        let op = match self.inner[self.cursor] {
            1 => Operation::Add,
            2 => Operation::Mul,
            99 => Operation::Stop,
            v => panic!("invalid op: {}", v),
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
        let (op, lhs, rhs, pos) = self.fetch_operation();

        match op {
            Operation::Add => self.inner[pos] = lhs + rhs,
            Operation::Mul => self.inner[pos] = lhs * rhs,
            Operation::Stop => return None,
        };

        self.cursor += 4;

        Some(self.inner[0])
    }
}
