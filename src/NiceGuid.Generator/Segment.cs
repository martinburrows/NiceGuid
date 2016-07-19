namespace NiceGuid.Generator
{
    public class Segment
    {
        public static Segment Separator = new Segment("-");

        public Segment(string text)
        {
            Text = text;
            Guid = text.Replace("l", "1").Replace("i", "1").Replace("o", "0").Replace("s", "5");
        }

        public string Guid { get; }
        public string Text { get; }
    }
}