using System.Text.RegularExpressions;
using pc2;

var html = await Load("https://chani-k.co.il/sherlok-game");
var tree = Serialize();
var t=Selector.ConvertToSelector("div#board .img.closed");

HashSet<HtmlElement> root = new HashSet<HtmlElement>();
root.Add(tree);
var l = tree.FindElementBySelector(t, root);
if (l.Count == 0)
    Console.WriteLine("there is no such as element!");
else
    Console.WriteLine("I found!!!!!!!!!!!!!!!!!!!!");
Console.WriteLine();
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
HtmlElement Serialize()
{
    html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
    string[] htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0).ToArray();
    HtmlElement root = new HtmlElement();
    HtmlElement current = root;
    foreach (string line in htmlLines)
    {
        string[] arr = line.Split(" ");
        if (!arr[0].Equals("/html"))//הגענו לסוף הדף
        {
            if (arr[0].StartsWith("/"))//סימן שמדובר בסגירת תגית 
                current = current.Parent;
            else if (HtmlHelper.Instance.HtmlVoidTags.Contains(arr[0]) || HtmlHelper.Instance.HtmlTags.Contains(arr[0]))
            {//מדובר בפתיחת תגית
                HtmlElement newElement = new HtmlElement();
                newElement.Name = arr[0];
                newElement.Parent = current;
                current.Children.Add(newElement);
                current = newElement;
                if (line.IndexOf(" ") > 0)//אין לי attributes
                {
                    var currentAttributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line.Substring(line.IndexOf(" ")));
                    foreach (var attribute in currentAttributes)
                    {
                        string[] a = attribute.ToString().Split("=");
                        if (a[0].Equals("id"))
                            current.Id = a[1];
                        else if (a[0].Equals("class"))
                            current.Classes = a[1].Split(" ").ToList();
                        else
                            current.Attributes.Add(a[0], a[1]);
                    }
                }
                if (HtmlHelper.Instance.HtmlVoidTags.Contains(arr[0]))
                    current = current.Parent;

            }
            else  //מדובר בתוכן התגית
                current.InnerHtml = line;

        }
    }
    return root;
}

