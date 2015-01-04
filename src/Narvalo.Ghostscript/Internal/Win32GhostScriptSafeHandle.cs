﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Internal
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;

    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    sealed class Win32GhostScriptSafeHandle : SafeHandle
    {
        public Win32GhostScriptSafeHandle()
            : base(IntPtr.Zero, true /* onwshandle */)
        {
        }

        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        protected override bool ReleaseHandle()
        {
            int code = Win32NativeMethods.gsapi_exit(handle);
            Win32NativeMethods.gsapi_delete_instance(handle);
            return NativeMethodsUtil.IsSuccess(code);
        }
    }
}
