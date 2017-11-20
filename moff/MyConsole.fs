namespace moff

open System

module MyConsole =

    let infoln (s : string) =
        Console.WriteLine s

    let errorln (s : string) =
        Console.ForegroundColor <- ConsoleColor.Red
        Console.WriteLine s
        Console.ResetColor ()
