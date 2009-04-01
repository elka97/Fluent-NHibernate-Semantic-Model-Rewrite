﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class ComponentMapping : MapsMemberBase, IHasMappedMembers
    {
        private readonly AttributeStore<ComponentMapping> _attributes;
        private readonly MappedMembers _mappedMembers;
        public Type ComponentType { get; set; }

        public ComponentMapping()
            : this(new AttributeStore())
        { }

        protected ComponentMapping(AttributeStore store) : base(store)
        {
            _attributes = new AttributeStore<ComponentMapping>(store);
            _mappedMembers = new MappedMembers();
        }

        public AttributeStore<ComponentMapping> Attributes
        {
            get { return _attributes; }
        }

        public string PropertyName
        {
            get { return _attributes.Get(x => x.PropertyName); }
            set { _attributes.Set(x => x.PropertyName, value); }
        }

        public string ClassName
        {
            get { return _attributes.Get(x => x.ClassName); }
            set { _attributes.Set(x => x.ClassName, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);            
            _mappedMembers.AcceptVisitor(visitor);
        }

        #region IHasMappedMembers

        public void AddProperty(PropertyMapping property)
        {
            _mappedMembers.AddProperty(property);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            _mappedMembers.AddCollection(collection);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            _mappedMembers.AddReference(manyToOne);
        }

        public void AddComponent(ComponentMapping component)
        {
            _mappedMembers.AddComponent(component);
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return _mappedMembers.References; }
        }

        public IEnumerable<ICollectionMapping> Collections
        {
            get { return _mappedMembers.Collections; }
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return _mappedMembers.Properties; }
        }

        public IEnumerable<ComponentMapping> Components
        {
            get { return _mappedMembers.Components; }
        }

        #endregion
    }
}
