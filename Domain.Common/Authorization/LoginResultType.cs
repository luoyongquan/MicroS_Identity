namespace Domain.Authorization
{
    public enum LoginResultType : byte
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,
        
        InvalidPassword,
        
        UserIsNotActive,

      
        UserEmailIsNotConfirmed,
        
        UnknownExternalLogin,

        LockedOut,

        UserPhoneNumberIsNotConfirmed,
    }
}