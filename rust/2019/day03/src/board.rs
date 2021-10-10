use crate::direction::Direction;
use crate::geometry::{Line, Point};
use crate::wire::Wire;

pub fn project_wire(p: Point, w: &Wire, steps: f32) -> Line {
    let Point { x, y } = p;
    let len = w.len();

    match w.direction() {
        Direction::Up => Line {
            start: p,
            end: Point::new(x, y + len),
            steps: steps,
        },
        Direction::Down => Line {
            start: p,
            end: Point::new(x, y - len),
            steps: steps,
        },
        Direction::Left => Line {
            start: p,
            end: Point::new(x - len, y),
            steps: steps,
        },
        Direction::Right => Line {
            start: p,
            end: Point::new(x + len, y),
            steps: steps,
        },
    }
}

pub struct WirePath<'a> {
    iter: ::std::slice::Iter<'a, Wire>,
    position: Point,
    steps: f32,
}

impl<'a> WirePath<'a> {
    pub fn new(path: &'a Vec<Wire>) -> Self {
        Self {
            iter: path.iter(),
            position: Point::new(0., 0.),
            steps: 0.,
        }
    }
}

impl<'a> Iterator for WirePath<'a> {
    type Item = Line;

    fn next(&mut self) -> Option<Self::Item> {
        match self.iter.next() {
            None => None,
            Some(v) => {
                let line = project_wire(self.position, v, self.steps);

                self.steps += v.len();
                self.position = line.end;

                Some(line)
            }
        }
    }
}
