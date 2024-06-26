﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Win32.Foundation;
using Snap.Hutao.Win32.System.Com;
using Snap.Hutao.Win32.System.SystemServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Snap.Hutao.Win32.UI.Shell;

[SupportedOSPlatform("windows5.1.2600")]
[Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
internal unsafe struct IShellItem
{
    public readonly Vftbl* ThisPtr;

    internal static unsafe ref readonly Guid IID
    {
        get
        {
            ReadOnlySpan<byte> data = [0x1E, 0x6D, 0x82, 0x43, 0x18, 0xE7, 0xEE, 0x42, 0xBC, 0x55, 0xA1, 0xE2, 0x61, 0xC3, 0x7B, 0xFE];
            return ref Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(data));
        }
    }

    public unsafe HRESULT QueryInterface<TInterface>(ref readonly Guid riid, out TInterface* pvObject)
        where TInterface : unmanaged
    {
        fixed (Guid* riid2 = &riid)
        {
            fixed (TInterface** ppvObject = &pvObject)
            {
                return ThisPtr->IUnknownVftbl.QueryInterface((IUnknown*)Unsafe.AsPointer(ref this), riid2, (void**)ppvObject);
            }
        }
    }

    public uint AddRef()
    {
        return ThisPtr->IUnknownVftbl.AddRef((IUnknown*)Unsafe.AsPointer(ref this));
    }

    public uint Release()
    {
        return ThisPtr->IUnknownVftbl.Release((IUnknown*)Unsafe.AsPointer(ref this));
    }

    public HRESULT GetDisplayName(SIGDN sigdnName, out PWSTR pszName)
    {
        fixed (PWSTR* ppszName = &pszName)
        {
            return ThisPtr->GetDisplayName((IShellItem*)Unsafe.AsPointer(ref this), sigdnName, ppszName);
        }
    }

    internal readonly struct Vftbl
    {
        internal readonly IUnknown.Vftbl IUnknownVftbl;
        internal readonly delegate* unmanaged[Stdcall]<IShellItem*, IBindCtx*, Guid*, Guid*, void**, HRESULT> BindToHandler;
        internal readonly delegate* unmanaged[Stdcall]<IShellItem*, IShellItem**, HRESULT> GetParent;
        internal readonly delegate* unmanaged[Stdcall]<IShellItem*, SIGDN, PWSTR*, HRESULT> GetDisplayName;
        internal readonly delegate* unmanaged[Stdcall]<IShellItem*, SFGAO_FLAGS, SFGAO_FLAGS*, HRESULT> GetAttributes;
        internal readonly delegate* unmanaged[Stdcall]<IShellItem*, IShellItem*, uint, int*, HRESULT> Compare;
    }
}