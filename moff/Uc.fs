namespace moff

open MyConsole

module Uc =

    let private cleanErrorMsg (msg : string) =
        let i = msg.IndexOf "\n"
        if i = -1 then msg else msg.Substring (0, i)

    let downloadChapter baseFolder url =
        sprintf "Chapter url: %s" url |> infoln
        sprintf "Fetch chapter info ..." |> infoln
        match url |> (WebSite.detect url |> fst) with
        | Error msg ->
            errorln "[ERROR] Could not fetch chapter info"

        | Ok chapter ->
            sprintf "Chapter title: %s" chapter.Header.Title |> infoln
            sprintf "Page number: %d" chapter.PageUrls.Length |> infoln
            sprintf "Download pages ..." |> infoln

            chapter
            |> Chapter.download
                    System.Environment.ProcessorCount
                    baseFolder
                    (fun index total pageUrl result ->
                        sprintf "Page %d/%d. " index total |> green
                        match result with
                        | Ok _ ->
                            sprintf "%s [OK]" pageUrl |> infoln
                        | Error msg ->
                            sprintf "%s " pageUrl |> info
                            "[ERROR]" |> errorln
                            msg |> cleanErrorMsg |> errorln )

    let private processManga url f =
        sprintf "Manga url: %s" url |> infoln
        sprintf "Fetch manga info ..." |> infoln
        match url |> (WebSite.detect url |> snd) with
        | Error msg ->
            errorln "[ERROR] Could not fetch manga info"

        | Ok manga ->
            sprintf "Chapter number: %d" manga.Chapters.Length |> infoln

            f manga

    let downloadManga baseFolder url fromChapter toChapter =
        processManga url (fun manga ->
            let chapterNum = manga.Chapters.Length
            sprintf "Download chapters ..." |> infoln
            manga.Chapters
            |> List.iteri (fun i { Url = chapterUrl } ->
                let i = i + 1
                if fromChapter <= i && i <= toChapter then
                    sprintf "================================================================" |> greenln
                    sprintf "Chapter %d/%d" i chapterNum |> greenln
                    sprintf "================================================================" |> greenln
                    downloadChapter baseFolder chapterUrl))

    let viewMangaInfo url =
        processManga url (fun manga ->
            let chapterNum = manga.Chapters.Length
            manga.Chapters
            |> List.iteri (fun i chapter ->
                sprintf "Chapter %d/%d. " (i + 1) chapterNum |> green
                chapter.Title + " " |> info
                chapter.Url |> warnln))
