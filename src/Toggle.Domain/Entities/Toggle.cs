namespace Toggle.Domain.Entities
{
    public class Toggle
    {
        private Toggle() { }

        public Toggle(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Toggle(int serviceId, string version, string name, string value)
            : this(name, value)
        {
            ServiceId = serviceId;
            Version = version;
        }

        public int Id { get; private set; }
        public int? ServiceId { get; private set; }
        public string Version { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }

        public Service Service { get; private set; }

        public bool IsGlobal =>
            !ServiceId.HasValue;

        public bool BelongsTo(int serviceId, string version) =>
            ServiceId == serviceId && Version == version;

        public void Update(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
