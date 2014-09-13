using DSA.Dictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tests
{
   [TestClass]
   public sealed class ArrayDictionaryTests
   {
      [TestMethod]
      public void BasicOperations()
      {
         var dictionary = new ArrayDictionary<int, int>();
         dictionary.Add(1, 1);
         dictionary.Add(2, 1);

         Assert.AreEqual(1, dictionary[1]);
         Assert.AreEqual(1, dictionary[2]);

         var setOfExpectedKvps = new HashSet<KeyValuePair<int, int>>
         {
            new KeyValuePair<int, int>(1, 1),
            new KeyValuePair<int, int>(2, 1)
         };

         foreach (var i in dictionary)
         {
            Assert.IsTrue(setOfExpectedKvps.Contains(i));
         }
      }

      [TestMethod]
      public void OverrideKey()
      {
         var dictionary = new ArrayDictionary<int, int>();
         dictionary.Add(1, 1);
         dictionary[1] = 2;

         Assert.AreEqual(2, dictionary[1]);
      }

      [TestMethod]
      public void HeavyLoadOverride()
      {
         var reference = new Dictionary<int, int>();
         var experimental = new ArrayDictionary<int, int>();

         var random = new Random(5050);

         for (int i = 0; i < 100000; i++)
         {
            var key = random.Next(int.MinValue, int.MaxValue);
            var value = random.Next(int.MinValue, int.MaxValue);

            reference[key] = value;
            experimental[key] = value;
         }

         foreach (var kvp in experimental)
         {
            Assert.AreEqual(reference[kvp.Key], experimental[kvp.Key]);
         }
      }

      [TestMethod]
      public void TestTiming()
      {
         var reference = new Dictionary<int, int>();
         var experimental = new ArrayDictionary<int, int>(10000);

         var random = new Random(5050);

         long runningReferenceTime = 0;
         long runningExperimentalTime = 0;

         for (int i = 0; i < 100000; i++)
         {
            var key = random.Next(int.MinValue, int.MaxValue);
            var value = random.Next(int.MinValue, int.MaxValue);

            var referenceWatch = Stopwatch.StartNew();
            reference[key] = value;
            long refTime = referenceWatch.ElapsedTicks;

            var experimentalWatch = Stopwatch.StartNew();
            experimental[key] = value;
            long expTime = experimentalWatch.ElapsedTicks;

            runningReferenceTime += refTime;
            runningExperimentalTime += expTime;
         }

         double avgRefTime = runningReferenceTime / 100000.0;
         double avgExpTime = runningExperimentalTime / 100000.0;

         if (avgExpTime > avgRefTime)
         {
            Console.WriteLine("Wow!");
         }
      }
   }
}
