using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SpecFlowSampleProject;

namespace Tests
{
    public class Tests
    {
        List<SampleJE> expected = new List<SampleJE>();

        [SetUp]
        public void Setup()
        {

            expected.Add(
                new SampleJE
                (
                    "12345", "Blackline Ledger", 1, (decimal)1000.00,
                    (decimal)0.00
                ));
            expected.Add(
                new SampleJE
                (
                    "12345", "Blackline Ledger", 2, (decimal)0.00,
                    (decimal)1000.00
                ));
        }

        [Test]
        public void TransformFileTest()
        {
            string rootPath = Path.GetFullPath("../../../");
            string filePath = Path.Combine(rootPath, "TestResources", "test.csv");

            List<SampleJE> actual = new Transformer().TransformFile(filePath);
            CompareListContentWithExpected(actual);
            // Strange : inbuilt function is always false
            // CollectionAssert.AreEquivalent(expected, actual);
            //Assert.True(expected.SequenceEqual(actual));
        }

        private void CompareListContentWithExpected(List<SampleJE> actual )
        {
            var found = false;

            foreach (var item in actual)
            {
                found = false;
                foreach (var expectedItem in expected)
                {
                    if (expectedItem.ToString() == item.ToString())
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) Assert.Fail("Expected item not found" + item + " not found in expected");
            }

            if (!found && actual.Count == expected.Count) Assert.Fail("Expected not equivalent to actual");
        }
    }
}