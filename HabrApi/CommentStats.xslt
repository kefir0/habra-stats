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
        <style type="text/css">
          .score
          {
            text-align: right;
            color: green;
            display:inline;
            float:right;
          }
        </style>
      </head>

      <body>
        <xsl:for-each select="//Comment">
          <div>
            <!-- http://habrahabr.ru/i/avatars/stub-user-small.gif -->
            <!-- Avatar, username, date, link, postlink,           rating -->
            <img>
              <xsl:attribute name="src">
                <xsl:value-of select="Avatar"/>
              </xsl:attribute>
            </img>
            &#160;
            <xsl:value-of select="UserName" />
            &#160;&#160;
            <a>
              <xsl:attribute name="href">
                <xsl:value-of select="Url"/>
              </xsl:attribute>
              #
            </a>
            &#160;&#160;
            <a>
              <xsl:attribute name="href">
                <xsl:value-of select="PostUrl"/>
              </xsl:attribute>
              <xsl:value-of select="PostTitle"/>
            </a>
            <div class="score">
              <xsl:value-of select="Score" />
            </div>
            <br />
            <xsl:value-of select="Text" disable-output-escaping="yes" />
          </div>
          <p/>
        </xsl:for-each>
        <p/>

        <div style="color:gray">
          Generated: <xsl:value-of select="my:Now()"/><br />
          Created by kefir. naxah1 at gmail. Source: <a href="http://code.google.com/p/habra-stats/source/browse/">code.google.com/p/habra-stats</a>
        </div>
      </body>
      
    </html>
  </xsl:template>
</xsl:stylesheet>