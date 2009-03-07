﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmJoinedSubclassWriterTester
    {
        private RhinoAutoMocker<HbmJoinedSubclassWriter> _mocker;
        private HbmJoinedSubclassWriter _subclassWriter;

        [SetUp]
        public void SetUp()
        {
            _mocker = new RhinoAutoMocker<HbmJoinedSubclassWriter>();
            _subclassWriter = _mocker.ClassUnderTest;
        }

        [Test]
        public void Should_produce_valid_hbm()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping { Name = "joinedsubclass1", Key = new KeyMapping() };
            
            _mocker.Get<IHbmWriter<KeyMapping>>()
                .Expect(x => x.Write(joinedSubclassMapping.Key)).Return(new HbmKey());

            _subclassWriter.ShouldGenerateValidOutput(joinedSubclassMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Name, "mapping1").MapsToAttribute("name");

            testHelper.VerifyAll(_subclassWriter);
        }

        [Test]
        public void Should_write_the_key()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping { Key = new KeyMapping() };

            _mocker.Get<IHbmWriter<KeyMapping>>()
                .Expect(x => x.Write(joinedSubclassMapping.Key)).Return(new HbmKey());

            _subclassWriter.VerifyXml(joinedSubclassMapping)
                .Element("key").Exists();
        }

        [Test]
        public void Should_write_the_subclasses()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();
            joinedSubclassMapping.AddSubclass(new JoinedSubclassMapping());

            _subclassWriter.VerifyXml(joinedSubclassMapping)
                .Element("joined-subclass").Exists();
        }

        [Test]
        public void Should_write_multiple_nestings_of_subclasses()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();

            joinedSubclassMapping.AddSubclass(new JoinedSubclassMapping { Name = "Child" });
            joinedSubclassMapping.Subclasses.First().AddSubclass(new JoinedSubclassMapping { Name = "Grandchild" });

            _subclassWriter.VerifyXml(joinedSubclassMapping)
                .Element("joined-subclass").Exists().HasAttribute("name", "Child")
                .Element("joined-subclass").Exists().HasAttribute("name", "Grandchild");
        }

        [Test]
        public void Should_write_the_collections()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();
            joinedSubclassMapping.AddCollection(new BagMapping());

            _mocker.Get<IHbmWriter<ICollectionMapping>>()
                .Expect(x => x.Write(joinedSubclassMapping.Collections.First())).Return(new HbmBag());

            _subclassWriter.VerifyXml(joinedSubclassMapping)
                .Element("bag").Exists();   
        }

        [Test]
        public void Should_write_the_properties()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();
            joinedSubclassMapping.AddProperty(new PropertyMapping());

            _mocker.Get<IHbmWriter<PropertyMapping>>()
                .Expect(x => x.Write(joinedSubclassMapping.Properties.First())).Return(new HbmProperty());

            _subclassWriter.VerifyXml(joinedSubclassMapping)
                .Element("property").Exists();
        }

        [Test]
        public void Should_write_the_references()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();            
            joinedSubclassMapping.AddReference(new ManyToOneMapping());

            _mocker.Get<IHbmWriter<ManyToOneMapping>>()
                .Expect(x => x.Write(joinedSubclassMapping.References.First())).Return(new HbmManyToOne());

            _subclassWriter.VerifyXml(joinedSubclassMapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void Should_write_the_components()
        {
            var classMapping = new JoinedSubclassMapping();
            classMapping.AddComponent(new ComponentMapping());

            _mocker.Get<IHbmWriter<ComponentMapping>>()
                .Expect(x => x.Write(classMapping.Components.First())).Return(new HbmComponent());

            _subclassWriter.VerifyXml(classMapping)
                .Element("component").Exists();
        }


    }
}
