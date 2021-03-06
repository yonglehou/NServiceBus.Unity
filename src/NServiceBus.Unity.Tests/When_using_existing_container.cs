﻿namespace NServiceBus.Unity.Tests
{
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    [TestFixture]
    public class When_using_existing_container
    {
        [Test]
        public void Interfaces_registered_in_plain_container_are_resolvable_via_builder()
        {
            var container = new UnityContainer();
            container.RegisterType<ISomeInterface, SomeClass>();

            var builder = new UnityObjectBuilder(container);

            var result = builder.Build(typeof(ISomeInterface));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Interfaces_registered_in_the_container_after_wrapping_it_with_ObjectBuilder_are_resolvable_via_builder()
        {
            var container = new UnityContainer();

            var builder = new UnityObjectBuilder(container);

            container.RegisterType<ISomeInterface, SomeClass>();

            var result = builder.Build(typeof(ISomeInterface));

            Assert.IsInstanceOf<SomeClass>(result);
        }
        
        [Test]
        public void Abstract_classes_registered_in_plain_container_are_resolvable_via_builder()
        {
            var container = new UnityContainer();
            container.RegisterType<AbstractClass, SomeClass>();

            var builder = new UnityObjectBuilder(container);

            var result = builder.Build(typeof(AbstractClass));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Concrete_classes_registered_in_plain_container_are_resolvable_via_builder()
        {
            var container = new UnityContainer();
            container.RegisterType<SomeClass>();

            var builder = new UnityObjectBuilder(container);

            var result = builder.Build(typeof(SomeClass));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Existing_instances_registred_in_the_container_can_be_injected_via_property()
        {
            var container = new UnityContainer();

            var builder = new UnityObjectBuilder(container);
            builder.Configure(typeof(PropertyInjectionHandler), DependencyLifecycle.InstancePerCall);

            container.RegisterInstance<ISomeInterface>(new SomeClass());

            var result = (PropertyInjectionHandler)builder.Build(typeof(PropertyInjectionHandler));

            Assert.IsNotNull(result.Dependency);
        }
        
        [Test]
        public void Existing_instances_registred_in_the_container_can_be_injected_via_constructor()
        {
            var container = new UnityContainer();

            var builder = new UnityObjectBuilder(container);
            builder.Configure(typeof(ConstructorInjectionHandler), DependencyLifecycle.InstancePerCall);

            container.RegisterInstance<ISomeInterface>(new SomeClass());

            var result = (ConstructorInjectionHandler)builder.Build(typeof(ConstructorInjectionHandler));

            Assert.IsNotNull(result.Dependency);
        }

        class AbstractClass
        {
        }

        class SomeClass : AbstractClass, ISomeInterface
        {
        }

        class PropertyInjectionHandler : IHandleMessages<object>
        {
            public ISomeInterface Dependency { get; set; }

            public void Handle(object message)
            {
            }
        }
        
        class ConstructorInjectionHandler : IHandleMessages<object>
        {
            readonly ISomeInterface dependency;

            public ConstructorInjectionHandler(ISomeInterface dependency)
            {
                this.dependency = dependency;
            }

            public ISomeInterface Dependency
            {
                get { return dependency; }
            }

            public void Handle(object message)
            {
            }
        }

        interface ISomeInterface
        {
        }
    }
}