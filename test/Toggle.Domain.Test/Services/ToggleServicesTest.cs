using System.Linq;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Toggle.Domain.Entities;
using Toggle.Domain.Exceptions;
using Toggle.Domain.Repositories;
using Toggle.Domain.Services;
using Xunit;

namespace Toggle.Domain.Test.Services
{
    public class ToggleServicesTest
    {
        private readonly Mock<IServices> mockServices;
        private readonly Mock<IToggles> mockToggles;
        private readonly ToggleServices toggleServices;

        public ToggleServicesTest()
        {
            mockServices = new Mock<IServices>();
            mockToggles = new Mock<IToggles>();
            toggleServices = new ToggleServices(
                mockServices.Object,
                mockToggles.Object);
        }

        [Fact]
        public void GetFromServiceShouldReturnGlobalToggles()
        {
            mockToggles
                .Setup(m => m.GetGlobals())
                .Returns(new ToggleListBuilder().WithDefaultGlobalToggle().Build());

            var toggles = toggleServices.GetFromService(1, "1.0");

            toggles.ShouldBeEquivalentTo(new ToggleListBuilder().WithDefaultGlobalToggle().Build());
        }

        [Fact]
        public void GetFromServiceShouldReturnServiceToggles()
        {
            mockToggles
                .Setup(m => m.GetFromService(1, "1.0"))
                .Returns(new ToggleListBuilder().WithDefaultServiceToggle().Build());

            var toggles = toggleServices.GetFromService(1, "1.0");

            toggles.ShouldBeEquivalentTo(new ToggleListBuilder().WithDefaultServiceToggle().Build());
        }

        [Fact]
        public async Task GetByIdShouldReturnToggle()
        {
            mockToggles
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult(new ToggleListBuilder().WithDefaultGlobalToggle().Build().First()));

            var toggle = await toggleServices.GetById(1);

            toggle.Should().NotBeNull();
        }

        [Fact]
        public void GetFromServiceShouldMergeToggles()
        {
            mockToggles
                .Setup(m => m.GetGlobals())
                .Returns(
                    new ToggleListBuilder()
                    .WithDefaultGlobalToggle()
                    .WithGlobalToggle("global", "true").Build());
            mockToggles
                .Setup(m => m.GetFromService(1, "1.0"))
                .Returns(
                    new ToggleListBuilder()
                    .WithDefaultServiceToggle()
                    .WithServiceToggle("global", "false").Build);

            var toggles = toggleServices.GetFromService(1, "1.0");

            toggles.Should().Contain(t => t.Name == "default_global");
            toggles.Should().Contain(t => t.Name == "default_service");
            toggles.Should().Contain(t => t.Name == "global" && t.Value == "false");
        }

        [Fact]
        public async Task CreateShouldReturnExceptionIfServiceNotFound()
        {
            mockServices
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult((Service)null));

            await Assert.ThrowsAsync<ServiceNotFoundException>(
                async () => await toggleServices.CreateAsync(1, null, null, null));
        }

        [Fact]
        public async Task CreateShouldCreateToggle()
        {
            mockServices
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult(new Service("name")));

            await toggleServices.CreateAsync(1, "1.0", "name", "value");
            mockToggles.Verify(m => m.CreateAsync(
                It.Is<Domain.Entities.Toggle>(
                    t => CompareToggle(t, 1, "1.0", "name", "value"))));
        }

        [Fact]
        public async Task UpdateShouldReturnExceptionIfToggleNotFound()
        {
            mockToggles
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult((Domain.Entities.Toggle)null));

            await Assert.ThrowsAsync<ToggleNotFoundException>(
                async () => await toggleServices.UpdateAsync(1, null, null));
        }

        [Fact]
        public async Task UpdateShouldUpdateToggle()
        {
            mockToggles
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult(
                    new ToggleListBuilder().WithDefaultServiceToggle().Build().First()));

            await toggleServices.UpdateAsync(1, "new name", "new value");
            mockToggles.Verify(m => m.UpdateAsync(
                It.Is<Domain.Entities.Toggle>(
                    t => CompareToggle(t, 1, "1.0", "new name", "new value"))));
        }

        [Fact]
        public async Task DeleteShouldReturnExceptionIfToggleNotFound()
        {
            mockToggles
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult((Domain.Entities.Toggle)null));

            await Assert.ThrowsAsync<ToggleNotFoundException>(
                async () => await toggleServices.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteShouldDeleteToggle()
        {
            mockToggles
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult(
                    new ToggleListBuilder().WithDefaultServiceToggle().Build().First()));

            await toggleServices.DeleteAsync(1);
            mockToggles.Verify(m => m.DeleteAsync(
                It.Is<Domain.Entities.Toggle>(
                    t => CompareToggle(t, 1, "1.0", "default_service", "value"))));
        }

        private static bool CompareToggle(Domain.Entities.Toggle toggle, int serviceId,
            string version, string name, string value) =>
            toggle.ServiceId == serviceId && toggle.Version == version &&
            toggle.Name == name && toggle.Value == value;
    }
}

