using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class Question
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? ViewCount { get; set; }

    public int? VoteScore { get; set; }

    public int? AnswerCount { get; set; }

    public int? AcceptedAnswerId { get; set; }

    public bool? IsClosed { get; set; }

    public string? ClosedReason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [JsonIgnore]
    public virtual ICollection<QuestionTag> QuestionTags { get; set; } = new List<QuestionTag>();

    [JsonIgnore]
    public virtual User? User { get; set; }
}
