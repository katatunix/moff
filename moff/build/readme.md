# moff - a tool for downloading manga from famous websites

## Support websites:
* truyentranhonline.vn
* truyentranhtuan.com

## Usage:
* Go to `/moff/build` folder.
* Open Command Prompt (Windows) or Terminal (macOS, Linux).
* Run `moff.exe` with parameters as described below:

```
moff.exe (chapter|manga|manga-info) <params>
    chapter <chapterUrl> <mangaFolder>
    manga <mangaUrl> <mangaFolder>
    manga <mangaUrl> <mangaFolder> <fromChapter> <toChapter>
    manga-info <mangaUrl>
Examples:
    moff.exe chapter "http://truyentranhonline.vn/yaiba/?id=21809&load=0" D:/manga/yaiba
    moff.exe manga http://truyentranhonline.vn/yaiba D:/manga/yaiba
    moff.exe manga http://truyentranhonline.vn/yaiba D:/manga/yaiba 3 7
    moff.exe manga-info http://truyentranhonline.vn/yaiba
```
* In order to know the values of `<fromChapter>` and `<toChapter>` you should run `moff.exe manga-info <mangaUrl>` first.
* Be careful with `&` and spaces, if the url (or any parameter in general) contains those symbols, it must be rounded with a couple of `"`, as in the first example above.
