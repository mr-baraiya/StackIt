using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Color { get; set; }

    public int? UsageCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<QuestionTag> QuestionTags { get; set; } = new List<QuestionTag>();
}
