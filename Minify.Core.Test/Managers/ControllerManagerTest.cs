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
            AppManager.Initialize();
            Assert.IsNotNull(AppManager.Controllers);
        }

        [Test]
        public void Get_Without_Initialize_Return_Null()
        {
            var a = AppManager.Get<HitlistController>();
            Assert.IsNull(a);
            AppManager.Terminate();
        }

        [Test]
        public void Get_Return_Null()
        {
            AppManager.Initialize();
            var a = AppManager.Get<HitlistController>();
            Assert.IsNull(a);
            AppManager.Terminate();
        }

        [Test]
        public void Get_Return_Not_Null()
        {
            AppManager.Initialize();
            AppManager.Add(new HitlistController());
            Assert.IsNotNull(AppManager.Get<HitlistController>());
            AppManager.Terminate();
        }

        [Test]
        public void Add_Without_Initialize_Return_False()
        {
            var a = AppManager.Add(new HitlistController());
            Assert.IsFalse(a);
            AppManager.Terminate();
        }

        [Test]
        public void Add_Null_Return_False()
        {
            AppManager.Initialize();
            var a = AppManager.Add(null);
            Assert.IsFalse(a);
            AppManager.Terminate();
        }

        [Test]
        public void Add_Return_True()
        {
            AppManager.Initialize();            
            Assert.IsTrue(AppManager.Add(new HitlistController()));
            AppManager.Terminate();
        }

        [Test]
        public void AddRange_Without_Initialize_Return_False()
        {
            Assert.IsFalse(AppManager.AddRange(new HitlistController(), new StreamroomController()));
            AppManager.Terminate();
        }

        [Test]
        public void AddRange_Null_Return_False()
        {
            AppManager.Initialize();
            Assert.IsFalse(AppManager.AddRange(null, new StreamroomController()));
            AppManager.Terminate();
        }

        [Test]
        public void AddRange_Return_True()
        {
            AppManager.Initialize();
            Assert.IsTrue(AppManager.AddRange(new HitlistController(), new StreamroomController()));
            AppManager.Terminate();
        }

        [Test]
        public void Remove_Without_Initialize_Return_False()
        {
            Assert.IsFalse(AppManager.Remove(new HitlistController()));
            AppManager.Terminate();
        }

        [Test]
        public void Remove_Return_False()
        {
            AppManager.Initialize();
            Assert.IsFalse(AppManager.Remove(new HitlistController()));
            AppManager.Terminate();
        }

        [Test]
        public void Remove_Null_Return_False()
        {
            AppManager.Initialize();
            Assert.IsFalse(AppManager.Remove(null));
            AppManager.Terminate();
        }

        [Test]
        public void RemoveRange_Without_Initialize_Return_False()
        {
            Assert.IsFalse(AppManager.RemoveRange(new HitlistController(), new HitlistController()));
            AppManager.Terminate();
        }

        [Test]
        public void RemoveRange_Return_False()
        {
            AppManager.Initialize();
            Assert.IsFalse(AppManager.RemoveRange(new HitlistController(), new StreamroomController()));
            AppManager.Terminate();
        }

        [Test]
        public void RemoveRange_Null_Return_False()
        {
            AppManager.Initialize();
            Assert.IsFalse(AppManager.RemoveRange(null));
            AppManager.Terminate();
        }

        [Test]
        public void Remove_Return_True()
        {
            AppManager.Initialize();
            AppManager.Add(new HitlistController());
            Assert.IsTrue(AppManager.Remove(new HitlistController()));
            AppManager.Terminate();
        }

        [Test]
        public void RemoveRange_Return_True()
        {
            AppManager.Initialize();
            AppManager.AddRange(new HitlistController(), new StreamroomController());
            Assert.IsTrue(AppManager.RemoveRange(new HitlistController(), new StreamroomController()));
            AppManager.Terminate();
        }

        [Test]
        public void Clear_Without_Initialize_Fail()
        {
            AppManager.Clear();
            Assert.IsNull(AppManager.Controllers);
            AppManager.Terminate();
        }


        [Test]
        public void Clear_Success()
        {
            AppManager.Initialize();
            AppManager.AddRange(new HitlistController(), new StreamroomController());
            AppManager.Clear();
            Assert.AreEqual(0, AppManager.Controllers.Count);
            AppManager.Terminate();
        }


        [Test]
        public void Terminate_Success()
        {
            AppManager.Initialize();
            AppManager.AddRange(new HitlistController(), new StreamroomController());
            AppManager.Terminate();
            Assert.IsNull(AppManager.Controllers);
        }

    }
}
