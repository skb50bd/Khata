using System;

namespace Khata.Domain
{
    [Flags]
    public enum Role
    {
        Admin = 0,
        User,
        Manager,
        Employee,
        Guest
    }
}