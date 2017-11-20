namespace moff

open System

module MyConsole =

    let private out (s : string) write color =
        Console.ForegroundColor <- color
        write s
        Console.ResetColor ()

    let infoln (s : string) =
        Console.WriteLine s

    let info (s : string) =
        Console.Write s

    let errorln (s : string) =
        out s Console.WriteLine ConsoleColor.Red

    let warnln (s : string) =
        out s Console.WriteLine ConsoleColor.Yellow

    let debugln (s : string) =
        out s Console.WriteLine ConsoleColor.Gray

    let greenln (s : string) =
        out s Console.WriteLine ConsoleColor.Green

    let green (s : string) =
        out s Console.Write ConsoleColor.Green
