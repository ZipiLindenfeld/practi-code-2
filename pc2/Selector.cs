using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pc2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public static Selector ConvertToSelector(string str)
        {
            char[] chars = { '#', '.' };
            string[] arr = str.Split(" ");
            Selector root = new Selector();
            Selector current = root;
            string pattern = @"(?<tag>\w+)(?:#(?<id>\w+))?(?:\.(?<class>\w+))*";
            for (int i = 0; i < arr.Length; i++)
            {
                current.Child = new Selector();
                current = current.Child;
                // מציאת ההתאמות באמצעות regex
                Match match = Regex.Match(arr[i], pattern);
                // חלוץ התגית
                string tagName = match.Groups["tag"].Value;
                if (HtmlHelper.Instance.HtmlVoidTags.Contains(tagName) || HtmlHelper.Instance.HtmlTags.Contains(tagName))
                    current.TagName = tagName;
                // חלוץ המזהה
                string id = match.Groups["id"].Value;
                current.Id = id;
                // חלוץ הקלאסים
                string classes = string.Join(" ", match.Groups["class"].Captures.Cast<Capture>().Select(c => c.Value));
                current.Classes = classes.Split(" ").ToList();
            }
            return root.Child;
        }
    }
}
