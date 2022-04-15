using System;
using System.Numerics;

namespace login_server
{
    class Subscribe //64301; 77773
    {
        // Work in process

        private static readonly int p = 64301;
        private static readonly int q = 77773;
        private BigInteger hashValidationCode;

        private static readonly BigInteger modulus = (BigInteger)p * (BigInteger)q;
        private readonly int publicKey;
        private readonly double privateKey;

        private Subscribe(int e, double d)
        {
            publicKey = e;
            privateKey = d;
        }

        public static Subscribe CreateSubscribe(int e)
        {
            double fn = (p - 1) * (q - 1);

            if (e >= fn) { Console.WriteLine("e is too big"); return null; }
            if (GCD(e, fn) != 1) { Console.WriteLine("GCD error"); return null; }
            if (!PrimeNumberValidation(e)) { Console.WriteLine("e not prime"); return null; }

            double d = MultiplicativeInverse(e, fn);
            if (d == -1) { Console.WriteLine("wrong d"); return null; }

            if ((d * e) % fn != 1) { Console.WriteLine("smfn wrong"); }
            Console.WriteLine(e + "   " + d);
            return new Subscribe(e, d);
        }  //factory pattern for validating 'e' || 17 recomended

        private static double MultiplicativeInverse(int e, double fn)
        {
            double result;
            int k = 1;
            while (k < 1000000)
            {
                result = (double)((1 + (k * fn)) / e);
                if ((Math.Round(result, 5) % 1) == 0)
                {
                    return result;
                }
                else
                {
                    k++;
                }
            }

            return -1;
        }

        private static double GCD(double a, double b)
        {
            while (a != 0)
            {
                if (a < b) { b -= a; }
                else { a -= b; }
            }
            return b;
        }

        private static bool PrimeNumberValidation(int number)
        {
            double numberSqr = Math.Sqrt(number);

            for (int i = 2; i <= numberSqr; i++)
            {
                if (number % i == 0) { return false; }
            }
            return true;
        } 

        public BigInteger EcryptHash(byte[] byteHash)
        {
            BigInteger hash = new BigInteger(byteHash);

            return EncryptHash(hash);
        }
        public BigInteger EncryptHash(BigInteger hash)
        {
            return LowMemoryExponentiationByModulus(hash, privateKey, modulus);
        }

        public BigInteger DecryptHash(BigInteger code)
        {
            return LowMemoryExponentiationByModulus(code, publicKey, modulus);
        }

        private static BigInteger LowMemoryExponentiationByModulus(BigInteger number, double degree, BigInteger modulus)
        {
            BigInteger result = 1;
            for (int i = 0; i < degree; i++)
            {
                result = (result * number) % modulus;
            }
            return result;
        }
    }
}
