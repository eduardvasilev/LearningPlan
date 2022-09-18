using System;

namespace LearningPlan.DomainModel.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string message) : base(message)
    {
     //   
    }
}