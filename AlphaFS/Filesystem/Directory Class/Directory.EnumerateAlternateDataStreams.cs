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

using System.Collections.Generic;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      /// <summary>[AlphaFS] Enumerates the streams of type :$DATA from the specified directory.</summary>
      /// <param name="path">The path to the directory to enumerate streams of.</param>
      /// <returns>The streams of type :$DATA in the specified directory.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(string path)
      {
         return File.EnumerateAlternateDataStreamsCore(null, path, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Enumerates the streams of type :$DATA from the specified directory.</summary>
      /// <param name="path">The path to the directory to enumerate streams of.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The streams of type :$DATA in the specified directory.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(string path, PathFormat pathFormat)
      {
         return File.EnumerateAlternateDataStreamsCore(null, path, pathFormat);
      }

      /// <summary>[AlphaFS] Enumerates the streams of type :$DATA from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory to enumerate streams of.</param>
      /// <returns>The streams of type :$DATA in the specified directory.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreamsTransacted(KernelTransaction transaction, string path)
      {
         return File.EnumerateAlternateDataStreamsCore(transaction, path, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Enumerates the streams of type :$DATA from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory to enumerate streams of.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The streams of type :$DATA in the specified directory.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreamsTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.EnumerateAlternateDataStreamsCore(transaction, path, pathFormat);
      }
   }
}
