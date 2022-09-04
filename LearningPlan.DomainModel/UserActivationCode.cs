using System;

namespace LearningPlan.DomainModel;

public class UserActivationCode : EntityBase
{
    public string UserId { get; set; }

    public Guid Code { get; set; }
}