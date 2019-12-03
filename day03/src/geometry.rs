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
pub struct Line(pub Point, pub Point);

impl Line {
    pub fn axis_x(&self, v: f32) -> bool {
        let (min, max) = if self.0.x > self.1.x {
            (self.1.x, self.0.x)
        } else {
            (self.0.x, self.1.x)
        };

        if v > min && v < max {
            true
        } else {
            false
        }
    }

    pub fn axis_y(&self, v: f32) -> bool {
        let (min, max) = if self.0.y > self.1.y {
            (self.1.y, self.0.y)
        } else {
            (self.0.y, self.1.y)
        };

        if v > min && v < max {
            true
        } else {
            false
        }
    }

    pub fn intersect(&self, other: &Self) -> Option<Point> {
        if self.is_vertical() && other.is_vertical()
            || self.is_horizontal() && other.is_horizontal()
        {
            return None;
        }

        if self.is_horizontal() {
            if self.axis_x(other.0.x) && other.axis_y(self.0.y) {
                Some(Point::new(other.0.x, self.0.y))
            } else {
                None
            }
        } else {
            if self.axis_y(other.0.y) && other.axis_x(self.0.x) {
                Some(Point::new(self.0.x, other.0.y))
            } else {
                None
            }
        }
    }

    pub fn is_horizontal(&self) -> bool {
        if self.0.x - self.1.x != 0. {
            true
        } else {
            false
        }
    }

    pub fn is_vertical(&self) -> bool {
        if self.0.y - self.1.y != 0. {
            true
        } else {
            false
        }
    }
}
