using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace API.Data;

public class Seed
{
    public static async Task seedUsers(AppDbContext context)
    {
        if(await context.Users.AnyAsync()) return;
        
        var membersData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(membersData);

        if (members == null)
        {
            Console.WriteLine("no members found");
            return;
        }

       
        foreach (var member in members)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = member.Id,
                DisplayName = member.DisplayName,
                Email = member.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DateOfBirth = member.DateOfBirth,
                    ImageUrl = member.ImageUrl,
                    DisplayName = member.DisplayName,
                    Created = member.Created,
                    LastActive = member.LastActive,
                    Gender = member.Gender,
                    Description = member.Description,
                    City = member.City,
                    Country = member.Country
                }

            };
            user.Member.Photos.Add(new Photo
            {
              Url  = member.ImageUrl!,
              MemberId = member.Id
            });
            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }
}