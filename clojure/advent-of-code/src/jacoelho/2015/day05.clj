(ns jacoelho.2015.day05
  (:require
   [jacoelho.2015.aoc :as aoc]
   [clojure.test :refer [testing is]]
   [clojure.set :as set]))

(def day05-input (aoc/read-lines "2015/day05.txt"))

(def vowels #{\a \e \i \o \u})

(defn three-vowels?
  [input]
  (>=
   (->> input
        (filter vowels)
        (count))
   3))

(defn at-least-twice?
  [input]
  (boolean  (some (fn [[a b]]
                    (= a b)) (map vector input (rest input)))))

(defn not-contains-banned-substring?
  [input]
  (not (re-find #"ab|cd|pq|xy" input)))

(defn is-nice-string?
  [input]
  (and (three-vowels? input)
       (at-least-twice? input)
       (not-contains-banned-substring? input)))

(defn part01
  [input]
  (count (filter is-nice-string? input)))

(testing "Part 01"
  (is (= true (is-nice-string? "ugknbfddgicrmopn")))
  (is (= true (is-nice-string? "aaa")))
  (is (= false (is-nice-string? "jchzalrnumimnmhp")))
  (is (= false (is-nice-string? "haegwjzuvuyypxyu")))
  (is (= false (is-nice-string? "dvszwmarrgswjxmb")))
  (is (= 258 (part01 day05-input))))

(defn contains-two-pairs?
  [input]
  (->> input
       (map-indexed (fn [idx el] [idx el]))                ;; index elements
       (partition 2 1)                                     ;; create a pair with next element
       (map (fn [[[i a] [j b]]] [#{i j} [a b]]))           ;; create simplify pair 
       (group-by (fn [[_ a]] a))                           ;; group by elements
       (vals)                                              
       (filter (fn [elms] (>= (count elms) 2)))            ;; get elements with at least 2 (pairs)
       (map #(map (fn [[pos _]] pos) %))                   ;; get sets with positions
       (map (partial apply clojure.set/intersection))      ;; check if they intersect 
       (some empty?)))                                     ;; we want sets without overlaps

(defn contains-letter-repeats-with-one-between?
  [input]
  (->> input
       (partition 3 1)
       (some (fn [[a _ c]] (= a c)))))

(defn is-nicer-string?
  [input]
  (and (contains-two-pairs? input)
       (contains-letter-repeats-with-one-between? input)))

(defn part02
  [input]
  (count (filter is-nicer-string? input)))

(testing "Part 02"
  (is (= true (is-nicer-string? "qjhvhtzxzqqjkmpb")))
  (is (= true (is-nicer-string? "xxyxx")))
  (is (= false (is-nicer-string? "aaa")))
  (is (= false (is-nicer-string? "uurcxstgmygtbstg")))
  (is (= false (is-nicer-string? "ieodomkazucvgmuy")))
  (is (= 53 (part02 day05-input))))
