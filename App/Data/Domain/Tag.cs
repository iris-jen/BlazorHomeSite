namespace BlazorHomeSite.Data.Domain;

public class Tag : BaseEntity
{
    public Features Feature { get; private set; }
    public string Name { get; private set; }
    public int RelatedEntityId { get; private set; }

    public Tag()
    {
    }

    public Tag(string name, Features feature, int relatedEntityId)
    {
        Name = name;
        Feature = feature;
        RelatedEntityId = relatedEntityId;
    }
}