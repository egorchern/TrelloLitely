using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzyClassroomz.Library.Validators;

namespace EzyClassroomz.Testing.Unit
{
    [TestFixture]
    public class PasswordComplexityValidatorTests
    {
        private readonly PasswordComplexityRequirements _defaultRequirements = new PasswordComplexityRequirements
        {
            MinimumLength = 6,
            MaximumLength = 20,
            RequireUppercase = true,
            RequireLowercase = true,
            RequireDigit = true,
            RequireSpecialCharacter = true
        };

        [Test]
        public void Test_ValidPasswords()
        {
            var validPasswords = new[]
            {
                "lej8o#8E$0Xq",
                "A1b2C3d4!@#$",
                "StrongPass123!",
                "P@ssw0rd2024",
                "Complex#Pass9"
            };

            foreach (var password in validPasswords)
            {
                var result = PasswordComplexityValidator.Validate(password, _defaultRequirements);
                Assert.That(result == PasswordComplexityResult.Valid);
            }
        }

        [Test]
        public void Test_InvalidPasswords()
        {
            var testCases = new Dictionary<string, PasswordComplexityResult>
            {
                { "short", PasswordComplexityResult.TooShort },
                { "thispasswordiswaytoolongtobevalisdfffffffffffd", PasswordComplexityResult.TooLong },
                { "NoSpecialChar1", PasswordComplexityResult.MissingSpecialCharacter },
                { "missingupper1$", PasswordComplexityResult.MissingUppercase },
                { "NOSPECIALCHAR1!", PasswordComplexityResult.MissingLowercase },
                { "NoDigit!@", PasswordComplexityResult.MissingDigit },
                { "InvalidChar^1A", PasswordComplexityResult.SpecialCharacterNotAllowed },
                { "InvalidCharé1A!", PasswordComplexityResult.AlphabeticCharacterNotAllowed }
            };

            foreach (var testCase in testCases)
            {
                var result = PasswordComplexityValidator.Validate(testCase.Key, _defaultRequirements);
                Assert.That(result == testCase.Value);
            }
        }

        [Test]
        public void Test_CustomAllowedCharacters()
        {
            // Arrange
            var requirements = _defaultRequirements with
            {
                AllowedAlphabeticCharacters = new HashSet<char>("abAB".ToCharArray()),
                AllowedSpecialCharacters = new HashSet<char>("$".ToCharArray())
            };

            var validPassword = "Ababababab1$";
            var invalidCharPassword = validPassword + "c";
            var invalidSpecialCharPassword = validPassword + "!";

            // Act
            var validResult = PasswordComplexityValidator.Validate(validPassword, requirements);
            var invalidCharResult = PasswordComplexityValidator.Validate(invalidCharPassword, requirements);
            var invalidSpecialCharResult = PasswordComplexityValidator.Validate(invalidSpecialCharPassword, requirements);

            // Assert
            Assert.That(validResult == PasswordComplexityResult.Valid);
            Assert.That(invalidCharResult == PasswordComplexityResult.AlphabeticCharacterNotAllowed);
            Assert.That(invalidSpecialCharResult == PasswordComplexityResult.SpecialCharacterNotAllowed);
        }

        [Test]
        public void Test_BoundaryLengths()
        {
            // Arrange
            var atMinPassword = new string('a', _defaultRequirements.MinimumLength - 3) + "A1$";
            var justBelowMinPassword = new string('a', _defaultRequirements.MinimumLength - 4) + "A1$";
            var atMaxPassword = new string('a', _defaultRequirements.MaximumLength - 3) + "A1$";
            var justAboveMaxPassword = new string('a', _defaultRequirements.MaximumLength - 2) + "A1$";

            // Act
            var atMinResult = PasswordComplexityValidator.Validate(atMinPassword, _defaultRequirements);
            var justBelowMinResult = PasswordComplexityValidator.Validate(justBelowMinPassword, _defaultRequirements);
            var atMaxResult = PasswordComplexityValidator.Validate(atMaxPassword, _defaultRequirements);
            var justAboveMaxResult = PasswordComplexityValidator.Validate(justAboveMaxPassword, _defaultRequirements);

            // Assert
            Assert.That(atMinResult == PasswordComplexityResult.Valid);
            Assert.That(justBelowMinResult == PasswordComplexityResult.TooShort);
            Assert.That(atMaxResult == PasswordComplexityResult.Valid);
            Assert.That(justAboveMaxResult == PasswordComplexityResult.TooLong);
        }

        [Test]
        public void Test_NullPassword()
        {
            // Arrange
            string? nullPassword = null;

            // Act
            var result = PasswordComplexityValidator.Validate(nullPassword!, _defaultRequirements);

            // Assert
            Assert.That(result == PasswordComplexityResult.Null);
        }

        [Test]
        public void Test_TurningOffRequirements()
        {
            // Arrange
            var relaxedRequirements = new PasswordComplexityRequirements
            {
                MinimumLength = 0,
                MaximumLength = 128,
                RequireUppercase = false,
                RequireLowercase = false,
                RequireDigit = false,
                RequireSpecialCharacter = false
            };

            var simplePassword = "a";

            // Act
            var result = PasswordComplexityValidator.Validate(simplePassword, relaxedRequirements);

            // Assert
            Assert.That(result == PasswordComplexityResult.Valid);
        }
    }
}
