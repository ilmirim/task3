using System;
using System.Collections.Generic;
using System.Collections;

class Base
{
    public static void Main(String[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n\n\n\t\t Start point of program \n");

        Console.Write("\t Enter N: ");
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        long N = long.Parse(Console.ReadLine());
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;

        decimal parResult = parallelCalculateRowSum(function, N);
        decimal result = calcRow(function, 0, N);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\n\t\t Parallel result is ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.Write($" {parResult} ");
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"\n\t\t Result is {result}\n\n\n\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static decimal parallelCalculateRowSum(Func<long, decimal> func, long N)
    {
        int procCount = Environment.ProcessorCount;               
        Task[] tasks = new Task[procCount];
        long step = N / procCount;
        decimal sum = 0;
        Console.WriteLine($"\n\t {procCount} - processor count" +
            $"\n\t {step} - step");
        object locker = new();
        for (int i = 0; i < tasks.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            var start = step * i;
            Console.WriteLine($"\t start is {start}");
            tasks[i] = Task.Run(() =>
            {
                var localSum = calcRow(func, start, start + step);
                lock(locker)
                {
                    sum += localSum;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"\t task[{i}] : task \t summ : {sum}");
                }
            });
        }
        Task.WaitAll(tasks);
        return sum;
    }

    private static decimal calcRow(Func<long, decimal> func, long start, long end)
    {
        decimal sum = 0;
        for (long j = start; j < end; j++)
        {
            if (j == 0) continue;
            sum += func(j);
        }
        return sum;
    }

    private static decimal function(long n)
    {
        decimal first = (3*n)-2;
        decimal second = n*(n+1)*(n+2);
        return first/second;
    }
}