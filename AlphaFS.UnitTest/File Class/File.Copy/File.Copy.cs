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
using System.Globalization;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class File_CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Copy_LocalAndNetwork_Success()
      {
         File_Copy(false);
         File_Copy(true);
      }


      [TestMethod]
      public void File_Copy_WithProgress_LocalAndNetwork_Success()
      {
         File_Copy_WithProgress(false);
         File_Copy_WithProgress(true);
      }


      
      
      private void File_Copy(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            // Min: 1 bytes, Max: 10485760 = 10 MB.
            var fileLength = new Random().Next(1, 10485760);
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName, fileLength);
            var fileCopy = rootDir.RandomFileFullPath;

            Console.WriteLine("Src File Path: [{0}] [{1}]", Alphaleonis.Utils.UnitSizeToText(fileLength), fileSource);
            Console.WriteLine("Dst File Path: [{0}]", fileCopy);
            
            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The file does not exists, but is expected to.");


            Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy);
            

            Assert.IsTrue(System.IO.File.Exists(fileCopy), "The file does not exists, but is expected to.");
            

            var fileLen = new System.IO.FileInfo(fileCopy).Length;
            
            Assert.AreEqual(fileLength, fileLen, "The file copy is: {0} bytes, but is expected to be: {1} bytes.", fileLen, fileLength);
            
            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The original file does not exist, but is expected to.");
         }


         Console.WriteLine();
      }


      private void File_Copy_WithProgress(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            // Min: 1 bytes, Max: 10485760 = 10 MB.
            var fileLength = new Random().Next(1, 10485760);
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName, fileLength);
            var fileCopy = rootDir.RandomFileFullPath;

            Console.WriteLine("Src File Path: [{0:N0} ({1}) [{2}]", fileLength, Alphaleonis.Utils.UnitSizeToText(fileLength), fileSource);
            Console.WriteLine("Dst File Path: [{0}]", fileCopy);

            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The file does not exists, but is expected to.");


            
            
            // Allow copy to overwrite an existing file.
            const Alphaleonis.Win32.Filesystem.CopyOptions copyOptions = Alphaleonis.Win32.Filesystem.CopyOptions.None;

            // The copy progress handler.
            var callback = new Alphaleonis.Win32.Filesystem.CopyMoveProgressRoutine(FileCopyProgressHandler);

            Console.WriteLine();


            // You can pass any piece of data to userProgressData. This data can be accessed from the callback method.
            // Specify file length for assertion.
            var userProgressData = fileLength;


            var copyResult = Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy, copyOptions, callback, userProgressData);

            UnitTestConstants.Dump(copyResult, -18);


            Assert.IsTrue(System.IO.File.Exists(fileCopy), "The file does not exists, but is expected to.");


            var fileLen = new System.IO.FileInfo(fileCopy).Length;

            Assert.AreEqual(fileLength, fileLen, "The file copy is: {0} bytes, but is expected to be: {1} bytes.", fileLen, fileLength);

            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The original file does not exist, but is expected to.");
         }


         Console.WriteLine();
      }


      private static Alphaleonis.Win32.Filesystem.CopyMoveProgressResult FileCopyProgressHandler(long totalFileSize, long totalBytesTransferred, long streamSize,
         long streamBytesTransferred, int streamNumber, Alphaleonis.Win32.Filesystem.CopyMoveProgressCallbackReason callbackReason, object userData)
      {
         if (callbackReason == Alphaleonis.Win32.Filesystem.CopyMoveProgressCallbackReason.StreamSwitch)

            Assert.AreEqual(0, totalBytesTransferred);


         else
         {
            var pct = Convert.ToDouble(totalBytesTransferred, CultureInfo.InvariantCulture) / totalFileSize * 100;

            Console.WriteLine("\tCallback: Copied: [{0}%] --> [{1:N0}] bytes.", pct.ToString("N2", CultureInfo.CurrentCulture), totalBytesTransferred);

            Assert.IsTrue(totalBytesTransferred > 0);


            var bytes = Convert.ToInt64(userData);

            if (pct.Equals(100))
            {
               Assert.AreEqual(totalBytesTransferred, bytes);
               Assert.AreEqual(totalFileSize, bytes);
            }

            else
               Assert.IsTrue(totalBytesTransferred < bytes);
         }


         return Alphaleonis.Win32.Filesystem.CopyMoveProgressResult.Continue;
      }
   }
}
