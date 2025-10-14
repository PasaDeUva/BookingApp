using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.Update
{
    public class UpdateResourceRequest : IRequest<ResponseModel<bool>>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public List<int> ServiceIds { get; set; } = new List<int>();
    }
}
