using System.Net;
using xTend.Service;

namespace xTend {
    public static class Recaptcha {
        const string secret = "6LfvJyMqAAAAANzPPaUnbRX-loUH_0KGmvfnOZq9";
        public class ResponseToken {
            public DateTime challenge_ts { get; set; }
            public float score { get; set; }
            public List<string> ErrorCodes { get; set; }
            public bool Success { get; set; }
            public string hostname { get; set; }
        }

        public static bool Check(string recaptcha) {
            Log.W ("recaptcha: " + recaptcha);
            if (String.IsNullOrEmpty (recaptcha)) return false;
            try {
                string link = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response=" + recaptcha;
                Log.W (link);
                var webRequest = (HttpWebRequest)WebRequest.Create (link);
                using (var response = webRequest.GetResponse ())
                using (var content = response.GetResponseStream ())
                using (var reader = new System.IO.StreamReader (content)) {
                    string json = reader.ReadToEnd ();
                    ResponseToken token = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseToken> (json);
                    if (token != null) {
                        Log.W (json);
                        return !(token.Success == false || token.score < 0.5);
                    }
                }
            }
            catch (Exception ex) {
                Log.W (ex.ToString ());
            }
            return false;
        }
    }
}
