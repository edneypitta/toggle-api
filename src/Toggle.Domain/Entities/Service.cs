using System.Collections.Generic;

namespace Toggle.Domain.Entities
{
    public class Service
    {
        private Service() { }

        public Service(string name)
        {
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public ICollection<Toggle> Toggles { get; set; }
    }
}
