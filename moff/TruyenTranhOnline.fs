namespace moff

open System

type TruyenTranhOnline () =
    interface Parser with

        member this.ParseChapters (html : string) =
            CommonParse.parseChaptersInHyperlinks
                html
                "<a class=\"ch sts sts_1\" target=\"_blank\" href=\""
            |> List.map (fun header -> { header with Url = header.Url + "&load=0" })

        member this.ParseChapterTitle (html : string) =
            let i = html.IndexOf "<em class=\"refresh\""
            let i = html.LastIndexOf ("<span>", i) + 6
            let j = html.IndexOf ("</span>", i)
            html.Substring (i, j - i)

        member this.ParsePageUrls (html : string) =
            let KEY = "<p class=\"chap-manga-image\"><img src=\""
            let rec loop acc (i : int) =
                let keyStart = html.IndexOf (KEY, i)
                if keyStart = -1 then acc
                else
                    let urlStart = keyStart + KEY.Length
                    let urlEnd = html.IndexOf ("\"", urlStart)
                    let url = html.Substring (urlStart, urlEnd - urlStart)
                    loop (url :: acc) urlEnd
            loop [] 0
            |> List.rev
