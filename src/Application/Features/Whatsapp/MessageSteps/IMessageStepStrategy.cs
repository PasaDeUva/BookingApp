using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public interface IMessageStepStrategy
{
    Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client);
}
