/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      /// <summary>Makes an extended long path from the specified <paramref name="path"/> by prefixing <see cref="LongPathPrefix"/>.</summary>
      /// <returns>The <paramref name="path"/> prefixed with a <see cref="LongPathPrefix"/>, the minimum required full path is: "C:\".</returns>
      /// <remarks>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path to the file or directory, this can also be an UNC path.</param>
      [SecurityCritical]
      public static string GetLongPath(string path)
      {
         return GetLongPathCore(path, GetFullPathOptions.None);
      }


      /// <summary>[AlphaFS] Converts the specified existing path to its regular long form.</summary>
      /// <returns>The regular full path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetLongFrom83ShortPath(string path)
      {
         return GetLongShort83PathCore(null, path, false);
      }


      /// <summary>[AlphaFS] Converts the specified existing path to its regular long form.</summary>
      /// <returns>The regular full path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetLongFrom83ShortPathTransacted(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathCore(transaction, path, false);
      }


      /// <summary>[AlphaFS] Gets the regular path from long prefixed one. i.e.: "\\?\C:\Temp\file.txt" to C:\Temp\file.txt" or: "\\?\UNC\Server\share\file.txt" to "\\Server\share\file.txt".</summary>
      /// <returns>Regular form path string.</returns>
      /// <remarks>This method does not handle paths with volume names, eg. \\?\Volume{GUID}\Folder\file.txt.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetRegularPath(string path)
      {
         return GetRegularPathCore(path, GetFullPathOptions.CheckInvalidPathChars, false);
      }


      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetShort83Path(string path)
      {
         return GetLongShort83PathCore(null, path, true);
      }


      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetShort83PathTransacted(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathCore(transaction, path, true);
      }


      /// <summary>[AlphaFS] Determines whether the specified path starts with a <see cref="LongPathPrefix"/> or <see cref="LongPathUncPrefix"/>.</summary>
      /// <returns><see langword="true"/> if the specified path has a long path (UNC) prefix, <see langword="false"/> otherwise.</returns>
      /// <param name="path">The path to the file or directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public static bool IsLongPath(string path)
      {
         return !Utils.IsNullOrWhiteSpace(path) && path.StartsWith(LongPathPrefix, StringComparison.Ordinal);
      }




      internal static string GetCleanExceptionPath(string path)
      {
         return GetRegularPathCore(path, GetFullPathOptions.None, true) .TrimEnd(DirectorySeparatorChar, WildcardStarMatchAllChar);
      }


      /// <summary>Makes an extended long path from the specified <paramref name="path"/> by prefixing <see cref="LongPathPrefix"/>.</summary>
      /// <returns>The <paramref name="path"/> prefixed with a <see cref="LongPathPrefix"/>, the minimum required full path is: "C:\".</returns>
      /// <remarks>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to the file or directory, this can also be an UNC path.</param>
      /// <param name="options">Options for controlling the full path retrieval.</param>
      [SecurityCritical]
      internal static string GetLongPathCore(string path, GetFullPathOptions options)
      {
         if (null == path)
            throw new ArgumentNullException("path");


         if (path.Length == 0 || Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "path");


         if (options != GetFullPathOptions.None)
            path = ApplyFullPathOptions(path, options);


         // ".", "C:"
         if (path.Length <= 2 || path.StartsWith(LongPathPrefix, StringComparison.Ordinal) || path.StartsWith(LogicalDrivePrefix, StringComparison.Ordinal) || path.StartsWith(NonInterpretedPathPrefix, StringComparison.Ordinal))
            return path;


         if (path.StartsWith(UncPrefix, StringComparison.Ordinal))
            return LongPathUncPrefix + path.Substring(UncPrefix.Length);

         
         return IsPathRooted(path, false) && IsLogicalDriveCore(path, false, PathFormat.LongFullPath) ? LongPathPrefix + path : path;
      }


      /// <summary>Retrieves the short path form, or the regular long form of the specified <paramref name="path"/>.</summary>
      /// <returns>If <paramref name="getShort"/> is <see langword="true"/>, a path of the 8.3 form otherwise the regular long form.</returns>
      /// <remarks>
      ///   <para>Will fail on NTFS volumes with disabled 8.3 name generation.</para>
      ///   <para>The path must actually exist to be able to get the short- or long path name.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <param name="getShort"><see langword="true"/> to retrieve the short path form, <see langword="false"/> to retrieve the regular long form from the 8.3 <paramref name="path"/>.</param>
      [SecurityCritical]
      private static string GetLongShort83PathCore(KernelTransaction transaction, string path, bool getShort)
      {
         var pathLp = GetFullPathCore(transaction, path, GetFullPathOptions.AsLongPath | GetFullPathOptions.FullCheck);

         var buffer = new StringBuilder();
         var actualLength = getShort ? NativeMethods.GetShortPathName(pathLp, null, 0) : (uint) path.Length;

         while (actualLength > buffer.Capacity)
         {
            buffer = new StringBuilder((int) actualLength);
            actualLength = getShort

               // GetShortPathName()
               // 2014-01-29: MSDN confirms LongPath usage.

               // GetLongPathName()
               // 2014-01-29: MSDN confirms LongPath usage.

               ? NativeMethods.GetShortPathName(pathLp, buffer, (uint) buffer.Capacity)
               : transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  ? NativeMethods.GetLongPathName(pathLp, buffer, (uint) buffer.Capacity)
                  : NativeMethods.GetLongPathNameTransacted(pathLp, buffer, (uint) buffer.Capacity, transaction.SafeHandle);


            var lastError = Marshal.GetLastWin32Error();

            if (actualLength == 0)
               NativeError.ThrowException(lastError, pathLp);
         }

         return GetRegularPathCore(buffer.ToString(), GetFullPathOptions.None, false);
      }


      /// <summary>Gets the regular path from a long path.</summary>
      /// <returns>
      ///   <para>Returns the regular form of a long <paramref name="path"/>.</para>
      ///   <para>For example: "\\?\C:\Temp\file.txt" to: "C:\Temp\file.txt", or: "\\?\UNC\Server\share\file.txt" to: "\\Server\share\file.txt".</para>
      /// </returns>
      /// <remarks>
      ///   MSDN: String.TrimEnd Method notes to Callers: http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path.</param>
      /// <param name="options">Options for controlling the full path retrieval.</param>
      /// <param name="allowEmpty">When <see langword="false"/>, throws an <see cref="ArgumentException"/>.</param>
      [SecurityCritical]
      internal static string GetRegularPathCore(string path, GetFullPathOptions options, bool allowEmpty)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (!allowEmpty && (path.Length == 0 || Utils.IsNullOrWhiteSpace(path)))
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "path");

         if (options != GetFullPathOptions.None)
            path = ApplyFullPathOptions(path, options);


         if (path.StartsWith(DosDeviceUncPrefix, StringComparison.OrdinalIgnoreCase))
            return UncPrefix + path.Substring(DosDeviceUncPrefix.Length);

         if (path.StartsWith(NonInterpretedPathPrefix, StringComparison.Ordinal))
            return path.Substring(NonInterpretedPathPrefix.Length);


         return path.StartsWith(GlobalRootPrefix, StringComparison.OrdinalIgnoreCase)
                || path.StartsWith(VolumePrefix, StringComparison.OrdinalIgnoreCase)
                || !path.StartsWith(LongPathPrefix, StringComparison.Ordinal)
            ? path
            : (path.StartsWith(LongPathUncPrefix, StringComparison.OrdinalIgnoreCase)
               ? UncPrefix + path.Substring(LongPathUncPrefix.Length)
               : path.Substring(LongPathPrefix.Length));
      }


      /// <summary>Gets the path as a long full path.</summary>
      /// <returns>The path as an extended length path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to convert.</param>
      /// <param name="pathFormat">The path format to use.</param>
      /// <param name="options">Options for controlling the operation. Note that on .NET 3.5 the TrimEnd option has no effect.</param>
      internal static string GetExtendedLengthPathCore(KernelTransaction transaction, string path, PathFormat pathFormat, GetFullPathOptions options)
      {
         if (null == path)
            return null;


         switch (pathFormat)
         {
            case PathFormat.LongFullPath:
               if (options != GetFullPathOptions.None)
               {
                  // If pathFormat equals LongFullPath it is possible that the trailing backslashg ('\') is not added or removed.
                  // Prevent that.

                  options &= ~GetFullPathOptions.CheckAdditional;
                  options &= ~GetFullPathOptions.CheckInvalidPathChars;
                  options &= ~GetFullPathOptions.FullCheck;
                  options &= ~GetFullPathOptions.TrimEnd;
                  
                  path = ApplyFullPathOptions(path, options);
               }

               return path;


            case PathFormat.FullPath:
               return GetLongPathCore(path, GetFullPathOptions.None);

            case PathFormat.RelativePath:
#if NET35
               // .NET 3.5 the TrimEnd option has no effect.
               options = options & ~GetFullPathOptions.TrimEnd;
#endif
               return GetFullPathCore(transaction, path, GetFullPathOptions.AsLongPath | options);

            default:
               throw new ArgumentException("Invalid value: " + pathFormat, "pathFormat");
         }
      }
   }
}
