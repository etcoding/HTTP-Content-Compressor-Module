using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace ET.ContentCompressorModule.CompressorStreams
{
    public abstract class CompressorStreamBase : Stream
    {
        protected Stream inputStream;

        /// <summary>
        /// This function will perform the actual minification.
        /// The argument to a function will be a content of a response stream, converted to string.
        /// </summary>
        protected Func<string, string> MinifyContent = null;

        /// <summary>
        /// This dictionary will contain the minified content. The YUI minifier takes almost a second to minify jquery file, so without caching it'll take too long.
        /// Key will be a string hash.
        /// </summary>
        protected static Dictionary<string, string> minifiedContent = new Dictionary<string, string>();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            this.inputStream.Flush();
        }

        public override long Length
        {
            get { return this.inputStream.Length; }
        }

        public override long Position
        {
            get
            {
                return this.inputStream.Position;
            }
            set
            {
                this.inputStream.Position = value;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CompressorStreamBase"/> class.
        /// </summary>
        /// <param name="inputStream">The input stream of content that will be manipulated.</param>
        /// <param name="minifyAction">The minify method that will be used to compress the content. The argument to the function is a response content, converted to string.</param>
        public CompressorStreamBase(Stream inputStream, Func<string, string> minifyAction)
        {
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");
            if (minifyAction == null)
                throw new ArgumentNullException("minufyAction");

            this.inputStream = inputStream;
            this.MinifyContent = minifyAction;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.inputStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.inputStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.inputStream.SetLength(value);
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                string content = System.Text.UTF8Encoding.UTF8.GetString(buffer, offset, count);

                string hash = ComputeSHA256Hash(content);
                if (!minifiedContent.ContainsKey(hash))
                {
                    lock (minifiedContent)
                    {
                        if (!minifiedContent.ContainsKey(hash))
                        {
                            string compressedJs = this.MinifyContent(content);
                            minifiedContent.Add(hash, compressedJs);
                        }
                    }
                }
                byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(minifiedContent[hash]);
                inputStream.Write(data, 0, data.Length);
            }
            catch
            {
                inputStream.Write(buffer, offset, count);
            }
        }

        private string ComputeSHA256Hash(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            byte[] inputBytes = System.Text.UTF8Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = new SHA256CryptoServiceProvider().ComputeHash(inputBytes);
            string output = System.Text.UTF8Encoding.UTF8.GetString(hashBytes);
            return output;
        }
    }
}