using System.Linq;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ClassMapping : ClassMappingBase
    {
        private readonly AttributeStore<ClassMapping> _attributes;
        private readonly IList<ISubclassMapping> _subclasses;
        private DiscriminatorMapping _discriminator;
        public IIdentityMapping Id { get; set; }

        public ClassMapping()
            : this(new AttributeStore())
        { }

        protected ClassMapping(AttributeStore store)
            : base(store)
        {
            _attributes = new AttributeStore<ClassMapping>(store);
            _subclasses = new List<ISubclassMapping>();
        }

        public DiscriminatorMapping Discriminator
        {
            get { return _discriminator; }
            set
            {
                if (_discriminator != null)
                    _discriminator.ParentClass = null;

                _discriminator = value;

                if (_discriminator != null)
                    _discriminator.ParentClass = this;
            }
        }

        public IEnumerable<ISubclassMapping> Subclasses
        {
            get { return _subclasses; }
        }

        public void AddSubclass(ISubclassMapping subclass)
        {
            _subclasses.Add(subclass);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessClass(this);            

            if (Id != null)
                visitor.Visit(Id);

            if (Discriminator != null)
                visitor.Visit(Discriminator);

            foreach (var subclass in Subclasses)
                visitor.Visit(subclass);

            base.AcceptVisitor(visitor);
        }

        public string TableName
        {
            get { return _attributes.Get(x => x.TableName); }
            set { _attributes.Set(x => x.TableName, value); }
        }

        public AttributeStore<ClassMapping> Attributes
        {
            get { return _attributes; }
        }

        
    }
}