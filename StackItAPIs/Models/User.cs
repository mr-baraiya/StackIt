using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StackItAPIs.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string? Bio { get; set; }

    public int? Reputation { get; set; }

    public string? Role { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsBanned { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [JsonIgnore]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [JsonIgnore]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [JsonIgnore]
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
