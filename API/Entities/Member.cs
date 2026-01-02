using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Entities;

public class Member
{
   public string Id { get; set; } = null!;
   public DateOnly DateOfBirth { get; set; }
   public string?  ImageUrl { get; set; }
   public required string DisplayName { get; set; }
   public DateTime Created  { get; set; }= DateTime.UtcNow;
   public DateTime LastActive { set; get;} = DateTime.UtcNow;
   public required string Gender { get; set; }
   public string? Description { get; set; }
   public required string City { get; set; }
   public required string Country { get; set; }
   
   //navigation proparty
   [JsonIgnore]
   public List<Photo> Photos { get; set; } = [];
   
   [JsonIgnore]
   [ForeignKey(nameof(Id))]
   public AppUser User { get; set; } = null!;
}