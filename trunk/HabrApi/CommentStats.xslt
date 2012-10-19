﻿<?xml version="1.0" encoding="utf-8"?>
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
        <meta http-equiv="Content-Type" content="text/html; charset=windows-1251" />
        <title>
          <xsl:value-of select="Title"/>
        </title>
        <link href="comments.css" rel="stylesheet" media="all" />
      </head>

      <body>
        <!-- Header with selection -->
        <div style="width:80%; margin-left:auto; margin-right:auto;display:block;">
          <div style="dispay:inline-block;">
            <xsl:for-each select="//ArrayOfCommentReportAttribute">
              <div style="display:inline; float:left; margin-right:30px;">
                <form>
                  <xsl:for-each select="CommentReportAttribute">
                    <input type="radio">
                      <xsl:attribute name="name"><xsl:value-of select="Category" /></xsl:attribute>
                      <xsl:attribute name="value"><xsl:value-of select="Value" /></xsl:attribute>
                      <xsl:attribute name="id"><xsl:value-of select="Value" /></xsl:attribute>
                      <xsl:if test="position() = 1">
                        <xsl:attribute name="checked">yes</xsl:attribute>
                      </xsl:if>
                    </input>
                    <label>
                      <xsl:attribute name="for"><xsl:value-of select="Value" /></xsl:attribute>
                      <xsl:value-of select="Name" />
                    </label>
                    <br />
                  </xsl:for-each>
                </form>
              </div>
            </xsl:for-each>
            
            <img src="habrahabr_01.png" style="display:inline; float:right; width:100px; height:75px" />
          </div>
          
        </div>

        <div style="clear:both;" />
        <hr  />

        <!-- Comments -->
        <div class="comments_list" style="width:80%; margin-left:auto; margin-right:auto;">
          <xsl:for-each select="//Comment">
          
            <div class="comment_item">
              <div class="info  ">
                <div class="voting   ">
                  <xsl:if test="Score &gt; 0">
                    <div class="mark positive">
                      <span class="score">
                        <xsl:attribute name="title">Всего <xsl:value-of select="Score"/>: &#8593;<xsl:value-of select="ScorePlus"/> и &#8595;<xsl:value-of select="ScoreMinus"/></xsl:attribute>
                        <xsl:value-of select="Score"/>
                      </span>
                    </div>
                  </xsl:if>
                  <xsl:if test="Score &lt; 0">
                    <div class="mark negative">
                      <span class="score">
                        <xsl:attribute name="title">Всего <xsl:value-of select="Score"/>: &#8593;<xsl:value-of select="ScorePlus"/> и &#8595;<xsl:value-of select="ScoreMinus"/></xsl:attribute>
                        <xsl:value-of select="Score"/>
                      </span>
                    </div>
                  </xsl:if>
                </div>

                <a class="avatar">
                  <xsl:attribute name="href">http://habrahabr.ru/users/<xsl:value-of select="UserName"/>/</xsl:attribute>
                  <img>
                    <xsl:attribute name="src"><xsl:value-of select="Avatar"/></xsl:attribute>
                  </img>
                </a>
                
                <a class="username">
                  <xsl:attribute name="href">http://habrahabr.ru/users/<xsl:value-of select="UserName"/>/</xsl:attribute>
                  <xsl:value-of select="UserName"/>
                </a>

                <span class="comma">,</span>
                <time>
                  <xsl:value-of select="Date"/>
                </time>

                <a class="link_to_comment">
                  <xsl:attribute name="href"><xsl:value-of select="Url"/></xsl:attribute>#
                </a>

                <a class="link_to_comment">
                  <xsl:attribute name="href"><xsl:value-of select="PostUrl"/></xsl:attribute>
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