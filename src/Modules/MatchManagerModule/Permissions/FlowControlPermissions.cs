﻿using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MatchManagerModule.Permissions;

[PermissionGroup]
public enum FlowControlPermissions
{
    [Description("[Permission.RestartMatch]")]
    RestartMatch,
    
    [Description("[Permission.EndRound]")]
    EndRound,
    
    [Description("[Permission.SkipMap]")]
    SkipMap
}
