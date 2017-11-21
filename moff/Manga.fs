namespace moff

open FSharp.Data

module Manga =

    let fetchInfoExn url (parser : Parser) =
        let html = Http.RequestString url
        let chapters =  try parser.ParseChapters html
                        with _ -> failwith "Could not parse chapters of the manga"
        {   Url = url
            Chapters = chapters }

    let fetchInfo url parser =
        try fetchInfoExn url parser |> Ok
        with ex -> ex.Message |> Error
