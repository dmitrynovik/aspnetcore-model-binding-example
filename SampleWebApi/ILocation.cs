namespace SampleWebApi
{
    public interface ILocation
    {
        string Airport { get; set; }
        string Terminal { get; set; }
        string Type { get; set; }
        string Id { get; set; }
    }
}