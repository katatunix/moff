namespace moff

module WebSite =

    let detect (url : string) : Parser =
        if url.Contains "truyentranhonline.vn" then
            TruyenTranhOnline () :> Parser
        elif url.Contains "truyentranhtuan.com" then
            TruyenTranhTuan () :> Parser
        else
            TruyenTranhOnline () :> Parser
