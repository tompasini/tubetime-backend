namespace tubetime.Models
{
  public class Comment
  {
    public string Body { get; set; }
    public int Id { get; set; }
    public string CreatorId { get; set; }
    public int BlogId { get; set; }
    public Profile Creator { get; set; }
  }
}