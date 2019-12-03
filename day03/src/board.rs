use crate::direction::Direction;
use crate::geometry::{Line, Point};
use crate::wire::Wire;

pub fn project_wire(p: Point, w: &Wire) -> Line {
    let Point { x, y } = p;
    let len = w.len();

    match w.direction() {
        Direction::Up => Line(p, Point::new(x, y + len)),
        Direction::Down => Line(p, Point::new(x, y - len)),
        Direction::Left => Line(p, Point::new(x - len, y)),
        Direction::Right => Line(p, Point::new(x + len, y)),
    }
}

pub struct WirePath<'a> {
    iter: ::std::slice::Iter<'a, Wire>,
    position: Point,
}

impl<'a> WirePath<'a> {
    pub fn new(path: &'a Vec<Wire>) -> Self {
        Self {
            iter: path.iter(),
            position: Point::new(0., 0.),
        }
    }
}

impl<'a> Iterator for WirePath<'a> {
    type Item = Line;

    fn next(&mut self) -> Option<Self::Item> {
        match self.iter.next() {
            None => None,
            Some(v) => {
                let line = project_wire(self.position, v);

                self.position = line.1;

                Some(line)
            }
        }
    }
}
