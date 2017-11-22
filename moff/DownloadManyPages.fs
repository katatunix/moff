namespace moff

open NghiaBui.Common.Parallel

module DownloadManyPages =

    let exec jobs (folder : string) onPageDone (urls : string list) =
        let enum = (urls :> seq<string>).GetEnumerator ()
        let mutable index = 0
        let next () = lock enum (fun _ ->
            if enum.MoveNext () then
                index <- index + 1
                Some (enum.Current, index)
            else
                None)

        let o = obj ()
        let total = urls.Length
        runParallel jobs (fun _ ->
            let rec loop () =
                match next () with
                | None -> ()
                | Some (pageUrl, index) ->
                    let result = DownloadPage.exec pageUrl folder index
                    lock o (fun _ -> onPageDone index total pageUrl result)
                    loop ()
            loop ())
