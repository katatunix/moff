namespace moff

open MyConsole

module Uc =

    let private firstLine (msg : string) =
        let i = msg.IndexOf "\n"
        if i = -1 then msg else msg.Substring (0, i)

    let downloadChapter mangaFolder url =
        sprintf "Chapter url: %s" url |> infoln
        sprintf "Fetch chapter info ..." |> infoln

        WebSite.detect url
        |> Chapter.fetchInfo url
        |> function
        | Error msg ->
            errorln "[ERROR] Could not fetch chapter info"
            sprintf "%s" msg |> errorln

        | Ok chapter ->
            sprintf "Chapter title: %s" chapter.Header.Title |> infoln
            sprintf "Page number: %d" chapter.PageUrls.Length |> infoln
            sprintf "Download pages ..." |> infoln
            chapter
            |> Chapter.download
                System.Environment.ProcessorCount
                mangaFolder
                (fun index total pageUrl result ->
                    sprintf "Page %d/%d. " index total |> green
                    match result with
                    | Ok _ ->
                        sprintf "%s [OK]" pageUrl |> infoln
                    | Error msg ->
                        sprintf "%s " pageUrl |> info
                        "[ERROR]" |> errorln
                        msg |> firstLine |> errorln)

    let private processManga url cont =
        sprintf "Manga url: %s" url |> infoln
        infoln "Fetch manga info ..."

        WebSite.detect url
        |> Manga.fetchInfo url
        |> function
        | Error msg ->
            errorln "[ERROR] Could not fetch manga info"
            sprintf "%s" msg |> errorln
        | Ok manga ->
            sprintf "Chapter number: %d" manga.Chapters.Length |> infoln
            cont manga

    let downloadManga mangaFolder url fromChapter toChapter =
        processManga url (fun manga ->
            sprintf "Download chapters ..." |> infoln
            let total = manga.Chapters.Length
            manga.Chapters
            |> List.iteri (fun i { Url = chapterUrl } ->
                let i = i + 1
                if fromChapter <= i && i <= toChapter then
                    sprintf "================================================================" |> greenln
                    sprintf "Chapter %d/%d" i total |> greenln
                    sprintf "================================================================" |> greenln
                    downloadChapter mangaFolder chapterUrl))

    let viewMangaInfo url =
        processManga url (fun manga ->
            let total = manga.Chapters.Length
            manga.Chapters
            |> List.iteri (fun i chapter ->
                sprintf "Chapter %d/%d. " (i + 1) total |> green
                chapter.Title + " " |> info
                chapter.Url |> warnln))
