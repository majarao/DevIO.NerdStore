using FluentValidation.Results;

namespace DevIO.NerdStore.Core.Messages.Integration;

public class ResponseMessage(ValidationResult? validationResult) : Message
{
    public ValidationResult? ValidationResult { get; set; } = validationResult;
}
