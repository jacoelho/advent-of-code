(ns jacoelho.2015.day04
  (:require
   [clojure.string :as str]
   [clojure.test :refer [testing is]])
  (:import (java.security MessageDigest)))

(set! *warn-on-reflection* true)

(defn md5 [^String s]
  (let [algorithm (MessageDigest/getInstance "MD5")
        raw       (.digest algorithm (.getBytes s))]
    (format "%032x" (BigInteger. 1 raw))))

(defn coin?
  [input suffix]
  (when (str/starts-with? input suffix)
    input))

(defn mine
  [password suffix]
  (->> (range)
       (map (fn [el] [el (md5 (str password el))]))
       (some (fn [[el checksum]] (when (coin? checksum suffix) el)))))

(defn part01
  [password]
  (mine password "00000"))

(testing "Part 01"
  (is (= 609043 (part01 "abcdef")))
  (is (= 1048970 (part01 "pqrstuv")))
  (is (= 254575 (part01 "bgvyzdsv"))))

(defn part02
  [password]
  (mine password "000000"))

(testing "Part 02"
  (is (= 1038736 (part02 "bgvyzdsv"))))
