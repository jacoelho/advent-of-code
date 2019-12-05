use std::convert::TryFrom;
use std::fmt;

#[derive(Debug, PartialEq)]
enum Mode {
    Position,
    Immediate,
}

impl TryFrom<i32> for Mode {
    type Error = String;

    fn try_from(item: i32) -> Result<Self, Self::Error> {
        match item {
            0 => Ok(Self::Position),
            1 => Ok(Self::Immediate),
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
}

impl TryFrom<i32> for Opcode {
    type Error = String;

    fn try_from(item: i32) -> Result<Self, Self::Error> {
        match item {
            1 => Ok(Self::Add),
            2 => Ok(Self::Mul),
            3 => Ok(Self::Input),
            4 => Ok(Self::Output),
            5 => Ok(Self::JumpIfTrue),
            6 => Ok(Self::JumpIfFalse),
            7 => Ok(Self::LessThan),
            8 => Ok(Self::Equals),
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

impl TryFrom<i32> for Instruction {
    type Error = String;

    fn try_from(item: i32) -> Result<Self, Self::Error> {
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
    inner: Vec<i32>,
    instruction_point: usize,
    input: i32,
}

impl IntCode {
    pub fn new(mut v: Vec<i32>, input: i32) -> Self {
        Self {
            inner: v,
            instruction_point: 0,
            input: input,
        }
    }

    fn fetch_instruction(&self) -> (Opcode, i32, i32, usize) {
        let instruction = match Instruction::try_from(self.inner[self.instruction_point]) {
            Ok(v) => v,
            Err(err) => panic!(err),
        };

        if instruction.opcode == Opcode::Stop {
            return (Opcode::Stop, 0, 0, 0);
        }

        if instruction.opcode == Opcode::Input || instruction.opcode == Opcode::Output {
            let pos = self.inner[self.instruction_point + 1];
            return (instruction.opcode, 0, 0, pos as usize);
        }

        let lhs = match instruction.parameter_one {
            Mode::Immediate => self.inner[self.instruction_point + 1],
            Mode::Position => self.inner[self.inner[self.instruction_point + 1] as usize],
        };

        let rhs = match instruction.parameter_two {
            Mode::Immediate => self.inner[self.instruction_point + 2],
            Mode::Position => self.inner[self.inner[self.instruction_point + 2] as usize],
        };

        if instruction.opcode == Opcode::JumpIfTrue || instruction.opcode == Opcode::JumpIfFalse {
            return (instruction.opcode, lhs, 0, rhs as usize);
        }

        let pos = self.inner[self.instruction_point + 3];

        (instruction.opcode, lhs, rhs, pos as usize)
    }
}

impl Iterator for IntCode {
    type Item = i32;

    fn next(&mut self) -> Option<Self::Item> {
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
                self.inner[pos] = self.input;
                self.instruction_point += 2;
            }
            (Opcode::Output, _, _, pos) => {
                println!("diagnostic: {}", self.inner[pos]);
                self.instruction_point += 2;
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
            (Opcode::Stop, _, _, _) => return None,
        };

        Some(self.inner[0])
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_opcode() {
        match Opcode::try_from(1) {
            Ok(Opcode::Add) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Opcode::try_from(2) {
            Ok(Opcode::Mul) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Opcode::try_from(99) {
            Ok(Opcode::Stop) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Opcode::try_from(9) {
            Err(_) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }
    }

    #[test]
    fn test_mode() {
        match Mode::try_from(0) {
            Ok(Mode::Position) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Mode::try_from(1) {
            Ok(Mode::Immediate) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Mode::try_from(5) {
            Err(_) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }
    }

    #[test]
    fn test_instruction() {
        match Instruction::try_from(1002) {
            Ok(Instruction {
                opcode: Opcode::Mul,
                parameter_one: Mode::Position,
                parameter_two: Mode::Immediate,
                parameter_three: Mode::Position,
            }) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Instruction::try_from(01002) {
            Ok(Instruction {
                opcode: Opcode::Mul,
                parameter_one: Mode::Position,
                parameter_two: Mode::Immediate,
                parameter_three: Mode::Position,
            }) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Instruction::try_from(11199) {
            Ok(Instruction {
                opcode: Opcode::Stop,
                parameter_one: Mode::Immediate,
                parameter_two: Mode::Immediate,
                parameter_three: Mode::Immediate,
            }) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }

        match Instruction::try_from(12199) {
            Err(_) => assert!(true),
            v => assert!(false, "unexpected {:?}", v),
        }
    }

    #[test]
    fn test_fetch() {
        let subject = IntCode {
            inner: vec![1002, 4, 3, 4, 33],
            instruction_point: 0,
            input: 0,
        };

        assert_eq!(subject.fetch_instruction(), (Opcode::Mul, 33, 3, 4));
    }
}
