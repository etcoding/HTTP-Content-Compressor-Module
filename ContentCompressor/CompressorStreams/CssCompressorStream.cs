using System.IO;

namespace ET.ContentCompressorModule.CompressorStreams
{
    /// <summary>
    /// Compresses css files.
    /// </summary>
    public class CssCompressorStream : CompressorStreamBase
    {
        public CssCompressorStream(Stream input) : base(input, CompressCss) { }

        private static string CompressCss(string css)
        {
            return Yahoo.Yui.Compressor.CssCompressor.Compress(css);
        }
    }
}