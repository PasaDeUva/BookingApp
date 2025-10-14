using BotWhatsapp.Application.Features.User.Register;
using BotWhatsapp.Application.Features.User.UpdateUser;
using BotWhatsapp.Application.Features.User.UserList;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Interfaces;

public interface IUserService
{
    Task<ResponseModel<PaginationResponse<UserDto>>> GetAllAsync(UserListRequest request);
    Task<ResponseModel<bool>> CreateAsync(RegisterRequest request);
    Task<ResponseModel<bool>> UpdateAsync(UpdateUserRequest request);
    Task<ResponseModel<bool>> DeleteAsync(int id);
    Task<ResponseModel<int>> GetShopFromInstanceAsync(string instance);
}
