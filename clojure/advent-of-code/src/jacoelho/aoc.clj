(ns jacoelho.aoc 
  (:require [clojure.java.io :as io]))

(defn read-lines
  "Reads aoc lines"
  ([input]
   (-> (io/resource input)
       (io/reader)
       (line-seq)))
  ([f input]
   (map f (read-lines input))))

(defn read-input
  "Reads aoc lines"
  [input]
   (slurp (io/resource input)))

(defn ->int [int]
  (Integer/parseInt int))

(defn ->uint [int]
  (Integer/parseUnsignedInt int))

(defn ->long [l]
  (Long/parseLong l))

(defn long->digits [^long num]
  (loop [n num
         res '()]
    (if (= n 0)
      res
      (recur (quot n 10) (conj res (rem n 10))))))

(defn digits->long [col]
  (reduce #(+ (* 10 %) %2) (long 0) col))