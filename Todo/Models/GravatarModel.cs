namespace Todo.Models
{
    public class GravatarModel
    {
        public Entry[] entry { get; set; }
    }
    public class Entry
    {
        public string displayName { get; set; }
        public string thumbnailUrl { get; set; }
    }
}
