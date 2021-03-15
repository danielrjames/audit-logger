using Microsoft.AspNetCore.Identity;
using System;

namespace app.Domain.Entities.User
{
    public class ApplicationUser : IdentityUser<Guid> // extending this to a GUID PK, because default guid-string is not good
    {

    }
}
