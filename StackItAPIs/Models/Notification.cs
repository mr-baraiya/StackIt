using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public string? RelatedType { get; set; }

    public int? RelatedId { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
