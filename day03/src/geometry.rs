#[derive(Debug, Clone, Copy)]
pub struct Point {
    pub x: f32,
    pub y: f32,
}

impl Point {
    pub fn new(x: f32, y: f32) -> Self {
        Point { x, y }
    }

    pub fn manhattan_distance(&self, other: Self) -> f32 {
        (self.x - other.x).abs() + (self.y - other.y).abs()
    }
}

#[derive(Debug)]
pub struct Line {
    pub start: Point,
    pub end: Point,
    pub steps: f32,
}

impl Line {
    pub fn axis_x(&self, v: f32) -> bool {
        let (min, max) = if self.start.x > self.end.x {
            (self.end.x, self.start.x)
        } else {
            (self.start.x, self.end.x)
        };

        if v > min && v < max {
            true
        } else {
            false
        }
    }

    pub fn axis_y(&self, v: f32) -> bool {
        let (min, max) = if self.start.y > self.end.y {
            (self.end.y, self.start.y)
        } else {
            (self.start.y, self.end.y)
        };

        if v > min && v < max {
            true
        } else {
            false
        }
    }

    pub fn intersect_with_steps(&self, other: &Self) -> Option<(Point, f32)> {
        if let Some(point) = self.intersect(other) {
            if point.x == self.start.x {
                Some((
                    point,
                    self.steps + other.steps + (point.y - self.start.y).abs() + (point.x - other.start.x).abs(),
                ))
            } else {
                Some((
                    point,
                    self.steps + other.steps + (point.x - self.start.x).abs() + (point.y - other.start.y).abs(),
                ))
            }
        } else {
            None
        }
    }

    pub fn intersect(&self, other: &Self) -> Option<Point> {
        if self.is_vertical() && other.is_vertical()
            || self.is_horizontal() && other.is_horizontal()
        {
            return None;
        }

        if self.is_horizontal() {
            if self.axis_x(other.start.x) && other.axis_y(self.start.y) {
                Some(Point::new(other.start.x, self.end.y))
            } else {
                None
            }
        } else {
            if self.axis_y(other.start.y) && other.axis_x(self.start.x) {
                Some(Point::new(self.start.x, other.start.y))
            } else {
                None
            }
        }
    }

    pub fn is_horizontal(&self) -> bool {
        if self.start.x - self.end.x != 0. {
            true
        } else {
            false
        }
    }

    pub fn is_vertical(&self) -> bool {
        if self.start.y - self.end.y != 0. {
            true
        } else {
            false
        }
    }
}
