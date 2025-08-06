namespace ExternalDestinationSearch.API.Models;

public class DestinationDto(string name)
{
    public string Name { get; } = name;
    public string? Description { get; init; }
    public string? ImageRootUri { get; set; }
    public string? ImageName { get; init; }
    public Uri? ImageUri => ImageName == null || ImageRootUri == null ? null : new Uri(ImageRootUri + ImageName);
}
