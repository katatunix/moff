namespace moff

open System
open NghiaBui.Common.Sorting

type TruyenTranhTuan () =
    interface Parser with

        member this.ParseChapters (html : string) =
            CommonParse.parseChaptersInHyperlinks
                html
                "<span class=\"chapter-name\">\n<a href=\""
        
        member this.ParseChapterTitle (html : string) =
            CommonParse.parseTitleInStandardTag html

        member this.ParsePageUrls (html : string) =
            let KEY = "var slides_page_url_path = [\""
            let i = (html.IndexOf KEY) + KEY.Length
            let j = html.IndexOf ("\"];", i)
            let list =
                html.Substring(i, j - i).Split([| "\",\"" |], StringSplitOptions.RemoveEmptyEntries)
                |> Array.map (fun url -> url.Trim ())
                |> System.Collections.Generic.List
            if not (html.Contains "use_server_gg = true") then
                qsortList list (fun a b -> a.CompareTo b < 0) |> ignore
            List.init list.Count (fun i -> list.[i])
