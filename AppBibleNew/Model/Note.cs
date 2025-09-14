namespace AppBibleNew.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = ""; // HTML từ trình soạn thảo
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
