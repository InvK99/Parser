namespace Parser
{
    public class Branch
    {
        public string Title { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Branch[] Children { get; set; }

        public Branch()
        {
            Title = string.Empty;
            Children = new Branch[0];
        }

        public Branch(string title)
        {
            Title = title;
            Children = new Branch[0];
        }

        public string Print(int depth)
        {
            string result = "".PadLeft(depth * 4) + Title + "\r\n";

            if (Children != null)
            {
                int carret = depth + 1;
                foreach (Branch child in Children)
                {
                    if (child != null)
                        result += child.Print(carret);
                }
            }

            if (Title.StartsWith("<") && Title.EndsWith(">"))
                result += "".PadLeft(depth * 4) + Title.Replace("<", "</") + "\r\n";

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public override string ToString()
        {
            string result = Title + " --> ";

            if (Children != null)
            {
                foreach (Branch child in Children)
                {
                    if (child != null)
                        result += child.Title;
                }
            }

            return result;
        }
    }
}
