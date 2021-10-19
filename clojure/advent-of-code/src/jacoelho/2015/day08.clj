(ns jacoelho.2015.day08
  (:require
   [jacoelho.aoc :as aoc]
   [clojure.test :refer [testing is]]))

(def day08-input
  (aoc/read-lines "2015/day08.txt"))

(defn count-memory
  [input]
  (count input))

(defn count-code
  [input]
  (loop [count         -2
         [x y :as all] input]
    (if (nil? x)
      count
      (recur (inc count)
             (case [x y]
               [\\ \\] (drop 2 all)
               [\\ \"] (drop 2 all)
               [\\ \x] (drop 4 all)
               (rest all))))))

(defn part01
  [input]
  (apply + (map #(- (count-memory %)
                    (count-code %)) 
                input)))

(testing "Part 01"
  (is (= 1333 (part01 day08-input))))

(defn count-encoded
  [input]
  (loop [count         2
         [x & xs] input]
    (if (nil? x)
      count
      (recur (case x
               \" (+ 2 count)
               \\ (+ 2 count)
               (inc count)) 
             xs))))

(defn part02
  [input]
  (apply + (map #(- (count-encoded %)
                    (count-memory %))
                input)))

(testing "Part 02"
  (is (= 2046 (part02 day08-input))))
