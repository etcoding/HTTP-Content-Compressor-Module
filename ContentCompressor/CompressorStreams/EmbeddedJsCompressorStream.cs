using System.IO;
using System.Text.RegularExpressions;

namespace ET.ContentCompressorModule.CompressorStreams
{
    /// <summary>
    /// Compresses embedded (to html page) JavaScript.
    /// Script tag must have a proper type (text/javascript) set.
    /// </summary>
    public class EmbeddedJsCompressorStream : CompressorStreamBase
    {
        public EmbeddedJsCompressorStream(Stream input) : base(input, CompressEmbeddedJs) { }
        
        private static string CompressEmbeddedJs(string html)
        {
            MatchEvaluator meCompressor = (m) =>
            {
                if (m.Groups["js"] != null && !string.IsNullOrEmpty(m.Groups["js"].Value))
                    return m.Value.Replace(m.Groups["js"].Value, Yahoo.Yui.Compressor.JavaScriptCompressor.Compress(m.Groups["js"].Value));
                return m.Value;
            };

            string compressedJs = Regex.Replace(html, @"<script.*?text/javascript.*?>(?<js>.*?)</script>", meCompressor, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return compressedJs;
        }
    }
}