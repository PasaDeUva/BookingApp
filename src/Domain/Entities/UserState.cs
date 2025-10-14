using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Domain.Entities;

public class UserState
{
    public State State { get; set; } = State.None;
    public string? Name { get; set; }
    public int? UserId { get; set; }
}
