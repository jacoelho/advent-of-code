(ns jacoelho.2015.day02
  (:require
   [jacoelho.2015.aoc :as aoc]
   [clojure.test :refer [testing is]]
   [clojure.string :as str]))

(defn parse
  [line]
  (->> (str/split line #"x")
       (map aoc/->int)))

(def day02-input
  (aoc/read-lines parse "2015/day02.txt"))

(defn side-areas
  [[length width height]]
  [(* length width) (* width height) (* height length)])

(defn wrap-smallest-side
  [areas]
  (apply min areas))

(defn wrap-box
  [areas]
  (reduce #(+ %1 (* 2 %2)) 0 areas))

(defn wrap
  [input]
  (let [areas (side-areas input)]
    (+ (wrap-box areas) 
       (wrap-smallest-side areas))))

(defn part01
  [input]
  (->> input
       (map wrap)
       (reduce +)))

(testing "Part 01"
  (is (= 58 (wrap [2 3 4])))
  (is (= 43 (wrap [1 1 10])))
  (is (= 1588178 (part01 day02-input))))

(defn perimeter
  [l w]
  (* 2 (+ l w)))

(defn ribbon
  [[length width height]]
  (let [smallest-perimeter (min (perimeter length width)
                                (perimeter width height)
                                (perimeter height length))
        volume             (* length width height)]
    (+ smallest-perimeter volume)))

(defn part02
  [input]
  (->> input
       (map ribbon)
       (reduce +)))

(testing "Part 02"
  (is (= 34 (ribbon [2 3 4])))
  (is (= 14 (ribbon [1 1 10])))
  (is (= 3783758 (part02 day02-input))))