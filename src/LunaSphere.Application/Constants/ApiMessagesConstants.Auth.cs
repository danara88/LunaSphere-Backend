namespace LunaSphere.Application.Constants;

public static partial class ApiMessagesConstants
{
    public static class Auth
    {
        public const string RegisterFail = "User registration failed. Please try again later."; 
        public const string VerificationCodeSendSuccess = "The verification code has been sent successfully.";
        public const string SendVerificationCodeFail = "We couldn't send the verification code to your email. Please try again later.";
        public const string GenerateVerificationCodeSuccess = "The verification code has been generated successfully.";
        public const string GenerateVerificationCodeFail = "We couldn't generate the verification code. Please try again later.";
        public const string UserAccountEligibleForVerification = "User account is eligible for verification.";
        public const string VerifyVerificationCodeFail = "Something went wrong. We couldn't verify your verification code. Please try again later.";
    }
}