using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Domain.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Shop? Shop { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }

}