using ET.ContentCompressorModule.CompressorStreams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;
using System.IO;
using System.Diagnostics;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for CssCompressorStreamTest and is intended
    ///to contain all CssCompressorStreamTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CompressorStreamsTests
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CompressCss
        ///</summary>
        [TestMethod()]
        public void CompressCssTest()
        {
            string input = @"body {color:red;}
                            h1 {color:#00ff00;}
                            p.ex {color:rgb(0,0,255);}";
            string output = CompressCss(input);

            output.Length.Should().BeGreaterThan(0);
            output.Length.Should().BeLessThan(input.Length);
        }


        /// <summary>
        /// The compressed content should be cached, so 2nd call with same input should be very fast.
        /// This is done in a base class, so only need to test it with any of the children classes.
        /// </summary>
        [TestMethod()]
        public void CompressCssCachingTest()
        {
            string input = @"body {color:red;}
                            h1 {color:#00ff00;}
                            p.ex {color:rgb(0,0,255);}";
            string output = CompressCss(input); // this will take about a second

            Stopwatch sw = new Stopwatch();
            sw.Start();
            output = CompressCss(input);
            sw.Stop();
            sw.ElapsedMilliseconds.Should().BeLessThan(10);
        }


        private static string CompressCss(string input)
        {
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(input);
            string output = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CssCompressorStream cssc = new CssCompressorStream(ms))
                {
                    cssc.Write(bytes, 0, bytes.Length);
                    output = StreamToString(ms);
                }
            }
            return output;
        }

        [TestMethod()]
        public void CompressInlineCssTest()
        {
            string input = @"<html>
                                <head>
                                <style type='text/css'>
                                body {color:red;}
                                h1 {color:#00ff00;}
                                p.ex {color:rgb(0,0,255);}
                                </style>
                                </head>

                                <body>
                                <h1>This is heading 1</h1>
                                <p>This is an ordinary paragraph. Notice that this text is red. The default text-color for a page is defined in the body selector.</p>
                                </body>
                                </html>
                                ";
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(input);
            string output = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (EmbeddedCssCompressorStream cssc = new EmbeddedCssCompressorStream(ms))
                {
                    cssc.Write(bytes, 0, bytes.Length);
                    output = StreamToString(ms);
                }
            }

            output.Length.Should().BeGreaterThan(0);
            output.Length.Should().BeLessThan(input.Length);
        }

        [TestMethod()]
        public void CompressJSTest()
        {
            string input = @"function myfunction(txt)
                            {
                            var longname = 'asdfasdfasf';
                            alert(txt + longname);
                            }";
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(input);
            string output = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (JsCompressorStream jsc = new JsCompressorStream(ms))
                {
                    jsc.Write(bytes, 0, bytes.Length);
                    output = StreamToString(ms);
                }
            }

            output.Length.Should().BeGreaterThan(0);
            output.Length.Should().BeLessThan(input.Length);
        }

        [TestMethod()]
        public void CompressInlineJSTest()
        {
            string input = @"<html>
                                <body>

                                <script type='text/javascript'>
                                /*
                                The code below will write
                                one heading and two paragraphs
                                */
                                document.write('<h1>This is a heading</h1>');
                                document.write('<p>This is a paragraph.</p>');
                                document.write('<p>This is another paragraph.</p>');
                                </script>

                                </body>
                                </html>";
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(input);
            string output = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (EmbeddedJsCompressorStream jsc = new EmbeddedJsCompressorStream(ms))
                {
                    jsc.Write(bytes, 0, bytes.Length);
                    output = StreamToString(ms);
                }
            }

            output.Length.Should().BeGreaterThan(0);
            output.Length.Should().BeLessThan(input.Length);
        }


        private static string StreamToString(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }
    }
}
