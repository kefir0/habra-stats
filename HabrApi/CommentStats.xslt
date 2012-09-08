<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:template match="/">
    <html>
      <head>
        <title>GMS Notice</title>
        <style type="text/css">
        </style>
      </head>

      <body>
        <xsl:for-each select="//Comment">
          <xsl:value-of select="Text" />
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>