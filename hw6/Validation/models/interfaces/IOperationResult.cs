using System;
namespace ContractControlCentre.Validation.Models.Interfaces
{
    public interface IOperationResult<TResult>
    {
        TResult Result { get; }
        IReadOnlyList<IOperationFailure> Failures { get; }
        bool Succeed { get; }
    }
}

