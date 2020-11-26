using Microsoft.AspNetCore.Mvc;

namespace SampleWebApi.Models
{
    // Available in ASP.NET Core 2.1 
    [BindProperties(SupportsGet = true)]
    public class Location : ILocation
    {
        public string Airport { get; set; }
        public string Terminal { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
    }
}
