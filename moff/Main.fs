namespace moff

open NghiaBui.Common.ActivePatterns
open System
open MyConsole

module Main =

    let printAbout () =
        Console.ForegroundColor <- ConsoleColor.Green
        Console.WriteLine "==========================================================="
        Console.WriteLine "              moff 1.0 - katatunix@gmail.com"
        Console.WriteLine "==========================================================="
        Console.ResetColor ()

    let printUsage () =
        infoln "Usage: moff.exe (chapter|manga|manga-info) <params>"
        infoln "    chapter <baseFolder> <url>"
        infoln "    manga <baseFolder> url"
        infoln "    manga <baseFolder> url <fromChapter> <toChapter>"
        infoln "    manga-info url"

    [<EntryPoint>]
    let main argv =
        printAbout ()

        match argv with

        | [| "chapter"; baseFolder; url |] ->
            Uc.downloadChapter baseFolder url

        | [| "manga"; baseFolder; url |] ->
            Uc.downloadManga baseFolder url 1 System.Int32.MaxValue

        | [| "manga"; baseFolder; url; Int fromChapter; Int toChapter |] ->
            Uc.downloadManga baseFolder url fromChapter toChapter

        | [| "manga-info"; url |] ->
            Uc.viewMangaInfo url

        | _ ->
            printUsage ()
        
        0
