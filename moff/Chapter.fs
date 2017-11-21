namespace moff

open System.IO
open FSharp.Data
open NghiaBui.Common.Parallel

module Chapter =

    let download jobs (mangaFolder : string) onPageDone (chapter : Chapter) =
        let folder = Path.Combine (mangaFolder, chapter.Header.Title)
        Directory.CreateDirectory folder |> ignore

        let enum = (chapter.PageUrls :> seq<string>).GetEnumerator ()
        let mutable index = 0
        let next () = lock enum (fun _ ->
            if enum.MoveNext () then
                index <- index + 1
                Some (enum.Current, index)
            else
                None)

        let oDone = obj ()
        let total = chapter.PageUrls.Length
        runParallel jobs (fun _ ->
            let rec loop () =
                match next () with
                | None -> ()
                | Some (pageUrl, index) ->
                    let result = Page.downloadHardly pageUrl folder index
                    lock oDone (fun _ -> onPageDone index total pageUrl result)
                    loop ()
            loop ())

    let fetchInfoExn url (parser : Parser) =
        let html = Http.RequestString url

        let title = try parser.ParseChapterTitle html
                    with _ -> failwith "Could not parse chapter title"

        let pageUrls =  try parser.ParsePageUrls html
                        with _ -> failwith "Could not parse page urls of the chapter"

        {   Header = { Url = url; Title = title }
            PageUrls = pageUrls }

    let fetchInfo url parser =
        try
            fetchInfoExn url parser |> Ok
        with ex ->
            ex.Message |> Error
