using System;
using System.Linq;
using NUnit.Framework;

namespace Utilities
{
    public class PrimeNumbersGenerator
    {
        [Ignore("Not needed")]
        [Test]
        public static void Generate()
        {
            var powers = GetPowersOfTwo(25);
            var primes = GeneratePrimes(33554432 * 2);

            for (var i = 0; i < powers.Length; i++)
            {
                var nextPower = powers[i + 1];

                var primeIndex = 0;

                while (primes[primeIndex] < nextPower)
                {
                    primeIndex++;
                }

                powers[i] = primes[primeIndex - 1];

                Console.WriteLine(powers[i]);
            }

            Console.WriteLine(string.Join(",", GeneratePrimes(int.MaxValue / 16)));
        }

        public static int[] GeneratePrimes(int limit)
        {
            if (limit < 2)
                return Array.Empty<int>();

            bool[] isPrime = new bool[limit + 1];
            Array.Fill(isPrime, true);
            isPrime[0] = isPrime[1] = false;

            for (int i = 2; i * i <= limit; i++)
            {
                if (isPrime[i])
                {
                    for (int j = i * i; j <= limit; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            return Enumerable.Range(0, limit + 1)
                .Where(x => isPrime[x])
                .ToArray();
        }

        public static int[] GetPowersOfTwo(int n)
        {
            int[] powers = new int[n];
            int power = 2;

            for (int i = 0; i < n; i++)
            {
                powers[i] = power;
                power *= 2;
            }

            return powers;
        }
    }
}