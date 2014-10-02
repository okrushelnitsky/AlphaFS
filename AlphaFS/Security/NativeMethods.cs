/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Alphaleonis.Win32.Security
{
   internal static partial class NativeMethods
   {
      #region AdjustTokenPrivileges

      /// <summary>The AdjustTokenPrivileges function enables or disables privileges in the specified access token. Enabling or disabling privileges in an access token requires TOKEN_ADJUST_PRIVILEGES access.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// To determine whether the function adjusted all of the specified privileges, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool AdjustTokenPrivileges(IntPtr tokenHandle, [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges, ref TokenPrivileges newState, uint bufferLength, out TokenPrivileges previousState, out uint returnLength);

      #endregion // AdjustTokenPrivileges

      #region LookupPrivilegeDisplayName

      /// <summary>The LookupPrivilegeDisplayName function retrieves the display name that represents a specified privilege.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "LookupPrivilegeDisplayNameW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool LookupPrivilegeDisplayName([MarshalAs(UnmanagedType.LPWStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPWStr)] string lpName, ref StringBuilder lpDisplayName, ref uint cchDisplayName, out uint lpLanguageId);

      #endregion // LookupPrivilegeDisplayName

      #region LookupPrivilegeValue

      /// <summary>The LookupPrivilegeValue function retrieves the locally unique identifier (LUID) used on a specified system to locally represent the specified privilege name.</summary>
      /// <returns>
      /// If the function succeeds, the function returns nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "LookupPrivilegeValueW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool LookupPrivilegeValue([MarshalAs(UnmanagedType.LPWStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPWStr)] string lpName, out Luid lpLuid);

      #endregion // LookupPrivilegeValue

      
      #region GetFileSecurity

      /// <summary>The GetFileSecurity function obtains specified information about the security of a file or directory.
      /// The information obtained is constrained by the caller's access rights and privileges.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFileSecurityW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileSecurity([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, SecurityInformation securityInfo, SafeHandle pSecurityDescriptor, uint nLength, out uint lpnLengthNeeded);

      #endregion // GetFileSecurity

      #region GetSecurityInfo

      /// <summary>The GetSecurityInfo function retrieves a copy of the security descriptor for an object specified by a handle.</summary>
      /// <returns>
      /// If the function succeeds, the function returns nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetSecurityInfo(SafeHandle handle, ObjectType objectType, SecurityInformation securityInfo, out IntPtr pSidOwner, out IntPtr pSidGroup, out IntPtr pDacl, out IntPtr pSacl, out SafeGlobalMemoryBufferHandle pSecurityDescriptor);

      #endregion // GetSecurityInfo

      #region SetSecurityInfo

      /// <summary>The SetSecurityInfo function sets specified security information in the security descriptor of a specified object. 
      /// The caller identifies the object by a handle.</summary>
      /// <returns>
      /// If the function succeeds, the function returns ERROR_SUCCESS.
      /// If the function fails, it returns a nonzero error code defined in WinError.h.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint SetSecurityInfo(SafeHandle handle, ObjectType objectType, SecurityInformation securityInfo, IntPtr psidOwner, IntPtr psidGroup, IntPtr pDacl, IntPtr pSacl);

      #endregion // SetSecurityInfo

      #region SetNamedSecurityInfo

      /// <summary>The SetNamedSecurityInfo function sets specified security information in the security descriptor of a specified object. The caller identifies the object by name.</summary>
      /// <returns>
      /// If the function succeeds, the function returns ERROR_SUCCESS.
      /// If the function fails, it returns a nonzero error code defined in WinError.h.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetNamedSecurityInfoW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint SetNamedSecurityInfo([MarshalAs(UnmanagedType.LPWStr)] string pObjectName, ObjectType objectType, SecurityInformation securityInfo, IntPtr pSidOwner, IntPtr pSidGroup, IntPtr pDacl, IntPtr pSacl);

      #endregion // SetNamedSecurityInfo


      #region GetSecurityDescriptorDacl

      /// <summary>The GetSecurityDescriptorDacl function retrieves a pointer to the discretionary access control list (DACL) in a specified security descriptor.</summary>
      /// <returns>
      /// If the function succeeds, the function returns nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetSecurityDescriptorDacl(SafeHandle pSecurityDescriptor, [MarshalAs(UnmanagedType.Bool)] out bool lpbDaclPresent, out IntPtr pDacl, [MarshalAs(UnmanagedType.Bool)] out bool lpbDaclDefaulted);

      #endregion // GetSecurityDescriptorDacl

      #region GetSecurityDescriptorSacl

      /// <summary>The GetSecurityDescriptorSacl function retrieves a pointer to the system access control list (SACL) in a specified security descriptor.</summary>
      /// <returns>
      /// If the function succeeds, the function returns nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetSecurityDescriptorSacl(SafeHandle pSecurityDescriptor, [MarshalAs(UnmanagedType.Bool)] out bool lpbSaclPresent, out IntPtr pSacl, [MarshalAs(UnmanagedType.Bool)] out bool lpbSaclDefaulted);

      #endregion // GetSecurityDescriptorSacl

      #region GetSecurityDescriptorGroup

      /// <summary>The GetSecurityDescriptorGroup function retrieves the primary group information from a security descriptor.</summary>
      /// <returns>
      /// If the function succeeds, the function returns nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetSecurityDescriptorGroup(SafeHandle pSecurityDescriptor, out IntPtr pGroup, [MarshalAs(UnmanagedType.Bool)] out bool lpbGroupDefaulted);

      #endregion // GetSecurityDescriptorGroup

      #region GetSecurityDescriptorControl

      /// <summary>The GetSecurityDescriptorControl function retrieves a security descriptor control and revision information.</summary>
      /// <returns>
      /// If the function succeeds, the function returns nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetSecurityDescriptorControl(SafeHandle pSecurityDescriptor, out SecurityDescriptorControl pControl, out uint lpdwRevision);

      #endregion // GetSecurityDescriptorControl

      #region GetSecurityDescriptorOwner

      /// <summary>The GetSecurityDescriptorOwner function retrieves the owner information from a security descriptor.</summary>
      /// <returns>
      /// If the function succeeds, the function returns nonzero.
      /// If the function fails, it returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetSecurityDescriptorOwner(SafeHandle pSecurityDescriptor, out IntPtr pOwner, [MarshalAs(UnmanagedType.Bool)] out bool lpbOwnerDefaulted);

      #endregion // GetSecurityDescriptorOwner

      #region GetSecurityDescriptorLength

      /// <summary>The GetSecurityDescriptorLength function returns the length, in bytes, of a structurally valid security descriptor. The length includes the length of all associated structures.</summary>
      /// <returns>
      /// If the function succeeds, the function returns the length, in bytes, of the SECURITY_DESCRIPTOR structure.
      /// If the SECURITY_DESCRIPTOR structure is not valid, the return value is undefined.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetSecurityDescriptorLength(SafeHandle pSecurityDescriptor);

      #endregion // GetSecurityDescriptorLength


      #region LocalFree

      /// <summary>Frees the specified local memory object and invalidates its handle.</summary>
      /// <returns>
      /// If the function succeeds, the return value is <c>null</c>.
      /// If the function fails, the return value is equal to a handle to the local memory object. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>
      /// Note  The local functions have greater overhead and provide fewer features than other memory management functions.
      /// New applications should use the heap functions unless documentation states that a local function should be used.
      /// For more information, see Global and Local Functions.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      internal static extern IntPtr LocalFree(IntPtr hMem);

      #endregion // LocalFree
   }
}