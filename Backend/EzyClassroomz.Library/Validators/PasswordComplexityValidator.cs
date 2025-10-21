namespace EzyClassroomz.Library.Validators;

public record PasswordComplexityRequirements
{
    private static readonly HashSet<char> _defaultAllowedAlphabeticCharacters = new HashSet<char>("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
    private static readonly HashSet<char> _defaultAllowedSpecialCharacters = new HashSet<char>("!@#$%&()-_=+[]{}|;:,.?".ToCharArray());

    public int MinimumLength { get; set; } = 8;
    public int MaximumLength { get; set; } = 128;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public bool RequireSpecialCharacter { get; set; } = true;
    public HashSet<char> AllowedAlphabeticCharacters { get; set; } = _defaultAllowedAlphabeticCharacters;
    public HashSet<char> AllowedSpecialCharacters { get; set; } = _defaultAllowedSpecialCharacters;
}

public enum PasswordComplexityResult
{
    Valid,
    Null,
    TooShort,
    TooLong,
    AlphabeticCharacterNotAllowed,
    SpecialCharacterNotAllowed,
    MissingUppercase,
    MissingLowercase,
    MissingDigit,
    MissingSpecialCharacter
}

public static class PasswordComplexityValidator
{
    public static PasswordComplexityResult Validate(string password, PasswordComplexityRequirements requirements)
    {
        if (password == null)
            return PasswordComplexityResult.Null;
            
        if (password.Length < requirements.MinimumLength)
            return PasswordComplexityResult.TooShort;

        if (password.Length > requirements.MaximumLength)
            return PasswordComplexityResult.TooLong;

        if (password.Where(ch => char.IsLetter(ch)).Any(ch => !requirements.AllowedAlphabeticCharacters.Contains(ch)))
            return PasswordComplexityResult.AlphabeticCharacterNotAllowed;

        if (password.Where(ch => !char.IsLetterOrDigit(ch)).Any(ch => !requirements.AllowedSpecialCharacters.Contains(ch)))
            return PasswordComplexityResult.SpecialCharacterNotAllowed;

        if (requirements.RequireUppercase && !password.Any(char.IsUpper))
            return PasswordComplexityResult.MissingUppercase;

        if (requirements.RequireLowercase && !password.Any(char.IsLower))
            return PasswordComplexityResult.MissingLowercase;

        if (requirements.RequireDigit && !password.Any(char.IsDigit))
            return PasswordComplexityResult.MissingDigit;

        if (requirements.RequireSpecialCharacter && !password.Any(ch => !char.IsLetterOrDigit(ch)))
            return PasswordComplexityResult.MissingSpecialCharacter;

        return PasswordComplexityResult.Valid;
    }
}