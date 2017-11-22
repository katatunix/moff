namespace moff

open NghiaBui.Common.ActivePatterns
open System
open MyConsole

module Main =

    let printAbout () =
        greenln "╔════════════════════════════════════════════════════════════════╗"
        greenln "║                 moff 1.0 - katatunix@gmail.com                 ║"
        greenln "╚════════════════════════════════════════════════════════════════╝"
        greenln ""

    let printUsage () =
        infoln "Usage: moff.exe (chapter|manga|manga-info) <params>"
        infoln "    chapter <chapterUrl> <mangaFolder>"
        infoln "    manga <mangaUrl> <mangaFolder>"
        infoln "    manga <mangaUrl> <mangaFolder> <fromChapter> <toChapter>"
        infoln "    manga-info <mangaUrl>"
        infoln "Examples:"
        infoln "    moff.exe chapter http://truyentranhonline.vn/yaiba/?id=21809&load=0 D:/manga/yaiba"
        infoln "    moff.exe manga http://truyentranhonline.vn/yaiba D:/manga/yaiba"
        infoln "    moff.exe manga http://truyentranhonline.vn/yaiba D:/manga/yaiba 3 7"
        infoln "    moff.exe manga-info http://truyentranhonline.vn/yaiba"
        infoln "Supported websites:"
        infoln "    truyentranhonline.vn"
        infoln "    truyentranhtuan.com"

    [<EntryPoint>]
    let main argv =
        printAbout ()

        match argv with

        | [| "chapter"; chapterUrl; mangaFolder |] ->
            UcChapter.download chapterUrl mangaFolder

        | [| "manga"; mangaUrl; mangaFolder |] ->
            UcManga.download mangaUrl mangaFolder 1 System.Int32.MaxValue

        | [| "manga"; mangaUrl; mangaFolder; Int fromChapter; Int toChapter |] ->
            UcManga.download mangaUrl mangaFolder fromChapter toChapter

        | [| "manga-info"; mangaUrl |] ->
            UcManga.viewInfo mangaUrl

        | _ ->
            printUsage ()
        
        0
