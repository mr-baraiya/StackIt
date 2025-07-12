using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class Vote
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? VotableType { get; set; }

    public int? VotableId { get; set; }

    public string? VoteType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
