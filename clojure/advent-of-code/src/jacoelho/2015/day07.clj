(ns jacoelho.2015.day07
  (:require
   [jacoelho.aoc :as aoc]
   [clojure.test :refer [testing is]]))

(def mask 
  "16 bit mask"
  0xffff)

(defn gate-and
  [x y]
  (bit-and x y mask))

(defn gate-or
  [x y]
  (bit-and (bit-or x y) mask))

(defn gate-lshift
  [x y]
  (bit-and (bit-shift-left x y) mask))

(defn gate-rshift
  [x y]
  (bit-and (unsigned-bit-shift-right x y) mask))

(defn gate-not
  [x]
  (bit-and (bit-not x) mask))

(defn parse-line
  [line]
  (condp re-matches line
    #"^(\d+) -> (\w+)$" 
    :>> (fn [[_ value result]] [:assign result (aoc/->uint value)])

    #"^(\w+) -> (\w+)$"
    :>> (fn [[_ left result]] [:assign-from result left])

    #"^(\w+) AND (\w+) -> (\w+)$"
    :>> (fn [[_ left right result]] [:and result left right])
    
    #"^(\w+) OR (\w+) -> (\w+)$"
    :>> (fn [[_ left right result]] [:or result left right])
    
    #"^(\w+) LSHIFT (\w+) -> (\w+)$"
    :>> (fn [[_ left value result]] [:lshift result left (aoc/->uint value)])
    
    #"^(\w+) RSHIFT (\w+) -> (\w+)$"
    :>> (fn [[_ left value result]] [:rshift result left (aoc/->uint value)])

    #"^NOT (\w+) -> (\w+)$"
    :>> (fn [[_ left result]] [:not result left])))

(def day07-input (aoc/read-lines parse-line "2015/day07.txt"))

(defn update-circuit
  [circuit f result & args]
  (let [values (map #(get circuit % nil) args)]
    (assoc circuit result (apply f values))))

(update-circuit {} gate-and "a" "c" "d")

(defn simulate
  [circuit [op result argument-1 argument-2]]
  (case op
    :assign (assoc circuit result argument-1)

    :assign-from (assoc circuit result (get circuit argument-1 0))
    
    :not (assoc circuit result (gate-not (get circuit argument-1 0)))
    
    :and (assoc circuit result (gate-and (get circuit argument-1 0)
                                         (get circuit argument-2 0)))
    
    :or (assoc circuit result (gate-or (get circuit argument-1 0) 
                                       (get circuit argument-2 0)))
    
    :lshift (assoc circuit result (gate-lshift (get circuit argument-1 0)
                                               argument-2))
    
    :rshift (assoc circuit result (gate-rshift (get circuit argument-1 0)
                                               argument-2))))

(defn part01
  [input]
  (get (reduce simulate {} input) "a"))
