(ns jacoelho.2015.day17
  (:require
   [jacoelho.aoc :as aoc]
   [clojure.test :refer [testing is]]
   [clojure.math.combinatorics :refer [subsets]]))

(def day17-input
  (aoc/read-lines aoc/->int "2015/day17.txt"))

(defn containers-matches
  [input]
  (->> input
       (map-indexed vector)
       (subsets)
       (filter #(= 150 (reduce + (map second %))))))

(defn part01
  [input]
  (->> input
       (containers-matches)
       (count)))

(defn part02
  [input]
  (->> input
       (containers-matches)
       (map count)
       (apply min)))

(testing "Part 01"
  (is (= 4372 (part01 day17-input))))

(testing "Part 02"
  (is (= 4 (part02 day17-input))))
