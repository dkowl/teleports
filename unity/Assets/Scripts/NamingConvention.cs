using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamingConvention
{
    public abstract class NamingConvention : SomeBaseClass
    {
        public static readonly int ConstantVariable;

        [SerializeField] protected int inspectorVariable;
        [SerializeField] private int anotherInspectorVariable;

        protected int protectedVariable;
        private int privateVariable;

        public NamingConvention(int privateVariable)
        {
            this.privateVariable = privateVariable;
        }
        ~NamingConvention() { }

        public void UnityEventMethod() { }

        protected override void OverridenMethod() { }

        public abstract void AbstractMethod();
        protected abstract void AnotherAbstractMethod();

        public virtual void VirtualMethod() { }
        protected virtual void AnotherVirtualMethod() { }

        public static void PublicStaticMethod() { }
        public void PublicMethod() { }
        protected void ProtectedMethod() { }
        private void PrivateMethod() { }
        private static void PrivateStaticMethod() { }

        public int PublicProperty
        {
            get { return inspectorVariable; }
        }

        protected int ProtectedProperty
        {
            get { return anotherInspectorVariable; }
        }

        private int PrivateProperty
        {
            get { return privateVariable; }
        }

        public class NestedClass
        {
        }

        public struct NestedStruct
        {
        }

        public enum EnumName
        {
            EnumVariable
        }
    }

    public class SomeBaseClass
    {
        protected virtual void OverridenMethod() { }
    }
}
