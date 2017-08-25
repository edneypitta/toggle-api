using System.Collections.Generic;

namespace Toggle.Domain.Test
{
    public class ToggleListBuilder
    {
        private readonly List<Domain.Entities.Toggle> toggles = new List<Domain.Entities.Toggle>();

        public ToggleListBuilder WithGlobalToggle(string name, string value) =>
            WithToggle(new Domain.Entities.Toggle(name, value));

        public ToggleListBuilder WithDefaultGlobalToggle() =>
            WithGlobalToggle("default_global", "value");

        public ToggleListBuilder WithServiceToggle(string name, string value) =>
            WithToggle(new Domain.Entities.Toggle(1, "1.0", name, value));

        public ToggleListBuilder WithDefaultServiceToggle() =>
            WithServiceToggle("default_service", "value");

        private ToggleListBuilder WithToggle(Domain.Entities.Toggle toggle)
        {
            toggles.Add(toggle);
            return this;
        }

        public IEnumerable<Domain.Entities.Toggle> Build() => toggles;
    }
}