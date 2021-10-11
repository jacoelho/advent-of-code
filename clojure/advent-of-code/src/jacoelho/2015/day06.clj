(ns jacoelho.2015.day06
  (:require
   [jacoelho.aoc :as aoc]
   [clojure.test :refer [testing is]]))

(defn ->op
  [op]
  (case op
    "turn on" (constantly true)
    "toggle" not
    "turn off" (constantly false)))

(defn parse-line
  [line operation-translator]
  (let [[_ op & coords] (re-find #"^(turn on|toggle|turn off) (\d+),(\d+) through (\d+),(\d+)" line)
        operation (operation-translator op)
        [i j w z] (map aoc/->int coords)]
    [operation [i j] [w z]]))

(def day06-input
  (aoc/read-lines #(parse-line % ->op) "2015/day06.txt"))

(defn create-grid [rows cols initial-value]
  (vec (repeat rows (vec (repeat cols initial-value)))))

(defn update-grid
  [grid [op [i j] [w z]] ]
  (let [coords (for [x (range i (inc w))
                     y (range j (inc z))]
                 [x y])]
    (reduce #(update-in % %2 op) grid coords)))

(defn count-elements-on
  [grid]
  (apply + (map #(count (filter identity %)) grid)))

(defn part01
  [input]
  (->> input
       (reduce update-grid (create-grid 1000 1000 false))
       (count-elements-on)))

(testing "Part 01"
  (is (= 377891 (part01 day06-input))))

(defn ->brightness
  [op]
  (case op
    "turn on" inc
    "toggle" #(+ 2 %)
    "turn off" #(max 0 (dec %))))

(def day06-input-brightness
  (aoc/read-lines #(parse-line % ->brightness) "2015/day06.txt"))

(defn count-brightness
  [grid]
  (apply + (map #(reduce + %) grid)))

(defn part02
  [input]
  (->> input
       (reduce update-grid (create-grid 1000 1000 0))
       (count-brightness)))

(testing "Part 02"
  (is (= 14110788 (part02 day06-input-brightness))))
