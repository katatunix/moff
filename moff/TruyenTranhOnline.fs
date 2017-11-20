namespace moff

open FSharp.Data

module TruyenTranhOnline =

    let private parseChapterTitle (html : string) =
        let ERROR = "Unknown"
        let i = html.IndexOf "<em class=\"refresh\""
        if i = -1 then ERROR
        else
            let i = html.LastIndexOf ("<span>", i) + 6
            let j = html.IndexOf ("</span>", i)
            html.Substring (i, j - i)

    let private parsePageUrls (html : string) =
        let KEY = "<p class=\"chap-manga-image\"><img src=\""
        let rec loop acc (i : int) =
            let keyStart = html.IndexOf (KEY, i)
            if keyStart = -1 then acc
            else
                let urlStart = keyStart + KEY.Length
                let urlEnd = html.IndexOf ("\"", urlStart)
                if urlEnd = -1 then acc
                else
                    let url = html.Substring (urlStart, urlEnd - urlStart)
                    loop (url :: acc) urlEnd
        loop [] 0
        |> List.rev

    let fetchChapterInfoExn url =
        let html = Http.RequestString url
        {   Header =
                { Url = url; Title = parseChapterTitle html }
            PageUrls =
                parsePageUrls html }

    let fetchChapterInfo url =
        try fetchChapterInfoExn url |> Ok
        with ex -> ex.Message |> Error

    //==============================================================================

    let private parseMangaChapters (html : string) =
        let KEY = "<a class=\"ch sts sts_1\" target=\"_blank\" href=\""
        let rec loop acc (i : int) =
            let keyStart = html.IndexOf (KEY, i)
            if keyStart = -1 then acc
            else
                let urlStart = keyStart + KEY.Length
                let urlEnd = html.IndexOf ("\"", urlStart)
                let url = html.Substring(urlStart, urlEnd - urlStart) + "&load=0"

                let titleStart = urlEnd + 2
                let titleEnd = html.IndexOf ("</a>", titleStart)
                let title = html.Substring(titleStart, titleEnd - titleStart).Trim()

                let header = { Url = url; Title = title }
                loop (header :: acc) urlEnd
        loop [] 0

    let fetchMangaInfoExn url =
        let html = Http.RequestString url
        {   Url = url
            Chapters = parseMangaChapters html }

    let fetchMangaInfo url =
        try fetchMangaInfoExn url |> Ok
        with ex -> ex.Message |> Error
