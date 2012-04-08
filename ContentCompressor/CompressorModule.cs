using System;
using System.Configuration;
using System.Web;
using ET.ContentCompressorModule.CompressorStreams;

namespace ET.ContentCompressorModule
{
    public class CompressorModule : IHttpModule
    {
        protected HttpApplication context;

        /// <summary>
        /// Gets the inline js compression congiguration key.
        /// This is the key that will be checked for in config file and Session. To disable inline compression set it to "false" in either Session, or in AppSettings section of config file.
        /// </summary>
        /// <value>The inline js compression congiguration key.</value>
        public static string EmbeddedJsCompressionCongigurationKey { get { return "ContentCompressorModule.CompressEmbeddedJs"; } }
        public static string EmbeddedCssCompressionCongigurationKey { get { return "ContentCompressorModule.CompressEmbeddedCss"; } }
        public static string JsCompressionCongigurationKey { get { return "ContentCompressorModule.CompressJs"; } }
        public static string CssCompressionCongigurationKey { get { return "ContentCompressorModule.CompressCss"; } }

        /// <summary>
        /// Registers the module with http application.
        /// This method is called by PreApplicationStartMethod attribute in AssemblyInfo.
        /// </summary>
        public static void RegisterSelf()
        {
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(CompressorModule));
        }

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            this.context = context;
            context.PostReleaseRequestState += new EventHandler(context_PostReleaseRequestState);
        }

        #endregion

        /// <summary>
        /// Hook up the compressors.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_PostReleaseRequestState(object sender, EventArgs e)
        {
            // Hook inline js and css compression if this is html, and inline compression is not disabled in config file
            if (this.context.Response.ContentType.ToLower().Contains("text/html"))
            {
                if ((string.IsNullOrEmpty(ConfigurationManager.AppSettings[EmbeddedJsCompressionCongigurationKey]) ||
                    ConfigurationManager.AppSettings[EmbeddedJsCompressionCongigurationKey].ToLower() != "false"))
                    this.context.Response.Filter = new EmbeddedJsCompressorStream(this.context.Response.Filter);

                if ((string.IsNullOrEmpty(ConfigurationManager.AppSettings[EmbeddedCssCompressionCongigurationKey]) ||
                    ConfigurationManager.AppSettings[EmbeddedCssCompressionCongigurationKey].ToLower() != "false"))
                    this.context.Response.Filter = new EmbeddedCssCompressorStream(this.context.Response.Filter);
            }

            // is this a request for javascript file ?
            if (this.context.Response.ContentType.ToLower().Contains("javascript") &&
                (string.IsNullOrEmpty(ConfigurationManager.AppSettings[JsCompressionCongigurationKey]) ||
                    ConfigurationManager.AppSettings[JsCompressionCongigurationKey].ToLower() != "false"))
                this.context.Response.Filter = new JsCompressorStream(this.context.Response.Filter);

            if (this.context.Response.ContentType.ToLower().Contains("text/css") &&
                (string.IsNullOrEmpty(ConfigurationManager.AppSettings[CssCompressionCongigurationKey]) ||
                    ConfigurationManager.AppSettings[CssCompressionCongigurationKey].ToLower() != "false"))
                this.context.Response.Filter = new CssCompressorStream(this.context.Response.Filter);
        }
    }
}