namespace LunaSphere.Application.Auth.DTOs;

public record VerifyVerificationCodeDTO
(
    UserEligibleForVerificationDTO userEligibleForVerification,
    short VerificationCode
);