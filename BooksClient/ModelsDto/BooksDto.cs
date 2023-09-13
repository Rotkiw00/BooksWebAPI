namespace BooksClient.ModelsDto
{
    public class BooksDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime? PublishDate { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\n" +
                   $"Titile: {Title}\n" +
                   $"Author: {Author}\n" +
                   $"Description: {Description}\n" +
                   $"Genre: {Genre}\n" +
                   $"Publish Date: {PublishDate}\n";
        }
    }
}
