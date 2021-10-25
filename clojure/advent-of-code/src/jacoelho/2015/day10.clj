(ns jacoelho.2015.day10
  (:require
   [jacoelho.aoc :as aoc]
   [clojure.test :refer [testing is]]))

(def day10-input 
  (aoc/long->digits 1321131112))

(defn look-and-say-one
  [col]
  [(count col) (first col)])

(defn look-and-say
  [col]
  (mapcat look-and-say-one (partition-by identity col)))

(defn part01
  [n input]
  (->> (iterate look-and-say input)
       (drop n)
       (first)
       (count)))
  
(testing "Part 01"
  (is (= 492982 (part01 40 day10-input))))

(testing "Part 02"
  (is (= 6989950 (part01 50 day10-input))))