using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? CommentableType { get; set; }

    public int? CommentableId { get; set; }

    public string? Content { get; set; }

    public int? VoteScore { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
