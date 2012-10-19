<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:my="urn:sample">

  <msxsl:script language="C#" implements-prefix="my">
    public string Now()
    {
      return DateTime.Now.ToString();
    }
  </msxsl:script>
  
  <xsl:template match="/">
    <html>
      <head>
        <meta http-equiv="Content-Type" content="text/html; charset=windows-1251" />
        <title>Habrahabr top comments</title>
        <link href="comments.css" rel="stylesheet" media="all" />
      </head>

      <body>
        <div class="comments_list" style="width:80%; margin-left:auto; margin-right:auto;">
          <xsl:for-each select="//Comment">
          
            <div class="comment_item">
              <div class="info  ">
                <div class="voting   ">
                  <div class="mark positive">
                    <span class="score">
                      <xsl:value-of select="Score"/>
                    </span>
                  </div>
                  <!--<div class="mark  negative">
                    <span class="score" title="Всего 4: &uarr;1 и &darr;3">–2</span>
                  </div>-->
                </div>

                <a class="avatar">
                  <xsl:attribute name="href">
                    http://habrahabr.ru/<xsl:value-of select="UserName"/>/
                  </xsl:attribute>
                  <img>
                    <xsl:attribute name="src">
                      <xsl:value-of select="Avatar"/>
                    </xsl:attribute>
                  </img>
                </a>
                
                <a class="username">
                  <xsl:attribute name="href">
                    http://habrahabr.ru/<xsl:value-of select="UserName"/>/
                  </xsl:attribute>
                  <xsl:value-of select="UserName"/>
                </a>

                <span class="comma">,</span>
                <time>
                  <xsl:value-of select="Date"/>
                </time>

                <a class="link_to_comment">
                  <xsl:attribute name="href">
                    <xsl:value-of select="Url"/>
                  </xsl:attribute>
                  #
                </a>

                <a class="link_to_comment">
                  <xsl:attribute name="href">
                    <xsl:value-of select="PostUrl"/>
                  </xsl:attribute>
                  <xsl:value-of select="PostTitle"/>
                </a>
              </div>
              <br />
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
          Created by kefir. naxah1 at gmail. Source: <a href="http://code.google.com/p/habra-stats/source/browse/">code.google.com/p/habra-stats</a>
        </div>
      </body>
      
    </html>
  </xsl:template>
</xsl:stylesheet>