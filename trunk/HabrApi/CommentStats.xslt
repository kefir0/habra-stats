<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:my="urn:sample">

  <msxsl:script language="C#" implements-prefix="my">
    public string Now()
    {
      return DateTime.Now.ToString();
    }
  </msxsl:script>
  
  <xsl:template match="/Report">
    <html>
      <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>
          <xsl:value-of select="Title"/> - Хабрастатистика
        </title>
        <link href="comments.css" rel="stylesheet" media="all" />
        <script type="text/javascript" src="jquery-1.8.2.min.js">;</script>
        <style type="text/css">
          input, label { cursor: hand; cursor: pointer; }
        </style>
      </head>
      <body>

        <!-- Header with selection -->
        <div style="width:80%; margin-left:auto; margin-right:auto;display:block;">
          <h2 style="display:inline; float:right; margin-top:37px; color: gray;" >Топ комментариев <span style="color: red;">TM</span></h2>

          <xsl:for-each select="//ArrayOfCommentReportAttribute">
            <div style="display:inline; float:left; margin-right:30px;">
              <form>
                <xsl:for-each select="CommentReportAttribute">
                  <input type="radio" name="{Category}" value="{Value}" id="{Value}">
                    <xsl:if test="position() = 1">
                      <xsl:attribute name="checked">yes</xsl:attribute>
                    </xsl:if>
                  </input>
                  <label for="{Value}">
                    <xsl:value-of select="Name" />
                  </label>
                  <br />
                </xsl:for-each>
              </form>
            </div>
          </xsl:for-each>
        </div>

        <script>
          <xsl:text disable-output-escaping="yes">
            function initState()
            {
              $("input").map(function(){
                if (document.URL.indexOf(this.value) > 0)
                {
                  this.checked=true;
                  var selector = "label[for='"+this.id+"']";
                  $(selector).css("font-weight", "bold");
                }
              });

              $("input").click(function(event){
                var res = $("input:checked").map(function() {
                  return this.id;
                }).get().join('_') + ".html";
                window.location.href = res;
              });

            }

            initState();
          </xsl:text>
        </script>

        <div style="clear:both;" />
        <hr style="width:83%" />

        <!-- Comments -->
        <div class="comments_list" style="width:80%; margin-left:auto; margin-right:auto;">
          <xsl:for-each select="//Comment">

            <div class="comment_item">
              <div class="info  ">
                <div class="voting   ">
                  <xsl:if test="Score &gt; 0">
                    <div class="mark positive">
                      <span class="score" title="Всего {Score}: &#8593;{ScorePlus} и &#8595;{ScoreMinus}">
                        <xsl:value-of select="Score"/>
                      </span>
                    </div>
                  </xsl:if>
                  <xsl:if test="Score &lt; 0">
                    <div class="mark negative">
                      <span class="score" title="Всего {Score}: &#8593;{ScorePlus} и &#8595;{ScoreMinus}">
                        <xsl:value-of select="Score"/>
                      </span>
                    </div>
                  </xsl:if>
                </div>

                <a class="avatar" href="http://habrahabr.ru/users/{UserName}/">
                  <img src="{Avatar}" />
                </a>

                <a class="username" href="http://habrahabr.ru/users/{UserName}/">
                  <xsl:value-of select="UserName"/>
                </a>

                <span class="comma">,</span>
                <time>
                  <xsl:value-of select="Date"/>
                </time>

                <a class="link_to_comment" href="{Url}">#</a>

                <a class="link_to_comment" href="{PostUrl}">
                  <xsl:value-of select="PostTitle"/>
                </a>
              </div>
              <div style="clear:both;" />
              <div class="message html_format">
                <xsl:value-of select="Text" disable-output-escaping="yes" />
              </div>
            </div>

            <p/>
          </xsl:for-each>
        </div>
        <p/>

        <div style="color:gray; text-align:center;">
          Generated: <xsl:value-of select="my:Now()"/><br />
          Created by kefir. naxah1 at gmail. Source: <a href="http://code.google.com/p/habra-stats/source/browse/">code.google.com/p/habra-stats</a><br />
          Хабрастатья: <a href="http://habrahabr.ru/post/155541/">Топ комментариев Хабра — сервис, детали реализации, и немного статистики</a>
          <!-- Yandex.Metrika counter -->
          <script type="text/javascript">
            <xsl:text disable-output-escaping="yes">
            (function (d, w, c) {
            (w[c] = w[c] || []).push(function() {
            try {
            w.yaCounter17855674 = new Ya.Metrika({id:17855674,
            trackLinks:true,
            accurateTrackBounce:true});
            } catch(e) { }
            });

            var n = d.getElementsByTagName("script")[0],
            s = d.createElement("script"),
            f = function () { n.parentNode.insertBefore(s, n); };
            s.type = "text/javascript";
            s.async = true;
            s.src = (d.location.protocol == "https:" ? "https:" : "http:") + "//mc.yandex.ru/metrika/watch.js";

            if (w.opera == "[object Opera]") {
            d.addEventListener("DOMContentLoaded", f);
            } else { f(); }
            })(document, window, "yandex_metrika_callbacks");
          </xsl:text>
          </script>
          <noscript>
            <div>
              <img src="//mc.yandex.ru/watch/17855674" style="position:absolute; left:-9999px;" alt="" />
            </div>
          </noscript>
          <!-- /Yandex.Metrika counter -->
        </div>
      </body>
      
    </html>
  </xsl:template>
</xsl:stylesheet>