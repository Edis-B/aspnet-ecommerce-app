public class SearchCategoryFormModel
{
    public string CategoryId { get; set; }
    public string Query { get; set; }
    public int CurrentPage { get; set; } = 1; // Default to 1 if not provided
}