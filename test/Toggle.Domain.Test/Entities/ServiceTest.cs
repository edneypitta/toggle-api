using FluentAssertions;
using Toggle.Domain.Entities;
using Xunit;

namespace Toggle.Domain.Test.Entities
{
    public class ServiceTest
    {
        [Fact]
        public void ConstructorShouldBuildService()
        {
            var service = new Service("name");
            service.Name.Should().Be("name");
        }
    }
}
