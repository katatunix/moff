namespace moff

module CommonParse =

    let parseChaptersInHyperlinks (html : string) (keyBeforeUrl : string) =
        let rec loop acc (i : int) =
            let keyStart = html.IndexOf (keyBeforeUrl, i)
            if keyStart = -1 then acc
            else
                let urlStart = keyStart + keyBeforeUrl.Length
                let urlEnd = html.IndexOf ("\"", urlStart)
                let url = html.Substring(urlStart, urlEnd - urlStart)

                let titleStart = urlEnd + 2
                let titleEnd = html.IndexOf ("</a>", titleStart)
                let title = html.Substring(titleStart, titleEnd - titleStart).Trim()

                let header = { Url = url; Title = title }
                loop (header :: acc) urlEnd
        loop [] 0


    let parseTitleInStandardTag (html : string) =
        let KEY = "<title>"
        let i = (html.IndexOf KEY) + KEY.Length
        let j = html.IndexOf ("</title>", i)
        html.Substring (i, j - i)
