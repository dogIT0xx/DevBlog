namespace DevBlog.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public string PreviewContent { get; set; }
        public string Category { get; set; }
    }
}
