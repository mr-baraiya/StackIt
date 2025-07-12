using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class QuestionTag
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? TagId { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Question? Question { get; set; }

    [JsonIgnore]
    public virtual Tag? Tag { get; set; }
}
