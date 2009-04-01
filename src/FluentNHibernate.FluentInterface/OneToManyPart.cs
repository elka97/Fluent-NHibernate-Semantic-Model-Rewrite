using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.FluentInterface
{
    public class OneToManyPart<PARENT, CHILD> : IDeferredCollectionMapping
    {
        private readonly MemberInfo _info;
        private readonly AttributeStore<ICollectionMapping> _attributes;

        private Func<ICollectionMapping> _collectionBuilder;

        public OneToManyPart(MemberInfo info)
        {
            _info = info;
            _attributes = new AttributeStore<ICollectionMapping>();
            AsBag();   
        }

        public OneToManyPart<PARENT, CHILD> AsBag()
        {
            _collectionBuilder = () => new BagMapping();
            return this;
        }

        public OneToManyPart<PARENT, CHILD> AsSet()
        {
            _collectionBuilder = () => new SetMapping();
            return this;
        }

        public OneToManyPart<PARENT, CHILD> IsInverse()
        {
            _attributes.Set(x => x.IsInverse, true);
            return this;
        }

        ICollectionMapping IDeferredCollectionMapping.ResolveCollectionMapping()
        {
            var collection = _collectionBuilder();       
            _attributes.CopyTo(collection.Attributes);

            collection.MemberInfo = _info;            
            collection.Key = new KeyMapping();
            collection.Contents = new OneToManyMapping {ChildType = typeof (CHILD)};

            return collection;
        }

    }
}