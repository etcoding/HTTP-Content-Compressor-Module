using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ET.ContentCompressorModule.CompressorStreams
{
    /// <summary>
    /// Compresses embedded (in html page) css.
    /// Style tag must have a proper type (text/css) set.
    /// </summary>
    public class EmbeddedCssCompressorStream : CompressorStreamBase
    {
        public EmbeddedCssCompressorStream(Stream input) : base(input, CompressEmbeddedCss) { }

        private static string CompressEmbeddedCss(string html)
        {
            // find css in html
            MatchEvaluator meCompressor = (m) =>
            {
                if (m.Groups["css"] != null && !string.IsNullOrEmpty(m.Groups["css"].Value))
                    return m.Value.Replace(m.Groups["css"].Value, Yahoo.Yui.Compressor.CssCompressor.Compress(m.Groups["css"].Value));
                return m.Value;
            };

            string compressedCss = Regex.Replace(html, @"<style.*?text/css.*?>(?<css>.*?)</style>", meCompressor, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return compressedCss;
        }
    }
}