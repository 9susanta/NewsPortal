using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace NewsPortal.Common
{
    public class apiPlugin
    {
        ILog Log = log4net.LogManager.GetLogger(typeof(apiPlugin));
        public void pagePublish(string message,string postUrl)
        {
            try
            {
                var access_token = ConfigurationManager.AppSettings["fbpageToken"];

                HttpClient client = new HttpClient();

                var values = new Dictionary<string, string> { { "message", message},{ "link", postUrl }, { "access_token", access_token } };

                var content = new FormUrlEncodedContent(values);

                var response =  client.PostAsync("https://graph.facebook.com/111224040351840/feed", content);

                var responseString = response.Result;

                Log.Info(responseString.Headers.WwwAuthenticate);
            }
            catch (Exception ex)
            {

                Log.Error(ex);
            }
        }
    }
}