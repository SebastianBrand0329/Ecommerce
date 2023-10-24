using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Utilities
{
    public class ErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordRequiresLower),
                Description = "La contraseña debe tener al menos una Minuscula"
            };
        }
    }
}
