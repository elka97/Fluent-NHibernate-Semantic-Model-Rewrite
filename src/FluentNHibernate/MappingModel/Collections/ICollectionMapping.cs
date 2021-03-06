using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase, INameable, IMapsMember
    {
        bool IsInverse { get; }
        bool IsLazy { get; }
        KeyMapping Key { get; set; }
        ICollectionContentsMapping Contents { get; set; }
        AttributeStore<ICollectionMapping> Attributes { get; }
        CollectionCascadeType Cascade { get; }
    }
}