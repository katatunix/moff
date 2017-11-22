# moff - a tool for downloading manga from famous websites

## Support websites:
* truyentranhonline.vn
* truyentranhtuan.com

## Usage:
* Go to `/moff/build` folder
* Open Command Prompt (Windows) or Terminal (macOS, Linux)
* Run `moff.exe` with parameters as described below:

```
moff.exe (chapter|manga|manga-info) <params>
    chapter <mangaFolder> <chapterUrl>
    manga <mangaFolder> <mangaUrl>
    manga <mangaFolder> <mangaUrl> <fromChapter> <toChapter>
    manga-info <mangaUrl>
Examples:
    moff.exe chapter D:/manga/yaiba http://truyentranhonline.vn/yaiba/?id=21809&load=0
    moff.exe manga D:/manga/yaiba http://truyentranhonline.vn/yaiba
    moff.exe manga D:/manga/yaiba http://truyentranhonline.vn/yaiba 3 7
    moff.exe manga-info http://truyentranhonline.vn/yaiba
```
* In order to know the values of `<fromChapter>` and `<toChapter>` you should run `moff.exe manga-info <mangaUrl>` first.
