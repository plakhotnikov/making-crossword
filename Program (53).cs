using System;
using System.Diagnostics.Tracing;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;

namespace secondsem
{
    class Program
    {

        static int asdasd = 0;
        private static bool[,] MakeLettersTable (string keyword, string[] wordbase, out bool correctness)
        {
            bool tempCorrectness;
            correctness = true;
            bool[,] table = new bool[keyword.Length, keyword.Length];
            for (int i = 0; i < keyword.Length; i++)
            {
                tempCorrectness = false;
                for (int j = 0; j <  keyword.Length; j++) 
                {
                    table[i, j] = false;

                    for (int k = 0; k < wordbase[j].Length; k++)
                    {
                        if (keyword[i] == wordbase[j][k])
                        {
                            table[i,j] = true;
                            break;
                        }
                    }
                    if (table[i,j]) { tempCorrectness = true; }
                }
                correctness = correctness && tempCorrectness;
            }
            return table;
        }
        private static bool MakeSequence(bool[,] table, ref int[] sequence)
        {
            for (int i = 0; i < sequence.Length; i++)
            {
                sequence[i] = -1;
            }
            int lastIndex = -1;
            int sum;
            int cntChanges;
            
            while (CheckPresenceInArray(-1, sequence)) {
                cntChanges = 0;
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    sum = 0;
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        if (table[i, j] && !CheckPresenceInArray(j, sequence)) { sum++; lastIndex = j; }
                    }
                    if (sum == 1) { sequence[i] = lastIndex; cntChanges++; }
                }
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    if (CheckPresenceInArray(i, sequence)) { continue; }
                    sum = 0;
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        if (table[j, i] && sequence[j] == -1) { sum++; lastIndex = j; }
                    }
                    if (sum == 1) { sequence[lastIndex] = i; cntChanges++; }
                }
                if (cntChanges == 0) { break; }
            }
            if (!CheckPresenceInArray(-1, sequence))
            {
                return true;
            }
            else
            {
                int[] trySeq = new int[sequence.Length];
                trySeq = MakeSequenceRecursion(table, sequence);
                if (!CheckPresenceInArray(-1, trySeq)) {
                    for (int i = 0; i < sequence.Length; i++)
                    {
                        sequence[i] = trySeq[i];
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        private static int[] MakeSequenceRecursion(bool[,] table, int[] sequence)
        {
            int[] temp = new int[sequence.Length];
            int cnt = 0;
            for (int i = 0; i < table.GetLength(0); i++)
            {
                cnt = 0;
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] && !CheckPresenceInArray(j, sequence)) { temp[cnt++] = j; }
                }
                for (int k = 0; k < cnt; k++)
                {

                    int[] trySeq = new int[sequence.Length];
                    Array.Copy(sequence, trySeq, sequence.Length);
                    trySeq[i] = temp[k];
                    //trySeq = MakeSequenceRecursion(table, trySeq);
                    if (!CheckPresenceInArray(-1, trySeq))
                    {
                        return trySeq;
                    }
                }
            }
            
            return sequence;
        }
        private static bool CheckPresenceInArray (int element, int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == element) return true;
            }
            return false;
        }
        private static bool MakeCrossWord(string keyword, string[] wordbase, out string[] crossword)
        {
            bool correctness;
            bool[,] table = MakeLettersTable(keyword, wordbase, out correctness);
            if (!correctness) { crossword = new string[wordbase.Length]; return false; }
            int[] sequence = new int[keyword.Length];
            for (int i = 0;i < keyword.Length;i++)
            {
                sequence[i] = 0;
            }
            crossword = new string[wordbase.Length];
            if (!MakeSequence(table, ref sequence))
            {
                return false;
            }
            for (int i = 0; i < crossword.Length; i++)
            {
                crossword[i] = wordbase[sequence[i]];
            }
            
            return true;
        }
        private static void PrintCrossword (string keyword, string[] crossword)
        {
            int maxIndex = -1;
            for (int i = 0; i < keyword.Length; i++)
            { 
                for (int j = 0; j < crossword[i].Length; j++)
                {            
                    if (keyword[i] == crossword[i][j])
                    {
                        maxIndex = Math.Max(j, maxIndex);
                    }   
                }
            }
            int temp = 0;
            bool letter;
            for (int i = 0; i < keyword.Length; i++) 
            {
                for (int j = 0;j < crossword[i].Length; j++)
                {
                    if (keyword[i] == crossword[i][j]) { temp = maxIndex - j; break; }
                }
                crossword[i] = new string(' ', temp) + crossword[i];
            }
            
            for (int i = 0; i < keyword.Length; i++)
            {
                letter = true;
                for (int j = 0; j < crossword[i].Length; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (letter && crossword[i][j] == keyword[i]) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        letter = false;
                    }
                    Console.Write(crossword[i][j] + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            string[] wordbase = { "студенец", "журчит", "волк", "владычица", "ртуть", "бабариха", "прутик", "питерк" };
            string keyword = "художник";
            string[] crossword;
            if (!MakeCrossWord(keyword, wordbase, out crossword))
            {
                Console.WriteLine("Ошибка! Невозможно составить кроссворд с такими словами, зато я могу нарисовать вам осьминога");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("  _____\n /     \\\n| () () |\n \\  ^  /\n  |||||\n  |||||");
                Console.ResetColor();
            }
            else
            {
                PrintCrossword(keyword, crossword);
                
            }
        }
    }
}

