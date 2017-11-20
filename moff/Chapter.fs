namespace moff

open System.IO
open NghiaBui.Common.Parallel

module Chapter =

    let download jobs (baseFolder : string) onPageDone (chapter : Chapter) =
        let folder = Path.Combine (baseFolder, chapter.Header.Title)
        Directory.CreateDirectory(folder) |> ignore

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
                    match Page.downloadHardly pageUrl folder index with
                    | Ok _ ->
                        lock oDone (fun _ -> onPageDone index total pageUrl (Ok ()))
                    | Error msg ->
                        lock oDone (fun _ -> onPageDone index total pageUrl (Error msg))
                    loop ()
            loop ())
