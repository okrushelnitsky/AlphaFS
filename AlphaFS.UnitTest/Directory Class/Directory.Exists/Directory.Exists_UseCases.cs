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
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class Directory_ExistsTest
   {
      [TestMethod]
      public void Directory_Exists_UseCases_LocalAndNetwork_Success()
      {
         Directory_Exists_UseCases(false);
         Directory_Exists_UseCases(true);
      }


      private void Directory_Exists_UseCases(bool isNetwork)
      {
         // Issue #288: Directory.Exists on root drive problem has come back with recent updates


         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var sysDrive = UnitTestConstants.SysDrive + @"\";

         var tempPath = System.IO.Path.GetTempPath();

         if (isNetwork)
         {
            sysDrive = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(sysDrive);
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);
         }


         using (new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var randomName = UnitTestConstants.GetRandomFileNameWithDiacriticCharacters();

            // C:\randomName
            var nonExistingFolder1 = UnitTestConstants.SysDrive + @"\" + randomName;

            // C:randomName
            var nonExistingFolder2 = UnitTestConstants.SysDrive + randomName;


            // C:\randomName-exists
            var existingFolder1 = nonExistingFolder1 + "-exists";
            System.IO.Directory.CreateDirectory(existingFolder1);

            // C:randomName-exists
            var existingFolder2 = nonExistingFolder2 + "-exists";
            System.IO.Directory.CreateDirectory(existingFolder1);



            if (isNetwork)
            {
               nonExistingFolder1 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(nonExistingFolder1);
               nonExistingFolder2 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(nonExistingFolder2);
               existingFolder1 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(existingFolder1);
               existingFolder2 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(existingFolder2);
            }


            // Some use cases.
            var paths = new Dictionary<string, List<bool>>
            {
               {nonExistingFolder1, new List<bool> {false, false}},
               {nonExistingFolder2, new List<bool> {false, false}},
               {nonExistingFolder1 + @"\", new List<bool> {false, false}},
               {nonExistingFolder2 + @"\", new List<bool> {false, false}},

               {existingFolder1, new List<bool> {true, true}},
               {existingFolder2, new List<bool> {!isNetwork, !isNetwork}},
               {existingFolder1 + @"\", new List<bool> {true, true}},
               {existingFolder2 + @"\", new List<bool> {!isNetwork, !isNetwork}}
            };


            try
            {
               System.IO.Directory.SetCurrentDirectory(sysDrive);

               Console.WriteLine("Current directory: " + System.IO.Directory.GetCurrentDirectory());


               foreach (var path in paths)
               {
                  var sysIOshouldBe = path.Value[0];
                  var alphaFSshouldBe = path.Value[1];
                  var inputPath = path.Key;
                  var existSysIO = System.IO.Directory.Exists(inputPath);
                  var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputPath);


                  Console.WriteLine("\nSystem.IO   (should be {0}):\t[{1}]\t\tdirectory= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputPath);
                  Console.WriteLine("AlphaFS     (should be {0}):\t[{1}]\t\tdirectory= {2}", alphaFSshouldBe.ToString().ToUpperInvariant(), existAlpha, inputPath);


                  Assert.AreEqual(sysIOshouldBe, existSysIO);

                  Assert.AreEqual(alphaFSshouldBe, existAlpha);

                  Assert.AreEqual(sysIOshouldBe, alphaFSshouldBe);
               }
            }
            finally
            {
               System.IO.Directory.Delete(existingFolder1);
            }
         }


         Console.WriteLine();
      }
   }
}
