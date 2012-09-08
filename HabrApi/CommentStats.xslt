<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:template match="/">
    <html>
      <head>
        <title>GMS Notice</title>
        <style type="text/css">
          .score
          {
            text-align: right;
            color: green;
            margin: 10px;
          }
        </style>
      </head>

      <body>
        <table>
          <xsl:for-each select="//Comment">
            <tr>
              <td>
                <xsl:value-of select="Text" />
              </td>
              <td>
                <div class="score">
                  <xsl:value-of select="Score" />
                </div>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
      
    </html>
  </xsl:template>
</xsl:stylesheet>