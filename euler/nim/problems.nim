# nim compile --run problems.nim

import tables, sequtils

#[
  Multiples of 3 and 5

  If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6
  and 9. The sum of these multiples is 23. Find the sum of all the multiples of 3 or 5
  below 1000.

  This can be solved more efficiently with a formula.

  The sum of the multiples of numbers can be given a formula. The set of multiples
  of 3 and 5 int64ersect at the multiples of 15. So the solution is the sum of the
  multiples of 3 added to the sum of the multiples of 5 minus the sum of the
  multiples of 15.

  The sum of natural numbers from 1 to n = n(n+1)/2
  The sum of the first n multiples of 3  = 3n(n+1)/2
  The sum of the first n multiples of 5  = 5n(n+1)/2
  The sum of the first n multiples of 15 = 15n(n+1)/2

  Simple int64eger division can find the size of n for a number at or under k and multiple m:
      n = k / m

  Hence, given a limit of multiples under k, the sum S for this problem can be computed:
      S = 3(k/3)((k/3)+1)/2 + 5(k/5)((k/5)+1)/2 - 15(k/15)((k/15)+1)/2

  With k = 999 this equates to 233168.
]#

proc problem01(): int64 =
  toSeq(1..999).filterIt(it mod 3 == 0 or it mod 5 == 0).foldl(a + b)


#[
  Even Fibonacci Numbers

  Each new term in the Fibonacci sequence is generated by adding
  the previous two terms. By starting with 1 and 2, the first 10 terms will be:

  1, 2, 3, 5, 8, 13, 21, 34, 55, 89, ...

  By considering the terms in the Fibonacci sequence whose values do not exceed
  four million, find the sum of the even-valued terms.
]#
iterator fib(): int64 =
  var tmp = 0
  var a = 1
  var b = 1
  while true:
    yield b
    tmp = b
    b = a + b
    a = tmp

proc problem02(): int64 =
  var total = 0'i64
  for f in fib():
    # x_x No 'takeWhile' or equivalent in sequtils
    if f > 4000000'i64:
      break
    if f mod 2'i64 == 0'i64:
      total = total + f
  total


#[
  Largest prime factor

  The prime factors of 13195 are 5, 7, 13 and 29.
  What is the largest prime factor of the number 600851475143 ?
]#

type
  PrimeGeneratorState = ref object
    n: int64
    last: int64
    sieve: TableRef[int64, int64]

proc nextPrime(state: PrimeGeneratorState): int64 =
  while true:

    # x_x getOrDefault should have parameter for default, not default value
    # https://nim-lang.org/docs/tables.html#getOrDefault,Table[A,B],A
    # https://github.com/nim-lang/Nim/issues/4265
    var prime = state.sieve.getOrDefault(state.n)
    if prime == 0'i64:
      break

    # don't need this key, remove it to save space
    state.sieve.del(state.n)

    # add composite
    var composite = state.n + prime + prime
    while state.sieve.contains(composite):
      composite = composite + prime + prime

    state.sieve[composite] = prime
    state.n = state.n + 2'i64

  # add composite in prep for next round
  state.sieve[state.n * state.n] = state.n
  result = state.last
  state.last = state.n
  state.n = state.n + 2'i64

proc findFactors(ofn: int64): seq[int64] =
  var factors = newSeq[int64]()
  var quotient = ofn

  # x_x No initializer syntax to use with new?
  # https://nim-lang.org/docs/tut1.html#advanced-types-reference-and-pointer-types
  var pgs = new(PrimeGeneratorState)
  pgs.n = 3'i64
  pgs.last = 2'i64
  pgs.sieve = newTable[int64,int64]()

  while true:
    let prime = nextPrime(pgs)
    if prime > quotient:
      return factors

    var remainder = quotient mod prime
    while remainder == 0'i64:
      quotient = quotient div prime
      remainder = quotient mod prime
      factors.add(prime)

proc problem03(): int64 =
  findFactors(600851475143).foldl(if a > b: a else: b)


echo problem01()
echo problem02()
echo problem03()




