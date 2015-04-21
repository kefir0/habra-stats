# habra-stats
habrahabr.ru/geektimes.ru/megamozg.ru top comments website.

Comments often provide more information than articles. Habrahabr.ru used to have comments rating pages, but then they were removed.
So I wrote my own, with ~~blackjack and h~~ C# and SQL.

How it works: windows service runs on my home server
* periodically parses target sites
* stores comments in MS SQL Express db
* generates static HTML content and uploads it to a free web hosting

And yes, I do use [RegEx to parse HTML](http://stackoverflow.com/questions/1732348/regex-match-open-tags-except-xhtml-self-contained-tags/1732454#1732454), because this project started as a quick-and-dirty LINQPad snippet (LINQPad is amazing, try it!).
