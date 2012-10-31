Content Compressor Module
==========

This is simple drop-and-forget, configuration-less (optional) http module that will handle compression of javascript and css, both in files and embedded on html page.
Compressor requires a proper type set on embedded scripts - that is type="text/javascript" for javascript, and  type="text/css" for css.
It uses Yahoo's Yui.Compressor to process the content.


Usage:
------

Drop dll into bin directory. Done.

You can disable individual compression modes by adding following entries to config file's appSettings:
  `<add key="ContentCompressorModule.CompressJs" value="false"/>`             to disable compression of js files
  `<add key="ContentCompressorModule.CompressCss" value="false"/>`            to disable compression of css files
  `<add key="ContentCompressorModule.CompressEmbeddedCss" value="false"/>`    to disable compression of embedded css (that is, content of <style type="text/css"></style>)
  `<add key="ContentCompressorModule.CompressEmbeddedJs" value="false"/>`     to disable compression of embedded js (content of <script type="text/javascript"></script>)
Otherwise you don't need to touch config file - module will auto-register itself.