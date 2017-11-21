namespace moff

type Parser =
    abstract ParseChapters      : string -> Header list
    abstract ParseChapterTitle  : string -> string
    abstract ParsePageUrls      : string -> string list
