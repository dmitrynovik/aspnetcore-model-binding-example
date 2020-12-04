namespace SampleWebApi
{
    public class Device : ILocation
    {
        public string Airport { get; set; }
        public string Terminal { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
    }
}
