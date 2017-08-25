using FluentAssertions;
using Xunit;

namespace Toggle.Domain.Test.Entities
{
    public class ToggleTest
    {
        [Fact]
        public void ConstructorShouldBuildToggleWithNameAndValue()
        {
            var toggle = new Domain.Entities.Toggle("name", "value");
            toggle.Name.Should().Be("name");
            toggle.Value.Should().Be("value");
        }

        [Fact]
        public void ConstructorShouldBuildFullToggle()
        {
            var toggle = new Domain.Entities.Toggle(1, "1.0", "name", "value");
            toggle.ServiceId.Should().Be(1);
            toggle.Version.Should().Be("1.0");
            toggle.Name.Should().Be("name");
            toggle.Value.Should().Be("value");
        }

        [Fact]
        public void IsGlobalShouldReturnIfHasService()
        {
            var toggle = new Domain.Entities.Toggle("name", "value");
            toggle.IsGlobal.Should().BeTrue();

            toggle = new Domain.Entities.Toggle(1, "1.0", "name", "value");
            toggle.IsGlobal.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, "1.0", true)]
        [InlineData(2, "1.0", false)]
        [InlineData(1, "1.1", false)]
        public void BelongsToShouldCompareServiceAndVersion(int serviceId, string version, bool expectedBelongsTo)
        {
            var toggle = new Domain.Entities.Toggle(1, "1.0", "name", "value");
            toggle.BelongsTo(serviceId, version).Should().Be(expectedBelongsTo);
        }

        [Fact]
        public void UpdateShouldSetNameAndValue()
        {
            var toggle = new Domain.Entities.Toggle("name", "value");
            toggle.Update("new name", "new value");
            toggle.Name.Should().Be("new name");
            toggle.Value.Should().Be("new value");
        }
    }
}
