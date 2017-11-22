namespace moff

open System.IO
open NghiaBui.Common.Monads.Rop
open MyConsole

module UcChapter =

    let private firstLine (msg : string) =
        let i = msg.IndexOf "\n"
        if i = -1 then msg else msg.Substring (0, i)

    let download url mangaFolder =
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

            let chapterFolder = Path.Combine (mangaFolder, chapterInfo.Header.Title)
            Directory.CreateDirectory chapterFolder |> ignore
            File.WriteAllLines (
                Path.Combine (chapterFolder, "links.txt"),
                chapterInfo.Header.Url :: chapterInfo.PageUrls )

            sprintf "Download pages ..." |> infoln
            chapterInfo.PageUrls
            |> DownloadManyPages.exec
                System.Environment.ProcessorCount
                chapterFolder
                (fun index total pageUrl result ->
                    sprintf "Page %d/%d. " index total |> green
                    match result with
                    | Ok _ ->
                        sprintf "%s [OK]" pageUrl |> infoln
                    | Error msg ->
                        sprintf "%s " pageUrl |> info; "[ERROR]" |> errorln
                        msg |> firstLine |> errorln)
