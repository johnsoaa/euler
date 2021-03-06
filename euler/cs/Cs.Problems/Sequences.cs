﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Cs.Problems
{
    public class Sequences
    {
        public static IEnumerable<int> Fibonacci()
        {
            int a = 0, b = 1, temp = 0;
            yield return a;
            yield return b;
            while (true)
            {
                temp = b;
                b = a + b;
                a = temp;
                yield return b;
            }
        }

        public static IEnumerable<long> LongRange(long lowerBound, long upperBound)
        {
            long current = lowerBound;
            while (current <= upperBound)
            {
                yield return current;
                current += 1;
            }
        }

        private static List<long> primes = new List<long> { 2, 3 };
        private static Dictionary<long, long> composites = new Dictionary<long, long>();

        public static IEnumerable<long> Primes()
        {
            int i = 0;
            for (; i < primes.Count; i++)
                yield return primes[i];

            var n = primes[i - 1];
            long prime;

            while (true)
            {
                while (composites.TryGetValue(n, out prime))
                {
                    // remove composite n from sieve to save space
                    composites.Remove(n);

                    // add composite
                    var check = n + prime + prime;
                    while (composites.ContainsKey(check))
                        check += prime + prime;
                    composites[check] = prime;

                    n += 2;
                }

                // add composite for next iteration
                composites[n * n] = n;
                primes.Add(n);
                yield return n;
                n += 2;
            }
        }

        /// <summary>
        /// Find the prime factors of a number.
        /// </summary>
        /// <param name="n">The number to find the prime factors of.</param>
        /// <returns></returns>
        public static List<long> GetPrimeFactors(long n)
        {
            var factors = new List<long>(1);
            var quotient = n;

            foreach (var prime in Primes())
            {
                if (prime > quotient)
                    break;

                for (var remainder = quotient % prime;
                    remainder == 0;
                    quotient /= prime, remainder = quotient % prime)
                {
                    factors.Add(prime);
                }
            }

            return factors;
        }

        public static IEnumerable<int> Squares()
        {
            // this could be made faster for repeat calls with memoization
            return Enumerable.Range(1, int.MaxValue).Select(n => n * n);
        }

        public static IEnumerable<Tuple<int, int, int>> PythagoreanTriplets()
        {
            return from c2 in Squares()
                   from b2 in Squares().TakeWhile(b2 => b2 < c2)
                   from a2 in Squares().TakeWhile(a2 => a2 < b2)
                   where a2 + b2 == c2
                   let a = (int)Math.Sqrt(a2)
                   let b = (int)Math.Sqrt(b2)
                   let c = (int)Math.Sqrt(c2)
                   select new Tuple<int, int, int>(a, b, c);
        }
        
        public static IEnumerable<long> TriangleNumbers()
        {
            var increment = 1;
            var trinum = 0;
            while (true)
            {
                trinum += increment;
                increment += 1;
                yield return trinum;
            }
        }

        public static IEnumerable<long> Collatz(long starter)
        {
            long n = starter;
            while (true)
            {
                if (n == 1)
                {
                    break;
                }
                if (n % 2 == 0)
                {
                    yield return n;
                    n /= 2;
                }
                else
                {
                    yield return n;
                    n = 3 * n + 1;
                }
            }
        }

        public static IEnumerable<DateTime> DateTimeRange(string start, string end, int stepSeconds)
        {
            if (stepSeconds < 1) throw new ArgumentOutOfRangeException("stepSeconds");
            var startParsed = DateTime.Parse(start);
            var endParsed = DateTime.Parse(end);

            if (startParsed < endParsed)
            {
                for (; startParsed <= endParsed; startParsed = startParsed.AddSeconds(stepSeconds))
                {
                    yield return startParsed;
                }
            }
            else
            {
                for (; endParsed >= startParsed; endParsed = endParsed.AddSeconds(-stepSeconds))
                {
                    yield return endParsed;
                }
            }
        }

        public static IEnumerable<long> GetAbundantNumbers(long limit)
        {
            return
                Sequences.LongRange(1, limit)
                    .Select(n => new Tuple<long, HashSet<long>>(n, Problem012.GetFactors(n)))
                    .Where(t => t.Item1 < t.Item2.Sum() - t.Item1) // subtract t.Item1 to get only proper divisors
                    .Select(t => t.Item1);
        }

        public static IEnumerable<long> GetAbundantSums(long limit)
        {
            var abundantNumbers = GetAbundantNumbers(limit).ToArray();
            var abundantSums = from abundantOne in abundantNumbers
                               from abundantTwo in abundantNumbers
                               where abundantOne <= abundantTwo
                               select abundantOne + abundantTwo;
            return abundantSums;
        }

        public static IEnumerable<BigInteger> BigFibonacci()
        {
            var a = new BigInteger(0);
            var b = new BigInteger(1);
            yield return a;
            yield return b;
            while (true)
            {
                var temp = b;
                b = a + b;
                a = temp;
                yield return b;
            }
        }
    }
}
