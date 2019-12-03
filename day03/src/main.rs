use day03::board;
use day03::direction::Direction;
use day03::geometry::Point;
use day03::wire::Wire;

fn main() {
    // let l1 = Line(Point::new(3.0, 5.0), Point::new(3., 3.));
    // let l2 = Line(Point::new(6., 3.), Point::new(2., 3.));
    // println!("{:?}", l1.intersect(l2));

    let p = Point::new(2, 2);
    let l = Wire::new(Direction::Up, 5);

    let res = board::to_line(p, l);

    //println!("{:?}", p);
    println!("{:?}", res);
}
