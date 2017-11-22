namespace moff

module WebSite =

    let detect (url : string) : Result<Parser, string> =
        if url.Contains "truyentranhonline.vn" then
            TruyenTranhOnline () :> Parser |> Ok
        elif url.Contains "truyentranhtuan.com" then
            TruyenTranhTuan () :> Parser |> Ok
        else
            Error "Not support this website"
