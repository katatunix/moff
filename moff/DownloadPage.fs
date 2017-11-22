namespace moff

open FSharp.Data
open System.IO
open NghiaBui.Common.Misc

module DownloadPage =

    let private padZero (i : int) =
        let rec loop (s : string) = if s.Length >= 4 then s else loop ("0" + s)
        loop (string i)

    let private getType (headers : Map<string, string>) =
        match headers.TryFind "Content-Type" with
        | None -> "png"
        | Some contentType ->
            ["png"; "jpg"; "jpeg"; "gif"]

            |> List.tryFind contentType.Contains
            |> Option.defaultValue "png"

    let execExn url folder index =
        let res = Http.Request (url, timeout = 30000)
        let _type = getType res.Headers
        let _name = (padZero index) + "." + _type
        let _path = Path.Combine (folder, _name)

        match res.Body with
        | Text text ->
            File.WriteAllText (_path, text)
        | Binary bytes ->
            File.OpenWrite(_path).Write(bytes, 0, bytes.Length)

    let exec url folder index =
        try execExn url folder index |> Ok
        with ex -> Error ex.Message

    let execHardly url folder index =
        tryHard 3 1000 (fun _ -> execExn url folder index)
