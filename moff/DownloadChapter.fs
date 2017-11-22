namespace moff

open System.IO
open NghiaBui.Common.Parallel

module DownloadChapter =

    let exec jobs (mangaFolder : string) onPageDone (info : ChapterInfo) =
        let folder = Path.Combine (mangaFolder, info.Header.Title)
        Directory.CreateDirectory folder |> ignore

        File.WriteAllLines (
            Path.Combine (folder, "links.txt"),
            info.Header.Url :: info.PageUrls )

        let enum = (info.PageUrls :> seq<string>).GetEnumerator ()
        let mutable index = 0
        let next () = lock enum (fun _ ->
            if enum.MoveNext () then
                index <- index + 1
                Some (enum.Current, index)
            else
                None)

        let o = obj ()
        let total = info.PageUrls.Length
        runParallel jobs (fun _ ->
            let rec loop () =
                match next () with
                | None -> ()
                | Some (pageUrl, index) ->
                    let result = DownloadPage.execHardly pageUrl folder index
                    lock o (fun _ -> onPageDone index total pageUrl result)
                    loop ()
            loop ())
