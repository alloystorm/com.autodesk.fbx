﻿// ***********************************************************************
// Copyright (c) 2017 Unity Technologies. All rights reserved.
//
// Licensed under the ##LICENSENAME##.
// See LICENSE.md file in the project root for full license information.
// ***********************************************************************

using NUnit.Framework;
using System.Collections;
using FbxSdk;

namespace UnitTests
{
    public abstract class FbxLayerElementArrayTestBase<T> : TestBase<T> where T : FbxSdk.FbxLayerElementArray
    {
        static System.Reflection.ConstructorInfo s_constructor;

        static FbxLayerElementArrayTestBase() {
            s_constructor = typeof(T).GetConstructor (new[] { typeof(EFbxType) });

            #if ENABLE_COVERAGE_TEST
            // Register the calls we make through reflection.

            if (s_constructor != null) {
                var constructor = typeof(FbxLayerElementArrayTestBase<T>).GetMethod("CreateObject", new System.Type[] { typeof(EFbxType) });
                CoverageTester.RegisterReflectionCall(constructor, s_constructor);
            }
            #endif
        }

        public T CreateObject (EFbxType type = EFbxType.eFbxBlob) {
            return Invoker.InvokeConstructor<T>(s_constructor, type);
        }

        [Test]
        public virtual void TestBasics()
        {
            T layerElementArray = CreateObject ();

            // Test SetCount()
            layerElementArray.SetCount (1);
            Assert.AreEqual (layerElementArray.GetCount (), 1);

            // test invalid
            layerElementArray.SetCount (-1);

            // Test AddInt()
            layerElementArray.Add (0);
            layerElementArray.Add (-1);

            // Test AddFbxColor()
            layerElementArray.Add (new FbxColor ());
            layerElementArray.Add (new FbxColor (1, 0, 0));

            // Test AddFbxVector2()
            layerElementArray.Add (new FbxVector2 ());
            layerElementArray.Add (new FbxVector2 (1, 0));

            // Test AddFbxVector4()
            layerElementArray.Add (new FbxVector4 ());
            layerElementArray.Add (new FbxVector4 (1, 0, 0));

            // Test SetAtInt()
            layerElementArray.SetAt (0, 1);

            // test invalid index
            layerElementArray.SetAt (-1, 1);

            // test negative int
            layerElementArray.SetAt (1, -1);

            // Test SetAtFbxColor()
            layerElementArray.SetAt (0, new FbxColor ());

            // test invalid index
            layerElementArray.SetAt (-1, new FbxColor ());

            // Test SetFbxVector2()
            layerElementArray.SetAt (0, new FbxVector2 ());

            // test invalid index
            layerElementArray.SetAt (-1, new FbxVector2 ());

            // Test SetAtFbxVector4()
            layerElementArray.SetAt (0, new FbxVector4 ());

            // test invalid index
            layerElementArray.SetAt (-1, new FbxVector4 ());
        }

        [Test]
        public void TestDispose()
        {
            var layerElementArray = new FbxLayerElementArray (EFbxType.eFbxBlob);
            layerElementArray.Dispose ();
            Assert.That (() => {
                layerElementArray.SetCount (1);
            }, Throws.Exception.TypeOf<System.NullReferenceException> ());

            FbxLayerElementArray elementArray;
            using (elementArray = new FbxLayerElementArray (EFbxType.eFbxBlob)) {}
            Assert.That (() => {
                elementArray.SetCount (1);
            }, Throws.Exception.TypeOf<System.NullReferenceException> ());
        }
    }

    public abstract class FbxLayerElementArrayTemplateTestBase<T,U> : FbxLayerElementArrayTestBase<T> where T : FbxSdk.FbxLayerElementArray {

        static System.Reflection.MethodInfo s_getAt;

        static FbxLayerElementArrayTemplateTestBase() {
            s_getAt = typeof(T).GetMethod("GetAt", new System.Type[] { typeof(int) });

            #if ENABLE_COVERAGE_TEST
            // Register the calls we make through reflection.
            if(s_getAt != null){
                var getAt = typeof(FbxLayerElementArrayTemplateTestBase<T,U>).GetMethod("GetAt");
                CoverageTester.RegisterReflectionCall(getAt, s_getAt);
            }
            #endif
        }

        public U GetAt(T layerElementArray, int index){
            return Invoker.Invoke<U> (s_getAt, layerElementArray, index);
        }

        [Test]
        public void TestGetAt()
        {
            var layerElementArrayTemplate = CreateObject ();

            // make sure doesn't crash
            GetAt (layerElementArrayTemplate, 0);
            GetAt (layerElementArrayTemplate, int.MinValue);
            GetAt (layerElementArrayTemplate, int.MaxValue);
        }
    }

    public class FbxLayerElementArrayTest : FbxLayerElementArrayTestBase<FbxLayerElementArray> {}

    public class FbxLayerElementArrayTemplateFbxColorTest : 
        FbxLayerElementArrayTemplateTestBase<FbxLayerElementArrayTemplateFbxColor,FbxColor> {}

    public class FbxLayerElementArrayTemplateFbxSurfaceMaterialTest :
        FbxLayerElementArrayTemplateTestBase<FbxLayerElementArrayTemplateFbxSurfaceMaterial,FbxSurfaceMaterial> {}

    [Ignore("Calling GetAt() causes a crash")]
    public class FbxLayerElementArrayTemplateFbxVector2Test : 
        FbxLayerElementArrayTemplateTestBase<FbxLayerElementArrayTemplateFbxVector2,FbxVector2> {}

    public class FbxLayerElementArrayTemplateFbxVector4Test : 
        FbxLayerElementArrayTemplateTestBase<FbxLayerElementArrayTemplateFbxVector4,FbxVector4> {}

    public class FbxLayerElementArrayTemplateIntTest : 
        FbxLayerElementArrayTemplateTestBase<FbxLayerElementArrayTemplateInt,int> {}
}
