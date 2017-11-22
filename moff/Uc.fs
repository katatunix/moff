namespace moff

open System.IO
open NghiaBui.Common.Monads.Rop
open MyConsole

module Uc =

    let private firstLine (msg : string) =
        let i = msg.IndexOf "\n"
        if i = -1 then msg else msg.Substring (0, i)

    let downloadChapter mangaFolder url =
        sprintf "Chapter url: %s" url |> infoln
        sprintf "Fetch chapter info ..." |> infoln

        rop {
            let! parser = WebSite.detect url
            let! html = fetchHtml url
            return! parseChapterInfo url html parser }
        
        |> function
        | Error msg ->
            sprintf "[ERROR] %s" msg |> errorln

        | Ok chapterInfo ->
            sprintf "Chapter title: %s" chapterInfo.Header.Title |> infoln
            sprintf "Page number: %d" chapterInfo.PageUrls.Length |> infoln
            sprintf "Download pages ..." |> infoln
            chapterInfo
            |> DownloadChapter.exec
                System.Environment.ProcessorCount
                mangaFolder
                (fun index total pageUrl result ->
                    sprintf "Page %d/%d. " index total |> green
                    match result with
                    | Ok _ ->
                        sprintf "%s [OK]" pageUrl |> infoln
                    | Error msg ->
                        sprintf "%s " pageUrl |> info; "[ERROR]" |> errorln
                        msg |> firstLine |> errorln)

    let private processManga url cont =
        sprintf "Manga url: %s" url |> infoln
        infoln "Fetch manga info ..."

        rop {
            let! parser = WebSite.detect url
            let! html = fetchHtml url
            return! parseMangaInfo url html parser }

        |> function
        | Error msg ->
            sprintf "[ERROR] %s" msg |> errorln
        | Ok mangaInfo ->
            sprintf "Chapter number: %d" mangaInfo.ChapterHeaders.Length |> infoln
            cont mangaInfo

    let downloadManga mangaFolder url fromChapter toChapter =
        processManga url (fun mangaInfo ->
            Directory.CreateDirectory mangaFolder |> ignore
            File.WriteAllText (Path.Combine (mangaFolder, "link.txt"), url)

            sprintf "Download chapters ..." |> infoln
            let total = mangaInfo.ChapterHeaders.Length
            mangaInfo.ChapterHeaders
            |> List.iteri (fun i { Url = chapterUrl } ->
                let i = i + 1
                if fromChapter <= i && i <= toChapter then
                    sprintf "==================================" |> greenln
                    sprintf "Chapter %d/%d" i total |> greenln
                    sprintf "==================================" |> greenln
                    downloadChapter mangaFolder chapterUrl))

    let viewMangaInfo url =
        processManga url (fun mangaInfo ->
            let total = mangaInfo.ChapterHeaders.Length
            mangaInfo.ChapterHeaders
            |> List.iteri (fun i chapterHeader ->
                sprintf "Chapter %d/%d. " (i + 1) total |> green
                chapterHeader.Title + " " |> info
                chapterHeader.Url |> warnln))
