using PublishSubscribe.DTOs;
using PublishSubscribe.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.Tests.Validators.Tests
{
    public class SubscribeDTOValidatorTests
    {
        #region DTOExemples
        public IEnumerable<SubscribeDTO> ValidDTOs =>
            new List<SubscribeDTO>{
                new SubscribeDTO{
                ClientUrl="test",
                Topic="test",
                Token="test",
                SubscriberId=1
                },
                new SubscribeDTO{
                ClientUrl="test",
                Topic="test",
                Token="test",
                SubscriberId=50
                }
            };
        public IEnumerable<SubscribeDTO> InvalidDTOs =>
            new List<SubscribeDTO>{
                new SubscribeDTO{
                    ClientUrl="",
                    Topic="",
                    Token="",
                    SubscriberId=50
                },
                 new SubscribeDTO{
                    ClientUrl="test",
                    Topic="test",
                    SubscriberId=50
                },
                new SubscribeDTO{
                    ClientUrl="test",
                    Topic="test",
                    Token="test",
                },
                new SubscribeDTO{
                    ClientUrl="test",
                    Topic="test",
                    Token="test",
                    SubscriberId=-50
                },
                new SubscribeDTO{
                    ClientUrl="test",
                    Topic="test",
                    Token="test",
                    SubscriberId=0
                },
                new SubscribeDTO{
                    ClientUrl="",
                    Topic="test"
                },
                new SubscribeDTO{
                    ClientUrl="test",
                    Topic=""
                },
                new SubscribeDTO{
                    ClientUrl="test"
                },
                new SubscribeDTO{
                    Topic="test"
                },
                new SubscribeDTO{}
            };

        #endregion

        [Theory]
        [MemberData(nameof(CreateTestData), new object[] { nameof(ValidDTOs), typeof(SubscribeDTOValidatorTests) })]
        public void Valid_SubscribeDTO_ReturnsTrue(SubscribeDTO dTO)
        {
            // Arrange
            var validator = new SubscribeDTOValidator();

            // Act
            var result = validator.Validate(dTO);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [MemberData(nameof(CreateTestData), new object[] { nameof(InvalidDTOs), typeof(SubscribeDTOValidatorTests) })]
        public void Invalid_SubscribeDTO_ReturnsFalse(SubscribeDTO dTO)
        {
            // Arrange
            var validator = new SubscribeDTOValidator();

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
