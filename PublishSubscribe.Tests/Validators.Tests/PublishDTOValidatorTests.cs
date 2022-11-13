using PublishSubscribe.DTOs;
using PublishSubscribe.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.Tests.Validators.Tests
{
    public class PublishDTOValidatorTests
    {
        #region DTOExemples
        public IEnumerable<PublishDTO> ValidDTOs =>
            new List<PublishDTO>{
                new PublishDTO{
                    Message = "test",
                    Topic="test",
                    Token="test",
                    PublisherId=1
                },
                new PublishDTO{
                    Message = "test",
                    Topic="test",
                    Token="test",
                    PublisherId=3
                }
            };
        public IEnumerable<PublishDTO> InvalidDTOs =>
            new List<PublishDTO>{
                new PublishDTO{
                    Topic="test",
                    Token="test",
                    PublisherId=3
                },
                new PublishDTO{
                    Message = "test",
                    Token="test",
                    PublisherId=3
                },
                new PublishDTO{
                    Message = "test",
                    Topic="test",
                    PublisherId=3
                },
                new PublishDTO{
                    Message = "test",
                    Topic="test",
                    Token="test"
                },
                new PublishDTO{
                    Message = "test",
                    Topic="test",
                    Token="test",
                    PublisherId=-3
                },
                new PublishDTO{
                    Message = "test",
                    Topic="test",
                    Token="test",
                    PublisherId=0
                },
                new PublishDTO{
                    Topic="test"
                },
                new PublishDTO{}
            };

        #endregion

        [Theory]
        [MemberData(nameof(CreateTestData), new object[] { nameof(ValidDTOs), typeof(PublishDTOValidatorTests) })]
        public void Valid_PublishDTO_ReturnsTrue(PublishDTO dTO)
        {
            // Arrange
            var validator = new PublishDTOValidator();

            // Act
            var result = validator.Validate(dTO);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [MemberData(nameof(CreateTestData), new object[] { nameof(InvalidDTOs), typeof(PublishDTOValidatorTests) })]
        public void Invalid_PublishDTO_ReturnsFalse(PublishDTO dTO)
        {
            // Arrange
            var validator = new PublishDTOValidator();

            // Act
            var result = validator.Validate(dTO);

            // Assert
            Assert.False(result.IsValid);
        }

        public static IEnumerable<object[]> CreateTestData(string propertyName, Type type)
        {
            return TestingHelpers.CreateObjectArrayOfObjects(propertyName, type);
        }
    }
}
