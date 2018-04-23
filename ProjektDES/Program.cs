using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektDES
{
    class Program
    {
        private static List<bool> _extraBitsAdded = new List<bool>();
        private static readonly int[] Pc1 = //PERMUTATED CHOICE
          {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        private static readonly int[] Pc2 = //PERMUTATED CHOICE 2
        {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };

        private static readonly int[] Ip = //INITITAL PERMUTATION
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        private static readonly int[] Ipinv = //INITIAL PERMUTATION INVERTED
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        private static readonly int[] E =
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        private static readonly int[] P =
        {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        private static readonly byte[,] SBoxes =
        {
            {
                14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
                4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
                15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13
            },
            {
                15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
                0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
                13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9
            },
            {
                10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
                13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
                1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12
            },
            {
                7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
                10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
                3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14
            },
            {
                2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
                4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
                11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3
            },
            {
                12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
                9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
                4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13
            },
            {
                4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
                1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
                6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12
            },
            {
                13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
                7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
                2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11
            }
        };

        private static readonly int[] LeftShifts = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        private static BitArray _key;

        static void Main(string[] args)
        {
            Console.WriteLine("1-Plik binarny \n2-Konsola\n3-Plik");
            var c = Console.ReadKey();
            Console.WriteLine(Environment.NewLine);

            using (var file = new StreamReader("key.txt"))
            {
                _key = GetBitArrayFromInput(file.ReadLine());
            }

            while (c.KeyChar != '4')
            {
                Console.WriteLine("1 - ENCODE \n2 - DECODE\n\n");
                var choose = Console.ReadKey();
                switch (c.KeyChar)
                {

                    case '1': // Plik binarny
                        while (choose.KeyChar != '3')
                        {
                            switch (choose.KeyChar)
                            {
                                case '1':
                                    {
                                        Console.WriteLine("\nPodaj nazwę pliku wejsciowego");
                                        var inputFile = Console.ReadLine();
                                        Console.WriteLine("\nPodaj nazwę pliku wyjściowego");
                                        var outputFile = Console.ReadLine();

                                        var inputData = LoadFromFile(inputFile);
                                        var data = Encode(inputData, _key);

                                        SaveAsBinary(outputFile, data);
                                        Console.WriteLine("\nWynik został zapisany do pliku");
                                    }
                                    break;
                                case '2':
                                    {
                                        Console.WriteLine("\nPodaj nazwę pliku wejsciowego");
                                        var inputFile = Console.ReadLine();
                                        Console.WriteLine("\nPodaj nazwę pliku wyjściowego");
                                        var outputFile = Console.ReadLine();

                                        var inputData = LoadFromFile(inputFile);
                                        var data = Decode(inputData, _key);
                                        SaveAsBinary(outputFile, data);
                                        Console.WriteLine("\n Wynik został zapisany do pliku");
                                    }
                                    break;
                                default:
                                    {
                                    }
                                    break;
                            }
                            choose = Console.ReadKey();
                        }
                        break;

                    case '2': //konsola
                        while (choose.KeyChar != '3')
                        {
                            switch (choose.KeyChar)
                            {
                                case '1':
                                    {
                                        Console.WriteLine("\nPodaj wejscie");
                                        var inputdata = Console.ReadLine();

                                        BitArray data = GetBitArrayFromInput(inputdata);
                                        Encode(data, _key);
                                    }
                                    break;
                                case '2':
                                    {
                                        Console.WriteLine("\nPodaj wejscie");
                                        var inputdata = Console.ReadLine();

                                        BitArray data = GetBitArrayFromInput(inputdata);
                                        Decode(data, _key);
                                    }
                                    break;
                                default:
                                    {
                                    }
                                    break;
                            }
                            choose = Console.ReadKey();
                        }
                        break;
                    case '3': //plik
                        while (choose.KeyChar != '3')
                        {
                            switch (choose.KeyChar)
                            {
                                case '1':
                                    {
                                        Console.WriteLine("\nPodaj nazwę pliku wejsciowego");
                                        var inputFile = Console.ReadLine();
                                        Console.WriteLine("\nPodaj nazwę pliku wyjściowego");
                                        var outputFile = Console.ReadLine();

                                        BitArray data = LoadContentFromFile(inputFile);
                                        var result=Encode(data, _key);

                                        SaveContentToFile(result,outputFile);
                                    }
                                    break;
                                case '2':
                                    {
                                        Console.WriteLine("\nPodaj nazwę pliku wejsciowego");
                                        var inputFile = Console.ReadLine();
                                        Console.WriteLine("\nPodaj nazwę pliku wyjściowego");
                                        var outputFile = Console.ReadLine();

                                        BitArray data = LoadContentFromFile(inputFile);
                                        var result = Decode(data, _key);

                                        SaveContentToFile(result, outputFile);
                                    }
                                    break;
                                default:
                                    {
                                    }
                                    break;
                            }
                            choose = Console.ReadKey();
                        }
                        break;
                }
            }

            Console.ReadKey();
        }

        private static void SaveContentToFile(BitArray data,string path)
        {
            using (var writetext = new StreamWriter(path))
            {
                foreach (bool b in data)
                {
                    var cToWrite=(b)?'1':'0';
                   writetext.Write(cToWrite);
                }
            }
        }

        private static BitArray LoadContentFromFile(string path)
        {
            var data = File.ReadAllLines(path);
            var boolArr = new List<bool>();
            foreach (var s in data)
            {
                foreach (char c in s)
                {
                    if (c == '1')
                    {
                        boolArr.Add(true);
                    }
                    else if (c == '0')
                    {
                        boolArr.Add(false);
                    }
                }
            }
            return  new BitArray(boolArr.ToArray());
        }

        private static void SaveAsBinary(string path, BitArray data)
        {
            byte[] bytes = new byte[data.Length / 8 + (data.Length % 8 == 0 ? 0 : 1)];
            data.CopyTo(bytes, 0);
            File.WriteAllBytes(path, bytes);
        }

        private static BitArray Encode(BitArray data, BitArray key)
        {
            var chunkedData = Chunk(data); //chunk for 64-bit data
            chunkedData.Add(CalculateAddedBits());
            var schedule = KeySchedule(key);

            var results = new List<BitArray>();

            for (var index = 0; index < chunkedData.Count; index++)
            {
                var bitArray = chunkedData[index];
                bitArray = Permutate(bitArray, Ip, 64);

                var splittedTo32Bits = SplitToTwoPieces(bitArray);

                for (int i = 0; i < 16; i++)
                {
                    var lastLeft = splittedTo32Bits.Left;
                    var lastRight = splittedTo32Bits.Right;
                    splittedTo32Bits = new Pair
                    {
                        Left = lastRight,
                        Right = Xor(lastLeft, F(splittedTo32Bits.Right, schedule[i + 1]))
                    };
                }

                var joined = Concat(splittedTo32Bits.Right, splittedTo32Bits.Left);
                results.Add(Permutate(joined, Ipinv, 64));
                //Log(results[index], index.ToString());
            }
            return ConcatBitArrays(results);
        }

        private static BitArray ConcatBitArrays(List<BitArray> results)
        {
            var res = results[0];

            for (var index = 1; index < results.Count; index++)
            {
                var bitArray = results[index];
                res = res.Append(bitArray);
            }
            return res;
        }

        private static BitArray CalculateAddedBits()
        {
            var result = new bool[64];

            string binary = Convert.ToString(_extraBitsAdded.Count, 2);

            var index = binary.Length - 1;

            for (int i = 63; i > 0; i--)
            {
                if (binary[index] == '0')
                    result[i] = false;
                else if (binary[index] == '1')
                    result[i] = true;

                index--;
                if (index < 0)
                    break;
            }

            return new BitArray(result);
        }

        private static BitArray Decode(BitArray data, BitArray key)
        {
            var chunkedData = Chunk(data); //chunk for 64-bit data
            var schedule = KeySchedule(key); //

            var results = new List<BitArray>();

            for (var index = 0; index < chunkedData.Count; index++)
            {
                var bitArray = chunkedData[index];
                bitArray = Permutate(bitArray, Ip, 64);

                var splittedTo32Bits = SplitToTwoPieces(bitArray);

                for (int i = 16; i >= 1; i--)
                {
                    var lastLeft = splittedTo32Bits.Left;
                    var lastRight = splittedTo32Bits.Right;
                    splittedTo32Bits = new Pair
                    {
                        Left = lastRight,
                        Right = Xor(lastLeft, F(splittedTo32Bits.Right, schedule[i]))
                    };
                }

                var joined = Concat(splittedTo32Bits.Right, splittedTo32Bits.Left);
                results.Add(Permutate(joined, Ipinv, 64));

               // Log(results[index], index.ToString());
            }

            results = CutExtraBits(results);

            //foreach (var item in results)
            //{
            //    Log(item);
            //}
            return ConcatBitArrays(results);
        }

        private static List<BitArray> CutExtraBits(List<BitArray> results)
        {
            int addedExtraBits = 0;
            var bitArray = results[results.Count - 1];

            for (int i = 0; i < bitArray.Count; i++)
            {
                if (bitArray[i])
                    addedExtraBits += Convert.ToInt32(Math.Pow(2, bitArray.Count - 1 - i));
            }

            var lastArray = new List<bool>();

            for (int i = 0; i < 64 - addedExtraBits; i++)
            {
                lastArray.Add(results[results.Count - 2][i]);
            }

            results.RemoveAt(results.Count - 1);
            results.RemoveAt(results.Count - 1);
            results.Add(new BitArray(lastArray.ToArray()));

            return results;
        }

        private static BitArray F(BitArray right, BitArray key)
        {
            var e = Permutate(right, E, 48);
            var x = Xor(e, key); //pkt 9

            var bs = Split(x);

            var result = new List<BitArray>();

            for (int i = 0; i < 8; i++)
            {
                result.Add(SBoxLookup(bs[i], i));
            }

            var bitArray32 = MergeListOfBitArrays(result);

            var permutated = Permutate(bitArray32, P, 32);
            // Log(permutated, "Funkcja f");

            return permutated;
        }

        private static BitArray MergeListOfBitArrays(List<BitArray> list)
        {
            return new BitArray(list.SelectMany(bitArray => bitArray.Cast<bool>()).ToArray());
        }

        private static BitArray SBoxLookup(BitArray bitArray, int sbox)
        {
            var row = 0;
            var col = 0;
            //Log(bitArray, "sbox");

            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i] && i == 0)
                    row += Convert.ToInt16(Math.Pow(2, 1));
                else if (bitArray[i] && i == 5)
                    row += Convert.ToInt16(Math.Pow(2, 0));
                else if (bitArray[i] && i == 4)
                    col += Convert.ToInt16(Math.Pow(2, 0));
                else if (bitArray[i] && i == 3)
                    col += Convert.ToInt16(Math.Pow(2, 1));
                else if (bitArray[i] && i == 2)
                    col += Convert.ToInt16(Math.Pow(2, 2));
                else if (bitArray[i] && i == 1)
                    col += Convert.ToInt16(Math.Pow(2, 3));
            }

            var index = row == 0 ? col : (row * 16) + col;

            var array = (new byte[] { SBoxes[sbox, index] });

            return Reverse(new BitArray(array));
        }

        private static BitArray Reverse(BitArray array)
        {
            var result = new List<bool>();

            for (int i = 3; i >= 0; i--)
            {
                result.Add(array[i]);
            }

            return new BitArray(result.ToArray());
        }

        private static List<BitArray> KeySchedule(BitArray key64)
        {
            var result = new List<BitArray>();
            var key56 = Permutate(key64, Pc1, 56);

            var keySplittedTo28Bits = SplitToTwoPieces(key56);

            var schedule = new List<Pair> { keySplittedTo28Bits };

            for (int i = 1; i <= LeftShifts.Count(); i++)
            {
                schedule.Add(new Pair
                {
                    Left = LeftShift56(schedule[i - 1].Left, LeftShifts[i - 1]),
                    Right = LeftShift56(schedule[i - 1].Right, LeftShifts[i - 1])
                });
            }

            for (int i = 0; i < schedule.Count; i++)
            {
                var joined = Concat(schedule[i].Left, schedule[i].Right);
                var permuted = Permutate(joined, Pc2, 48);
                Log(permuted, "Klucz" + i);
                result.Add(permuted);
            }

            return result;
        }

        private static BitArray Concat(BitArray left, BitArray right)
        {
            var result = left.Cast<bool>().ToList();
            result.AddRange(right.Cast<bool>());

            return new BitArray(result.ToArray());
        }

        private static BitArray LeftShift56(BitArray array, int v)
        {
            var result = new bool[28];

            for (int i = 0; i < array.Length; i++)
            {
                var index = i - v;
                if (index < 0)
                {
                    result[array.Length + index] = array[i];
                }
                else
                {
                    result[index] = array[i];
                }
            }

            return new BitArray(result);
        }

        private static Pair SplitToTwoPieces(BitArray bitArray)
        {
            var result = new Pair();
            var left = new List<bool>();
            var right = new List<bool>();

            var divider = bitArray.Length / 2;

            for (int i = 0; i < divider; i++)
            {
                left.Add(bitArray[i]);
            }
            for (int i = divider; i < bitArray.Length; i++)
            {
                right.Add(bitArray[i]);
            }
            result.Left = new BitArray(left.ToArray());
            result.Right = new BitArray(right.ToArray());

            return result;
        }

        private static BitArray Permutate(BitArray data, int[] table, int bitCount)
        {
            var result = new bool[bitCount];

            for (var index = 0; index < table.Length; index++)
            {
                var value = table[index] - 1;
                result[index] = data[value];
            }
            return new BitArray(result);
        }
        private static List<BitArray> Split(BitArray data)
        {
            var result = new List<BitArray>();
            var counter = 0;
            const int datalength = 6;

            var chunk = new List<bool>();

            for (var index = 0; index < data.Count; index++)
            {
                bool b = data[index];
                chunk.Add(b);
                counter++;
                if (counter == datalength)
                {
                    result.Add(new BitArray(chunk.ToArray()));
                    chunk = new List<bool>();
                    counter = 0;
                }
            }
            return result;
        }

        private static List<BitArray> Chunk(BitArray data)
        {
            var result = new List<BitArray>();
            var counter = 0;
            const int datalength = 64;

            var chunk = new List<bool>();
            for (var index = 0; index < data.Count; index++)
            {
                bool b = data[index];
                chunk.Add(b);
                counter++;
                if (counter == datalength)
                {
                    result.Add(new BitArray(chunk.ToArray()));
                    chunk = new List<bool>();
                    counter = 0;
                }
                if (index == data.Count - 1 && counter != datalength && counter > 0)
                {
                    //add to round 64 bits
                    for (int i = 0; i < datalength - counter; i++)
                    {
                        chunk.Add(false);
                        _extraBitsAdded.Add(false);
                    }
                    result.Add(new BitArray(chunk.ToArray()));
                }
            }
            return result;
        }

        private static BitArray GetBitArrayFromInput(string inputdata)
        {
            var result = new List<bool>();
            foreach (var c in inputdata)
            {
                if (c == '0')
                {
                    result.Add(false);
                }
                else if (c == '1')
                {
                    result.Add(true);
                }
            }
            return new BitArray(result.ToArray());
        }

        private static BitArray LoadFromFile(string path)
        {
            var bytesArr = File.ReadAllBytes(path);
            var result = new BitArray(bytesArr);

            return result;
        }

        private static BitArray Xor(BitArray e, BitArray key)
        {
            var result = new List<bool>();

            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] == false && key[i] == true || (e[i] == true && key[i] == false))
                {
                    result.Add(true);
                }
                else
                {
                    result.Add(false);
                }
            }
            return new BitArray(result.ToArray());
        }

        private static void Log(BitArray array, string message = "")
        {
            Debug.Write(message + "\n");
            foreach (bool item in array)
            {
                Debug.Write(item ? "1" : "0");
            }
            Debug.Write("\n");
        }


        private struct Pair
        {
            public BitArray Left { get; set; }
            public BitArray Right { get; set; }
        }
        public static BitArray Append(BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }
    }
}
