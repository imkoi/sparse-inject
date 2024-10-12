using System;
using System.Linq;
using System.Runtime.Serialization;
using SparseInject.Tests.TestSources;
using FluentAssertions;
using NUnit.Framework;

namespace SparseInject.Tests
{
    public class Tests
    {
        private ContainerBuilder _containerBuilder;
    
        [SetUp]
        public void Setup()
        {
            _containerBuilder = new ContainerBuilder();
        }

        [Test]
        public void ManyBindings()
        {
            ContainerBinder.BindDeps(_containerBuilder);

            var container = _containerBuilder.Build();
        
            var highestDependency = container.Resolve<Class0>();
        
            highestDependency.Should().NotBeNull();
        }

        [Test]
        public void ConstructorCheck()
        {
            var constructor = typeof(NoConstructorWithInitializedField).GetConstructors().First();

            var instance = constructor.Invoke(new object[0]);
        }

        public class NoConstructor
        {
            
        }
        
        public class NoConstructorWithInitializedField
        {
            private int[] _array = new int[10];
        }

        public class EmptyConstructor
        {
            public EmptyConstructor()
            {
                
            }
        }
        
        public class EmptyConstructorWithLogic
        {
            private int[] _array;
            
            public EmptyConstructorWithLogic()
            {
                _array = new int[10];
            }
        }
    }
}