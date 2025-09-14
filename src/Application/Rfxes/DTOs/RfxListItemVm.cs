namespace Application.Rfxes.DTOs;

public sealed class RfxListItemVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Type { get; set; } = default!;            // "RFP" | "RFQ" | "RFI" | "EOI"
    public string Category { get; set; } = default!;
    public DateTime PublicationDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string Status { get; set; } = default!;          // "Draft" | "Published" | ...
    public int Responses { get; set; }                      // TODO: replace with real count
}
