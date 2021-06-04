using System.Collections.Generic;

namespace Web.Api.Settings
{
   
    public class CorsSettings
    {
        public static string ConfigFileSectionName => "Cors";
        public static string PolocyName => "CorsWhiteList";
        public List<string> WhiteList { get; set; }
    }
}
