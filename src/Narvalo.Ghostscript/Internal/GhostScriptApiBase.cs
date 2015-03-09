// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Internal
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    using Narvalo.GhostScript.Options;

    internal abstract class GhostScriptApiBase : IGhostScriptApi
    {
        protected GhostScriptApiBase() { }

        #region IGhostScriptApi

        public void Execute<T>(GhostScriptArgs<T> args) where T : Device
        {
            Require.NotNull(args, "args");

            Execute(args.ToParamArray());
        }

        public void Execute(string[] args)
        {
            Require.NotNull(args, "args");

            string[] tmpArgs = PrepareParams(args);

            GCHandle[] argHandles = null;
            IntPtr[] argPtrs = null;

            try
            {
                var length = tmpArgs.Length;

                argHandles = new GCHandle[length];
                argPtrs = new IntPtr[length];

                for (var i = 0; i < length; i++)
                {
                    argHandles[i] = GCHandle.Alloc(Encoding.ASCII.GetBytes(tmpArgs[i]), GCHandleType.Pinned);
                    argPtrs[i] = argHandles[i].AddrOfPinnedObject();
                }

                // See http://stackoverflow.com/questions/10232320/using-try-catch-in-net-structure-constructor
                GCHandle argPtrsHandle = GCHandle.Alloc(argPtrs, GCHandleType.Pinned);

                try
                {
                    ExecuteNative(length, argPtrsHandle.AddrOfPinnedObject());
                }
                finally
                {
                    if (argPtrsHandle.IsAllocated)
                    {
                        argPtrsHandle.Free();
                    }
                }
            }
            finally
            {
                if (argHandles != null)
                {
                    foreach (var handle in argHandles)
                    {
                        if (handle.IsAllocated)
                        {
                            handle.Free();
                        }
                    }

                    argHandles = null;
                }
            }
        }

        #endregion

        protected abstract void ExecuteNative(int argc, IntPtr argv);

        private static string[] PrepareParams(string[] args)
        {
            string[] preparedArgs;

            if (String.IsNullOrEmpty(args[0]))
            {
                preparedArgs = args;
            }
            else
            {
                // Ghostscript ignores the first argument in the array therefore, if the array
                // doesn't have a blank item as the first item, create one saves callers having
                // to think about dummy values etc...
                preparedArgs = new String[args.Length + 1];
                preparedArgs[0] = String.Empty;
                args.CopyTo(preparedArgs, 1);
            }

            return preparedArgs;
        }
    }
}
