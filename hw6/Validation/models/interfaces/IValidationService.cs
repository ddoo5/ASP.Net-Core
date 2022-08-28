using ContractControlCentre.Validation.Messages;

namespace ContractControlCentre.Validation.Models.Interfaces
{
    public interface IValidationService<TEntity> where TEntity : class
    {
        IReadOnlyList<IOperationFailure> ValidateEntity(TEntity item);
    }
}