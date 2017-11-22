namespace moff

open System.IO
open NghiaBui.Common.Monads.Rop
open MyConsole

module UcManga =

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

    let download url mangaFolder fromChapter toChapter =
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
                    UcChapter.download chapterUrl mangaFolder))

    let viewInfo url =
        processManga url (fun mangaInfo ->
            let total = mangaInfo.ChapterHeaders.Length
            mangaInfo.ChapterHeaders
            |> List.iteri (fun i chapterHeader ->
                sprintf "Chapter %d/%d. " (i + 1) total |> green
                chapterHeader.Title + " " |> info
                chapterHeader.Url |> warnln))
