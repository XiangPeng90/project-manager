using System;
using System.Collections.Generic;
using System.Linq;
using ProjectManager.Domain.Shared;
using ProjectManager.Domain.Shared.Enums;

namespace ProjectManager.Domain.Entities
{
    public class WorkPackage:AuditedEntity
    {
        public long ProjectId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Owner { get; set; }
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Enabled;
    }
}