using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ratcow.ObjectDumper.Tests
{
    [TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public void BasicTest()
        {
            var dumpedValue = default(string);
            var dumpValue = "A basic test";
            var expectedResult = $"BasicTest -> {dumpValue}";

            void dumpFunction(object o)
            {
                dumpedValue = o.ToString();
            }

            ObjectDumperHelper.AddOutputHandler(dumpFunction);

            dumpValue.DumpObject();

            ObjectDumperHelper.RemoveOutputHandler(dumpFunction);

            Assert.IsNotNull(dumpedValue);
            Assert.AreEqual(expectedResult, dumpedValue);
        }

        /// <summary>
        /// Test to check we can dump the raw value
        /// </summary>
        [TestMethod]
        public void BasicRawTest()
        {
            var dumpedValue = default(string);
            var dumpValue = "A basic test";
            var expectedResult = $"BasicRawTest ->";
            var invocations = 0;

            void dumpFunction(object o)
            {
                dumpedValue = o.ToString();
                Assert.IsNotNull(dumpedValue);
                Assert.AreEqual(invocations++ == 0 ? expectedResult: dumpValue, dumpedValue);
            }

            ObjectDumperHelper.AddOutputHandler(dumpFunction);
            ObjectDumperHelper.UseRawValue = true;
            ObjectDumperHelper.RemoveOutputHandler(dumpFunction);
            ObjectDumperHelper.UseRawValue = false;

            dumpValue.DumpObject();            
        }

        /// <summary>
        /// Test to check we can dump the raw value, and exclude the callerName
        /// </summary>
        [TestMethod]
        public void BasicRawNoCallerNameTest()
        {
            var dumpedValue = default(string);
            var dumpValue = "A basic test";
            var expectedResult = $"BasicRawTest ->";
            var invocations = 0;

            void dumpFunction(object o)
            {
                dumpedValue = o.ToString();
                Assert.IsNotNull(dumpedValue);
                Assert.AreEqual(0, invocations++);
                Assert.AreEqual(dumpValue, dumpedValue);
            }

            ObjectDumperHelper.AddOutputHandler(dumpFunction);
            ObjectDumperHelper.UseRawValue = true;
            ObjectDumperHelper.RemoveOutputHandler(dumpFunction);
            ObjectDumperHelper.UseRawValue = false;

            dumpValue.DumpObject(null);
        }

        /// <summary>
        /// Tests to verify that bytes are dumped
        /// </summary>
        [TestMethod]
        public void DumpBytesTest()
        {
            var dumpedValue = default(string);
            var hexValue = "010507090A0B0C";
            var expectedResult = $"DumpBytesTest -> {hexValue}";
            var dumpValue = new byte[] { 0x01, 0x05, 0x07, 0x09, 0x0A, 0x0B, 0x0C};

            void dumpFunction(object o)
            {
                dumpedValue = o.ToString();
            }

            ObjectDumperHelper.AddOutputHandler(dumpFunction);

            dumpValue.DumpBytes();

            ObjectDumperHelper.RemoveOutputHandler(dumpFunction);

            Assert.IsNotNull(dumpedValue);
            Assert.AreEqual(expectedResult, dumpedValue);
        }
    }
}
