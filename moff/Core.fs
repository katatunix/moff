namespace moff

open FSharp.Data
open NghiaBui.Common.Misc

[<AutoOpen>]
module Core =

    type Header = {
        Url : string
        Title : string }

    type ChapterInfo = {
        Header : Header
        PageUrls : string list }

    type MangaInfo = {
        Url : string
        ChapterHeaders : Header list }

    type Parser =
        abstract ParseChapters      : string -> Header list
        abstract ParseChapterTitle  : string -> string
        abstract ParsePageUrls      : string -> string list

    let fetchHtml url =
        (fun _ -> Http.RequestString url) |> toResult

    let parseChapterInfoExn url html (parser : Parser) =
        let title = try parser.ParseChapterTitle html
                    with _ -> failwith "Could not parse chapter title"

        let pageUrls =  try parser.ParsePageUrls html
                        with _ -> failwith "Could not parse page urls of the chapter"

        {   Header = { Url = url; Title = title }
            PageUrls = pageUrls }

    let parseChapterInfo url html parser =
        (fun _ -> parseChapterInfoExn url html parser) |> toResult
    
    let parseMangaInfoExn url html (parser : Parser) =
        let chapters =  try parser.ParseChapters html
                        with _ -> failwith "Could not parse chapters of the manga"
        {   Url = url
            ChapterHeaders = chapters }

    let parseMangaInfo url html parser =
        (fun _ -> parseMangaInfoExn url html parser) |> toResult
