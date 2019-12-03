use day03::board::WirePath;
use day03::geometry::{Line, Point};
use day03::wire::Wire;
use std::fs;

fn main() -> Result<(), ::std::io::Error> {
    let contents = fs::read_to_string("test_data/input.txt")?;

    //let contents = "R8,U5,L5,D3\nU7,R6,D4,L4".to_string();

    let raw_wires: Vec<Vec<Wire>> = contents
        .lines()
        .map(|line| {
            line.split(",")
                .map(|v| v.parse().unwrap())
                .collect::<Vec<_>>()
        })
        .collect();

    let wire_a_path = WirePath::new(&raw_wires[0]).into_iter().collect::<Vec<_>>();
    let wire_b_path = WirePath::new(&raw_wires[1]).into_iter().collect::<Vec<_>>();

    println!("{:?}", part_1(&wire_a_path, &wire_b_path));

    Ok(())
}

fn part_1(a: &Vec<Line>, b: &Vec<Line>) -> Option<f32> {
    let mut result = Vec::new();

    for line_a in a.iter() {
        for line_b in b.iter() {
            if let Some(p) = line_a.intersect(line_b) {
                result.push(p);
            }
        }
    }

    let origin = Point::new(0., 0.);

    result
        .iter()
        .map(|p| p.manhattan_distance(origin))
        .fold(None, |min, x| match min {
            None => Some(x),
            Some(y) => Some(if x < y { x } else { y }),
        })
}
