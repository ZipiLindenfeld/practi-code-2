using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pc2
{

    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Attributes = new Dictionary<string, string>();
            Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            HtmlElement current;
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                current = queue.Dequeue();
                yield return current;
                current.Children.ForEach(child => queue.Enqueue(child));
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        public HashSet<HtmlElement> FindElementBySelector(Selector selector, HashSet<HtmlElement> list)
        {
            if (selector == null)
                return list;
            HashSet<HtmlElement> new_list = new HashSet<HtmlElement>();
            if (list != null)
                foreach (var l in list)
                    foreach (var item in l.Descendants())
                        if (checkSelector(item, selector))
                            new_list.Add(item);
            return FindElementBySelector(selector.Child, new_list);
        }
        static bool checkSelector(HtmlElement element, Selector selector)
        {
            if (selector.TagName != element.Name)
                return false;
            char[] cc = new char[] { '\\', '\"' };
            string elementId = null;
            if (element.Id != null)
                elementId = element.Id.Split(cc).First(c => c != "");
            if (elementId != null && !selector.Id.Equals(elementId))
                return false;
            foreach (var c in selector.Classes)
            {
                if (element.Classes != null)
                {
                    for (int i = 0; i < element.Classes.Count; i++)
                    {
                        element.Classes[i] = element.Classes[i].Split(cc).First(g => g != "");
                    }
                    if (!(element.Classes.Contains(c) || element.Classes.Contains(c + "\\\"") || element.Classes.Contains("\"\\" + c)))
                        return false;
                }
            }

            return true;
        }
    }
}

