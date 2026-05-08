using APPLICATION.Use_cases.Login_case;
using APPLICATION.Use_cases.SignUp_case;
using APPLICATION.Use_cases.Users_case;
using APPLICATION.Utils.JWT;
using DOMAIN.Entities;
using DOMAIN.Interfaces;

namespace APPLICATION.Handlers
{
    public class UserHandler(IUserRepository service, JwtService jwt)
    {
        public async Task<Response<LoginResponse>> LoginHandler(LoginRequestDTO request)
        {
            try
            {
                var user = await service.VerifyUser(request.Email, request.Password);
                if (user is null)
                {
                    return new Response<LoginResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "Inicio de sesión inválido, usuario y/o contraseña incorrectos"
                    };
                }

                var logged_in = new LoginResponse()
                {
                    access_token = jwt.GenerateToken(request.Email),
                    token_type = "Bearer",
                    expires_in = 3600,
                    email = user.Email,
                    linked_companies = user.Linked_companies?.Select(uc => new CompanyOption
                    {
                        company_id = uc.Company.Company_id,
                        company_name = uc.Company.Company_name
                    }).ToList(),
                    linked_processes = user.Linked_processes?.Select(up => new SmartFlowOption
                    {
                        smartFlow_id = up.SmartFlow_id,
                        smartflow_name = up.SmartFlow_name,
                        first_step_id = up.First_step_id,
                        first_step_name = up.First_step_name,
                        next_step_id = up.Next_step_id,
                        next_step_name = up.Next_step_name,
                        approver = up.Approver
                    }).ToList()
                };

                return new Response<LoginResponse>()
                {
                    Data = logged_in,
                    Success = true,
                    Message = "Inicio de sesión exitoso"
                };

                return null;
            }
            catch (Exception ex)
            {
                return new Response<LoginResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Hubo un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<SignUpResponse>> SignUpHandler(SignUpRequest request)
        {
            try
            {
                var new_user = new User()
                {
                    Email = request.Email,
                    Name = request.Name,
                    First_lastname = request.First_lastname,
                    Second_lastname = request.Second_lastname,
                    Phone = request.Phone,
                    Password = request.Password,
                };
                var created = await service.RegisterUser(new_user);
                if (created is null)
                {
                    return new Response<SignUpResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se ha podido registrar al usuario"
                    };
                }

                var success = new SignUpResponse()
                {
                    Email = created.Email,
                    Name = created.Name,
                    First_lastname = created.First_lastname,
                    Second_lastname = created.Second_lastname,
                    Phone = created.Phone,
                    Created_at = created.Created_at,
                    IsCreated = true
                };

                return new Response<SignUpResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Registro de usuario exitoso"
                };
            }
            catch (Exception ex)
            {
                return new Response<SignUpResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Hubo un error al procesar la solicitud: " + ex.InnerException?.Message
                };
            }
        }

        public async Task<Response<List<UserResponse>>> GetUsersByName(string query)
        {
            try
            {
                var clients = await service.SearchUsersByName(query);
                if (clients.Count is 0)
                {
                    return new Response<List<UserResponse>>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se han encontrado coincidencias en el sistema"
                    };
                }

                var success = clients.Select(c => new UserResponse()
                {
                    Email = c.Email,
                    Name = c.Name,
                    First_lastname = c.First_lastname,
                    Second_lastname = c.Second_lastname,
                    Phone = c.Phone
                }).ToList();

                return new Response<List<UserResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se encontraron coincidencias en el sistema"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<UserResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = $"Hubo un error al procesar la solicitud: {ex.Message}"
                };
            }
        }
    }
}
