using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mountain.Protocol.NBT;
using Mountain.Protocol.NBT.Data;
using System.Collections.Generic;

namespace MountainTest
{
    [TestClass]
    public class NBTTest
    {
        [TestMethod]
        public void FullNBTTest()
        {
            NBTCompound tag = NBTUtils.ReadNBT(@".\Resources\fulltest.nbt").ExpandRoot();
            Assert.AreEqual(11, tag.Count);
        }

        [TestMethod]
        public void NaNNBTTest()
        {
            NBTCompound tag = NBTUtils.ReadNBT(@".\Resources\nantest.nbt").ExpandRoot();
            Assert.AreEqual(12, tag.Count);

            IList<NBTTagDouble> pos = tag.GetList("Pos").CastTo<NBTTagDouble>();
            Assert.IsFalse(double.IsNaN(pos[0].GetValue()));
            Assert.IsTrue(double.IsNaN(pos[1].GetValue()));
            Assert.IsFalse(double.IsNaN(pos[2].GetValue()));
        }
    }
}
