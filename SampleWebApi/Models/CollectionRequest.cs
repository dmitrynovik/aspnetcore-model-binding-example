namespace SampleWebApi.Models
{
    public abstract class CollectionRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
}