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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_ByHandleFileInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_GetFileInfoByHandle_LocalAndNetwork_Success()
      {
         Directory_GetFileInfoByHandle(false);
         Directory_GetFileInfoByHandle(true);
      }




      private void Directory_GetFileInfoByHandle(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]]", folder);


            var dirInfo = new System.IO.DirectoryInfo(folder);
            dirInfo.Create();


            var bhfi = Alphaleonis.Win32.Filesystem.Directory.GetFileInfoByHandle(dirInfo.FullName);
            Assert.IsTrue(UnitTestConstants.Dump(bhfi, -18));


            Assert.AreEqual(dirInfo.CreationTimeUtc, bhfi.CreationTimeUtc);
            Assert.AreEqual(dirInfo.LastAccessTimeUtc, bhfi.LastAccessTimeUtc);
            Assert.AreEqual(dirInfo.LastWriteTimeUtc, bhfi.LastWriteTimeUtc);

         }

         Console.WriteLine();
      }
   }
}
