using Courier.API.DataTransferableObjects;
using FluentValidation;

namespace Courier.API.Validators
{
    public class AssignCourierToDeliveryDtoSaveValidator : AbstractValidator<AssignCourierToDeliveryDtoSave>,
        IFluentValidator
    {
        public AssignCourierToDeliveryDtoSaveValidator()
        {
            RuleFor(x => x.CourierId).NotEmpty();
            RuleFor(x => x.DeliveryId).NotEmpty();
        }
    }
}