namespace moff

module WebSite =

    let detect (url : string) =
        if url.Contains "truyentranhonline.vn" then
            (TruyenTranhOnline.fetchChapterInfo, TruyenTranhOnline.fetchMangaInfo)
        else
            (TruyenTranhOnline.fetchChapterInfo, TruyenTranhOnline.fetchMangaInfo)
