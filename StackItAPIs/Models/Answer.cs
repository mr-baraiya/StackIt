using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class Answer
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? UserId { get; set; }

    public string? Content { get; set; }

    public int? VoteScore { get; set; }

    public bool? IsAccepted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual Question? Question { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
