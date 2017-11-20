namespace moff

[<AutoOpen>]
module Domain =

    type Header = {
        Url : string
        Title : string }

    type Chapter = {
        Header : Header
        PageUrls : string list }

    type Manga = {
        Url : string
        Chapters : Header list }
