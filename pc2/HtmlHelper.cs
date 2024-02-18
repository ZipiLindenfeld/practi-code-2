using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace pc2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            var htmlTags = File.ReadAllText("TagsList/HtmlTags.json");
            var htmlVoidTags = File.ReadAllText("TagsList/HtmlVoidTags.json");

            this.HtmlTags = JsonSerializer.Deserialize<string[]>(htmlTags);
            this.HtmlVoidTags = JsonSerializer.Deserialize<string[]>(htmlVoidTags);
        }
    }
}
