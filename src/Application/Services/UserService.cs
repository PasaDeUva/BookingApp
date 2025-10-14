using BotWhatsapp.Application.Features.User.Register;
using BotWhatsapp.Application.Features.User.UpdateUser;
using BotWhatsapp.Application.Features.User.UserList;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Response;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace BotWhatsapp.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseModel<PaginationResponse<UserDto>>> GetAllAsync(UserListRequest request)
    {
        try
        {
            var users = await _userRepository.GetAllAsync(request.PageNumber, request.PageSize, request.Id,
                                                          request.Username, request.Email, request.Role,
                                                          request.IsActive);

            return new ResponseModel<PaginationResponse<UserDto>>()
            {
                Success = true,
                Data = new PaginationResponse<UserDto>
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = users.Item2,
                    Data = users.Item1.Select(u => new UserDto
                    {
                        Id = u.Id,
                        UserName = u.Username,
                        Email = u.Email,
                        Role = u.Role,
                        IsActive = u.IsActive
                    }).ToList()
                },
                Description = "Usuarios obtenidos exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<PaginationResponse<UserDto>>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener los usuarios"
            };
        }
    }
    public async Task<ResponseModel<bool>> CreateAsync(RegisterRequest request)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user != null)
            {

                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Description = "El nombre de usuario ya está en uso"
                };
            }

            var userC = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = GeneratePasswordHash(request.Password),
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var res = await _userRepository.CreateAsync(userC);

            return new ResponseModel<bool>
            {
                Success = res > 0,
                Data = res > 0,
                Description = res > 0 ? "Usuario creado exitosamente" : "Error al crear el usuario"
            };


        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al crear el usuario"
            };
        }
    }
    public async Task<ResponseModel<bool>> UpdateAsync(UpdateUserRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                return new ResponseModel<bool>()
                {
                    Success = false,
                    Data = false,
                    Description = "Usuario no encontrado"
                };
            }

            user.Username = request.UserName;
            user.Email = request.Email;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            var res = await _userRepository.UpdateAsync(user);

            return new ResponseModel<bool>
            {
                Success = res > 0,
                Data = res > 0,
                Description = res > 0 ? "Usuario actualizado exitosamente" : "Error al actualizar el usuario"
            };

        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al actualizar el usuario"
            };
        }
    }

    public async Task<ResponseModel<UserDto>> GetByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return new ResponseModel<UserDto>
                {
                    Success = false,
                    Data = null,
                    Description = "Usuario no encontrado"
                };
            }
            return new ResponseModel<UserDto>
            {
                Success = true,
                Data = new UserDto
                {
                    Id = user.Id,
                    UserName = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive
                },
                Description = "Usuario obtenido exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<UserDto>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener el usuario"
            };
        }
    }
    public async Task<ResponseModel<bool>> DeleteAsync(int id)
    {
        try
        {
            var res = await _userRepository.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = res > 0,
                Data = res > 0,
                Description = res > 0 ? "Usuario eliminado exitosamente" : "Error al eliminar el usuario"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al eliminar el usuario"
            };
        }
    }
    public async Task<ResponseModel<int>> GetShopFromInstanceAsync(string instance)
    {
        try
        {
            var shopId = await _userRepository.GetShopFromInstanceAsync(instance);
            return new ResponseModel<int>
            {
                Success = shopId > 0,
                Data = shopId,
                Description = shopId > 0 ? "Tienda encontrada" : "Tienda no encontrada"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<int>
            {
                Success = false,
                Data = 0,
                Description = "Error al obtener la tienda"
            };
        }
    }


    #region PRIVATE METHODS
    private static string GeneratePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    #endregion
}
