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
        infoln "    chapter <mangaFolder> <chapterUrl>"
        infoln "    manga <mangaFolder> <mangaUrl>"
        infoln "    manga <mangaFolder> <mangaUrl> <fromChapter> <toChapter>"
        infoln "    manga-info <mangaUrl>"
        infoln "Examples:"
        infoln "    moff.exe chapter D:/manga/yaiba http://truyentranhonline.vn/yaiba/?id=21809&load=0"
        infoln "    moff.exe manga D:/manga/yaiba http://truyentranhonline.vn/yaiba"
        infoln "    moff.exe manga D:/manga/yaiba http://truyentranhonline.vn/yaiba 3 7"
        infoln "    moff.exe manga-info http://truyentranhonline.vn/yaiba"
        infoln "Supported websites:"
        infoln "    truyentranhonline.vn"
        infoln "    truyentranhtuan.com"

    [<EntryPoint>]
    let main argv =
        printAbout ()

        match argv with

        | [| "chapter"; mangaFolder; chapterUrl |] ->
            Uc.downloadChapter mangaFolder chapterUrl

        | [| "manga"; mangaFolder; mangaUrl |] ->
            Uc.downloadManga mangaFolder mangaUrl 1 System.Int32.MaxValue

        | [| "manga"; mangaFolder; mangaUrl; Int fromChapter; Int toChapter |] ->
            Uc.downloadManga mangaFolder mangaUrl fromChapter toChapter

        | [| "manga-info"; mangaUrl |] ->
            Uc.viewMangaInfo mangaUrl

        | _ ->
            printUsage ()
        
        0
