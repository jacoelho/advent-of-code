use std::collections::VecDeque;
use std::convert::TryFrom;

#[derive(Debug, PartialEq)]
enum Mode {
    Position,
    Immediate,
    Relative,
}

impl TryFrom<i64> for Mode {
    type Error = String;

    fn try_from(item: i64) -> Result<Self, Self::Error> {
        match item {
            0 => Ok(Self::Position),
            1 => Ok(Self::Immediate),
            2 => Ok(Self::Relative),
            v => Err(format!("invalid operation: {}", v)),
        }
    }
}

#[derive(Debug, PartialEq)]
enum Opcode {
    Add,
    Mul,
    Stop,
    Input,
    Output,
    JumpIfTrue,
    JumpIfFalse,
    LessThan,
    Equals,
    RelativeBaseAdjust,
}

impl TryFrom<i64> for Opcode {
    type Error = String;

    fn try_from(item: i64) -> Result<Self, Self::Error> {
        match item {
            1 => Ok(Self::Add),
            2 => Ok(Self::Mul),
            3 => Ok(Self::Input),
            4 => Ok(Self::Output),
            5 => Ok(Self::JumpIfTrue),
            6 => Ok(Self::JumpIfFalse),
            7 => Ok(Self::LessThan),
            8 => Ok(Self::Equals),
            9 => Ok(Self::RelativeBaseAdjust),
            99 => Ok(Self::Stop),
            v => Err(format!("invalid opcode: {}", v)),
        }
    }
}

#[derive(Debug)]
struct Instruction {
    opcode: Opcode,
    parameter_one: Mode,
    parameter_two: Mode,
    parameter_three: Mode,
}

impl TryFrom<i64> for Instruction {
    type Error = String;

    fn try_from(item: i64) -> Result<Self, Self::Error> {
        let opcode = Opcode::try_from(item % 100)?;
        let parameter_one = Mode::try_from((item / 100) % 10)?;
        let parameter_two = Mode::try_from((item / 1_000) % 10)?;
        let parameter_three = Mode::try_from((item / 10_000) % 10)?;

        Ok(Self {
            opcode,
            parameter_one,
            parameter_two,
            parameter_three,
        })
    }
}

#[derive(Debug)]
pub struct IntCode {
    inner: Vec<i64>,
    instruction_point: usize,
    relative_point: i64,
    inputs: VecDeque<i64>,
}

#[derive(Debug)]
pub enum IntCodeState {
    Stopped,
    WaitingInput,
    Output(i64),
}

impl IntCode {
    pub fn new(v: Vec<i64>) -> Self {
        Self {
            inner: v,
            instruction_point: 0,
            relative_point: 0,
            inputs: VecDeque::new(),
        }
    }

    pub fn insert_input(&mut self, input: i64) {
        self.inputs.push_back(input);
    }

    fn fetch_position(&mut self, pos: usize) -> i64 {
        match self.inner.get(pos) {
            Some(v) => *v,
            None => {
                self.inner.resize(pos+1, 0);

                0
            }
        }
    }

    fn fetch_instruction(&mut self) -> (Opcode, i64, i64, usize) {
        let instruction = match Instruction::try_from(self.inner[self.instruction_point]) {
            Ok(v) => v,
            Err(err) => panic!(err),
        };

        if instruction.opcode == Opcode::Stop {
            return (Opcode::Stop, 0, 0, 0);
        }

        let lhs_pos = self.fetch_position(self.instruction_point + 1);
        let lhs = match instruction.parameter_one {
            Mode::Immediate => lhs_pos,
            Mode::Position => self.fetch_position(lhs_pos as usize),
            Mode::Relative => self.fetch_position((self.relative_point - lhs_pos) as usize),
        };

        if instruction.opcode == Opcode::Input {
            return (instruction.opcode, 0, 0, lhs as usize);
        }

        if instruction.opcode == Opcode::Output || instruction.opcode == Opcode::RelativeBaseAdjust {
            return (instruction.opcode, lhs, 0, 0);
        }
        
        let rhs_pos = self.fetch_position(self.instruction_point + 1);
        let rhs = match instruction.parameter_one {
            Mode::Immediate => rhs_pos,
            Mode::Position => self.fetch_position(rhs_pos as usize),
            Mode::Relative => self.fetch_position((self.relative_point - rhs_pos) as usize),
        };

        if instruction.opcode == Opcode::JumpIfTrue || instruction.opcode == Opcode::JumpIfFalse {
            return (instruction.opcode, lhs, 0, rhs as usize);
        }

        let pos = self.inner[self.instruction_point + 3];

        (instruction.opcode, lhs, rhs, pos as usize)
    }

    pub fn until_stops(&mut self) -> Result<i64, &str> {
        let mut output = 0;

        loop {
            match self.process() {
                IntCodeState::Stopped => return Ok(output),
                IntCodeState::WaitingInput => return Err("missing inputs"),
                IntCodeState::Output(value) => output = value,
            }
        }
    }

    pub fn process(&mut self) -> IntCodeState {
        loop {
            let instruction = self.fetch_instruction();

            match instruction {
                (Opcode::Add, lhs, rhs, pos) => {
                    self.inner[pos] = lhs + rhs;
                    self.instruction_point += 4;
                }
                (Opcode::Mul, lhs, rhs, pos) => {
                    self.inner[pos] = lhs * rhs;
                    self.instruction_point += 4;
                }
                (Opcode::Input, _, _, pos) => {
                    match self.inputs.pop_front() {
                        Some(x) => self.inner[pos] = x,
                        None => return IntCodeState::WaitingInput,
                    };
                    self.instruction_point += 2;
                }
                (Opcode::Output, lhs, _, _) => {
                    self.instruction_point += 2;
                    return IntCodeState::Output(lhs);
                }
                (Opcode::JumpIfTrue, pred, _, pos) => {
                    self.instruction_point = if pred != 0 {
                        pos
                    } else {
                        self.instruction_point + 3
                    };
                }
                (Opcode::JumpIfFalse, pred, _, pos) => {
                    self.instruction_point = if pred == 0 {
                        pos
                    } else {
                        self.instruction_point + 3
                    };
                }
                (Opcode::LessThan, lhs, rhs, pos) => {
                    self.inner[pos] = if lhs < rhs { 1 } else { 0 };
                    self.instruction_point += 4;
                }
                (Opcode::Equals, lhs, rhs, pos) => {
                    self.inner[pos] = if lhs == rhs { 1 } else { 0 };
                    self.instruction_point += 4;
                }
                (Opcode::Stop, _, _, _) => return IntCodeState::Stopped,
                (Opcode::RelativeBaseAdjust, lhs, _, _) => {
                    self.relative_point += lhs;
                    self.instruction_point += 2;
                }
            }
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_example_1() {
        let mut subject = IntCode::new(vec![104,1125899906842624,99]);

        subject.insert_input(1);

        match subject.until_stops() {
            Ok(v) => assert_eq!(v, 1125899906842624),
            Err(e) => assert!(false, "unexpected {:?}", e),
        };
    }

    #[test]
    fn test_example_2() {
        let mut subject = IntCode::new(vec![109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99]);

        subject.insert_input(1);

        match subject.until_stops() {
            Ok(v) => assert_eq!(v, 2),
            Err(e) => assert!(false, "unexpected {:?}", e),
        };
    }

    #[test]
    fn test_example_3() {
        let mut subject = IntCode::new(vec![1102,34915192,34915192,7,4,7,99,0]);

        subject.insert_input(1);

        match subject.until_stops() {
            Ok(v) => assert_eq!(v, 1219070632396864),
            Err(e) => assert!(false, "unexpected {:?}", e),
        };
    }
}