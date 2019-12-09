using System;

namespace Core.Data.Dto {
    public class SyncResultDto {
        public string Name { get; set; }
        public TimeSpan LeadTime { get; set; }

        public int Count { get; set; } = 0;

        public SyncResultDto(string name) {
            Name = name;
        }
    }
}
