using Minify.Core.Controllers;
using Minify.Core.Managers;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Minify.Core.Test
{
    public class ControllerManagerTest
    {
        [Test]
        public void Initialize_Successfull()
        {
            ControllerManager.Initialize();
            Assert.IsNotNull(ControllerManager.Controllers);
        }

        [Test]
        public void Get_Without_Initialize_Return_Null()
        {
            var a = ControllerManager.Get<HitlistController>();
            Assert.IsNull(a);
            ControllerManager.Terminate();
        }

        [Test]
        public void Get_Return_Null()
        {
            ControllerManager.Initialize();
            var a = ControllerManager.Get<HitlistController>();
            Assert.IsNull(a);
            ControllerManager.Terminate();
        }

        [Test]
        public void Get_Return_Not_Null()
        {
            ControllerManager.Initialize();
            ControllerManager.Add(new HitlistController());
            Assert.IsNotNull(ControllerManager.Get<HitlistController>());
            ControllerManager.Terminate();
        }

        [Test]
        public void Add_Without_Initialize_Return_False()
        {
            var a = ControllerManager.Add(new HitlistController());
            Assert.IsFalse(a);
            ControllerManager.Terminate();
        }

        [Test]
        public void Add_Null_Return_False()
        {
            ControllerManager.Initialize();
            var a = ControllerManager.Add(null);
            Assert.IsFalse(a);
            ControllerManager.Terminate();
        }

        [Test]
        public void Add_Return_True()
        {
            ControllerManager.Initialize();            
            Assert.IsTrue(ControllerManager.Add(new HitlistController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void AddRange_Without_Initialize_Return_False()
        {
            Assert.IsFalse(ControllerManager.AddRange(new HitlistController(), new StreamroomController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void AddRange_Null_Return_False()
        {
            ControllerManager.Initialize();
            Assert.IsFalse(ControllerManager.AddRange(null, new StreamroomController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void AddRange_Return_True()
        {
            ControllerManager.Initialize();
            Assert.IsTrue(ControllerManager.AddRange(new HitlistController(), new StreamroomController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void Remove_Without_Initialize_Return_False()
        {
            Assert.IsFalse(ControllerManager.Remove(new HitlistController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void Remove_Return_False()
        {
            ControllerManager.Initialize();
            Assert.IsFalse(ControllerManager.Remove(new HitlistController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void Remove_Null_Return_False()
        {
            ControllerManager.Initialize();
            Assert.IsFalse(ControllerManager.Remove(null));
            ControllerManager.Terminate();
        }

        [Test]
        public void RemoveRange_Without_Initialize_Return_False()
        {
            Assert.IsFalse(ControllerManager.RemoveRange(new HitlistController(), new HitlistController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void RemoveRange_Return_False()
        {
            ControllerManager.Initialize();
            Assert.IsFalse(ControllerManager.RemoveRange(new HitlistController(), new StreamroomController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void RemoveRange_Null_Return_False()
        {
            ControllerManager.Initialize();
            Assert.IsFalse(ControllerManager.RemoveRange(null));
            ControllerManager.Terminate();
        }

        [Test]
        public void Remove_Return_True()
        {
            ControllerManager.Initialize();
            ControllerManager.Add(new HitlistController());
            Assert.IsTrue(ControllerManager.Remove(new HitlistController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void RemoveRange_Return_True()
        {
            ControllerManager.Initialize();
            ControllerManager.AddRange(new HitlistController(), new StreamroomController());
            Assert.IsTrue(ControllerManager.RemoveRange(new HitlistController(), new StreamroomController()));
            ControllerManager.Terminate();
        }

        [Test]
        public void Clear_Without_Initialize_Fail()
        {
            ControllerManager.Clear();
            Assert.IsNull(ControllerManager.Controllers);
            ControllerManager.Terminate();
        }


        [Test]
        public void Clear_Success()
        {
            ControllerManager.Initialize();
            ControllerManager.AddRange(new HitlistController(), new StreamroomController());
            ControllerManager.Clear();
            Assert.AreEqual(0, ControllerManager.Controllers.Count);
            ControllerManager.Terminate();
        }


        [Test]
        public void Terminate_Success()
        {
            ControllerManager.Initialize();
            ControllerManager.AddRange(new HitlistController(), new StreamroomController());
            ControllerManager.Terminate();
            Assert.IsNull(ControllerManager.Controllers);
        }

    }
}
