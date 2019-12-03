use crate::direction::Direction;
use crate::geometry::{Line, Point};
use crate::wire::Wire;

pub fn to_line(p: Point, w: Wire) -> Line {
    let Point { x, y } = p;
    let len = w.len();

    match w.direction() {
        Direction::Up => Line(p, Point::new(x, y + len)),
        Direction::Down => Line(p, Point::new(x, y - len)),
        Direction::Left => Line(p, Point::new(x - len, y)),
        Direction::Right => Line(p, Point::new(x + len, y)),
    }
}
