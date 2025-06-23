namespace LeawareTest.Domain;

public class Attachment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
}
