namespace bombali.infrastructure.app.monitorchecks
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Cache;
    using logging;

    public class WebsiteCheck : ICheck
    {
        double failure_count = 0d;
        IList<HttpStatusCode> okay_responses;

        public WebsiteCheck()
        {
            build_list_of_okay_responses();
        }

        public void build_list_of_okay_responses()
        {
            okay_responses = new List<HttpStatusCode>
                                 {
                                     HttpStatusCode.OK,
                                     HttpStatusCode.Redirect,
                                     HttpStatusCode.Found,
                                     HttpStatusCode.TemporaryRedirect,
                                     HttpStatusCode.Forbidden,
                                     HttpStatusCode.Unauthorized,
                                     HttpStatusCode.ProxyAuthenticationRequired
                                 };
        }

        public string last_response { get; private set; }

        public bool run_check(string what_to_check)
        {
            bool successful_check = true;

            HttpStatusCode response_code = HttpStatusCode.Unused;

            //todo:make the number of loops configurable
            for (int i = 1; i <= 4; i++)
            {
                response_code = get_url_information(what_to_check);
                if (okay_responses.Contains(response_code))
                {
                    break;
                }
            }

            last_response = response_code.ToString();

            if (okay_responses.Contains(response_code))
            {
                failure_count = 0;
                Log.bound_to(this).Info("{0} was able to successfully reach {1}. Response code was {2}.", ApplicationParameters.name,
                                        what_to_check, response_code);
            }
            else
            {
                successful_check = false;
                failure_count += 1;
                Log.bound_to(this).Warn(
                    "{0} warning! {1} is unreachable. Response code was {2}. This has happened {3} times.",
                    ApplicationParameters.name, what_to_check, response_code, failure_count);
            }

            return successful_check;
        }

        public HttpStatusCode get_url_information(string the_url_to_check)
        {
            Log.bound_to(this).Debug("{0} is checking {1}.", ApplicationParameters.name, the_url_to_check);

            WebResponse response = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(the_url_to_check));
            request.KeepAlive = false;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy = WebRequest.GetSystemWebProxy();
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Method = WebRequestMethods.Http.Get;
            request.PreAuthenticate = true;
            request.Timeout = (int)TimeSpan.FromSeconds(15).TotalMilliseconds;

            try
            {
                response = request.GetResponse();
                return ((HttpWebResponse)response).StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.NotFound;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }
}