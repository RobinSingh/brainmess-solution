using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrainmessInterpreter;

namespace BrainMessTest
{
    [TestClass]
    public class BrainmessUnitTest
    {
        [TestMethod]
        public void TestIncrementMemoryAddress()
        {
            var objBrainmessInterpreter = new Program();
            objBrainmessInterpreter.IncrementMemoryAddress();
            Assert.AreEqual(1, objBrainmessInterpreter.ptrMemory);
        }

        [TestMethod]
        public void TestDecrementMemoryAddress()
        {
            var objBrainmessInterpreter = new Program();
            objBrainmessInterpreter.DecrementMemoryAddress();
            Assert.AreEqual(32766, objBrainmessInterpreter.ptrMemory);
        }

        [TestMethod]
        public void TestIncrementMemoryValue()
        {
            var objBrainmessInterpreter = new Program
                {
                    tapememory = new short[Constant.MemoryLength]
                };
            objBrainmessInterpreter.IncrementMemoryValue();
            Assert.AreEqual(1, objBrainmessInterpreter.tapememory[objBrainmessInterpreter.ptrMemory]);
        }

        [TestMethod]
        public void TestDecrementMemoryValue()
        {
            var objBrainmessInterpreter = new Program
                {
                    tapememory = new short[Constant.MemoryLength]
                };
            objBrainmessInterpreter.DecrementMemoryValue();
            Assert.AreEqual(-1, objBrainmessInterpreter.tapememory[objBrainmessInterpreter.ptrMemory]);
        }

        [TestMethod]
        public void TestShowOutput()
        {
            var objBrainmessInterpreter = new Program
            {
                tapememory = new short[Constant.MemoryLength]
            };
            objBrainmessInterpreter.tapememory[0] = Convert.ToInt16(82);
            objBrainmessInterpreter.ShowOutput();
            Assert.AreEqual("R", objBrainmessInterpreter.output);
        }
    }
}
