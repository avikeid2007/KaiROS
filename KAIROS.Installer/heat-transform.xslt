<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:wix="http://schemas.microsoft.com/wix/2006/wi">

  <xsl:output method="xml" indent="yes"/>

  <!-- Identity template - copy everything by default -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

  <!-- Exclude Upload directory (contains MSIX we don't need) -->
  <xsl:template match="wix:Directory[@Name='Upload']"/>
  
  <!-- Exclude any ComponentRef that references excluded components -->
  <xsl:template match="wix:ComponentRef[@Id='resources.pri']"/>
  <xsl:template match="wix:ComponentRef[contains(@Id, 'KAIROS_') and contains(@Id, '.msix')]"/>
  <xsl:template match="wix:ComponentRef[contains(@Id, 'KAIROS_') and contains(@Id, '.appxsym')]"/>
  
  <!-- Exclude PDB files in Release builds (optional) -->
  <!-- <xsl:template match="wix:Component[contains(wix:File/@Source, '.pdb')]"/> -->
  
</xsl:stylesheet>

