using Courier.API.DataTransferableObjects;
using FluentValidation;

namespace Courier.API.Validators
{
    public class CourierLocationDTOValidator : AbstractValidator<CourierLocationDTO>, IFluentValidator
    {
        public CourierLocationDTOValidator()
        {
            RuleFor(x => x.CourierId).NotEmpty();
            RuleFor(x => x.Latitude).Must(latitude => latitude >= -90 && latitude <= 90).WithMessage("Latitude must be between -90 and 90 degrees inclusive.");
            RuleFor(x => x.Longitude).Must(longitude => longitude >= -180 && longitude <= 180).WithMessage("Longitude must be between -180 and 180 degrees inclusive.");
        }
    }
}
