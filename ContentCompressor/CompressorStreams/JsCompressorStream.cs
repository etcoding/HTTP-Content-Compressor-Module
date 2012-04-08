using System.IO;

namespace ET.ContentCompressorModule.CompressorStreams
{
    /// <summary>
    /// Compresses JavaScript files.
    /// </summary>
    public class JsCompressorStream : CompressorStreamBase
    {
        public JsCompressorStream(Stream input) : base(input, CompressJs) { }

        private static string CompressJs(string js)
        {
            return Yahoo.Yui.Compressor.JavaScriptCompressor.Compress(js);
        }
    }
}