(ns jacoelho.2015.day01 
  (:require
   [jacoelho.aoc :as aoc]
   [clojure.test :refer [testing is]]
   [clojure.java.io :as io]))

(def day01-input
  (aoc/read-input "2015/day01.txt"))

(defn floor
  [direction]
  (case direction
    \( 1
    \) -1))

(defn part01
  [directions]
  (->> directions
       (map floor)
       (reduce +)))

(defn part02
  [directions]
  (->> directions
       (reductions (fn [[idx sum] elem]
                     [(inc idx) (+ sum (floor elem))]) [0 0])
       (drop-while (fn [[_ elm]] (not (neg? elm))))
       (ffirst)))

(testing "Part 01"
  (is (= 0 (part01 "()()")))
  (is (= 3 (part01 "))(((((")))
  (is (= 280 (part01 day01-input))))

(testing "Part 02"
  (is (= 5 (part02 "()())")))
  (is (= 1 (part02 ")")))
  (is (= 1797 (part02 day01-input))))
