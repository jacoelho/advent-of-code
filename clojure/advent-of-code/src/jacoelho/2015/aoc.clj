(ns jacoelho.2015.aoc 
  (:require [clojure.java.io :as io]))

(defn read-input
  "Reads aoc input"
  ([input]
   (-> (io/resource input)
       (io/reader)
       (line-seq)))
  ([f input]
   (map f (read-input input))))


(defn ->int [int]
  (Integer/parseInt int))

(defn ->long [l]
  (Long/parseLong l))
