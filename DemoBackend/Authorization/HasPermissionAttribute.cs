using Microsoft.AspNetCore.Authorization;

namespace DemoBackend.Authorization;

public class HasPermissionAttribute(Permission permission)
    : AuthorizeAttribute(policy: permission.ToString());