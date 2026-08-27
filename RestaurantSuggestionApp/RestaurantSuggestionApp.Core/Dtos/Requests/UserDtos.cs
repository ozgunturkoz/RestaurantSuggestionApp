using System;

namespace RestaurantSuggestionApp.Core.DTOs.Requests
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; }
    }

    public class VerifyLoginDto
    {
        public string Email { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }

    public class VerifyRegisterDto
    {
        public string Email { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public bool RequiresVerification { get; set; }

        public string Email { get; set; } = string.Empty;

        public string? Token { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class CreateExpenditureDto
    {
        public int? RestaurantId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? SpentAt { get; set; }
    }

    public class UserExpenditureResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? RestaurantId { get; set; }

        public string? RestaurantName { get; set; }

        public string? RestaurantCategory { get; set; }

        public decimal Amount { get; set; }

        public DateTime SpentAt { get; set; }
    }

    public class UpdateProfileDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ChangePasswordWithCodeDto
    {
        public string Code { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }

    public class SendEmailChangeCodeDto
    {
        public string NewEmail { get; set; } = string.Empty;
    }

    public class ChangeEmailWithCodeDto
    {
        public string NewEmail { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string CurrentPassword { get; set; } = string.Empty;
    }
}
