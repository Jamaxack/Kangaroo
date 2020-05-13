using Courier.API.DataTransferableObjects;
using FluentValidation;

namespace Courier.API.Validators
{
    public class AssignCourierToDeliveryDTOValidator : AbstractValidator<AssignCourierToDeliveryDTO>, IFluentValidator
    {
        public AssignCourierToDeliveryDTOValidator()
        {
            RuleFor(x => x.CourierId).NotEmpty();
            RuleFor(x => x.DelivertId).NotEmpty();
        }
    }
}
