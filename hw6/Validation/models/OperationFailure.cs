using ContractControlCentre.Validation.Models.Interfaces;

namespace ContractControlCentre.Validation.Models
{
    public sealed class OperationFailure : IOperationFailure
    {
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}

