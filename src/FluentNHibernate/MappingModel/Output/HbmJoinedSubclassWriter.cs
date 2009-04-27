using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmJoinedSubclassWriter : NullMappingModelVisitor, IHbmWriter<JoinedSubclassMapping>
    {
        private readonly IHbmWriter<KeyMapping> _keyWriter;
        private readonly HbmMappedMemberWriterHelper _mappedMemberHelper;

        private HbmJoinedSubclass _hbm;

        private HbmJoinedSubclassWriter(HbmMappedMemberWriterHelper helper, IHbmWriter<KeyMapping> keyWriter)
        {
            _mappedMemberHelper = helper;
            _keyWriter = keyWriter;
        }

        public HbmJoinedSubclassWriter(IHbmWriter<ICollectionMapping> collectionWriter, IHbmWriter<PropertyMapping> propertyWriter, IHbmWriter<ManyToOneMapping> manyToOneWriter, IHbmWriter<ComponentMapping> componentWriter, IHbmWriter<KeyMapping> keyWriter)
            : this(new HbmMappedMemberWriterHelper(collectionWriter, propertyWriter, manyToOneWriter, componentWriter), keyWriter)
        { }

        public object Write(JoinedSubclassMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            _hbm = new HbmJoinedSubclass();
            _hbm.name = subclassMapping.Name;

            if (subclassMapping.Attributes.IsSpecified(x => x.Proxy))
                _hbm.proxy = subclassMapping.Proxy.AssemblyQualifiedName;

            if (subclassMapping.Attributes.IsSpecified(x => x.Lazy))
            {
                _hbm.lazy = subclassMapping.Lazy;
                _hbm.lazySpecified = true;
            }

            if (subclassMapping.Attributes.IsSpecified(x => x.DynamicUpdate))
                _hbm.dynamicupdate = subclassMapping.DynamicUpdate;

            if (subclassMapping.Attributes.IsSpecified(x => x.DynamicInsert))
                _hbm.dynamicinsert = subclassMapping.DynamicInsert;

            if (subclassMapping.Attributes.IsSpecified(x => x.SelectBeforeUpdate))
                _hbm.selectbeforeupdate = subclassMapping.SelectBeforeUpdate;

            if (subclassMapping.Attributes.IsSpecified(x => x.Abstract))
            {
                _hbm.@abstract = subclassMapping.Abstract;
                _hbm.abstractSpecified = true;
            }
        }

        public override void Visit(KeyMapping keyMapping)
        {
            _hbm.key = (HbmKey)_keyWriter.Write(keyMapping);
        }

        public override void Visit(JoinedSubclassMapping subclassMapping)
        {
            var writer = new HbmJoinedSubclassWriter(_mappedMemberHelper,_keyWriter);
            var joinedSubclassHbm = (HbmJoinedSubclass)writer.Write(subclassMapping);
            joinedSubclassHbm.AddTo(ref _hbm.joinedsubclass1);
        }

        public override void Visit(ICollectionMapping collectionMapping)
        {
            object collectionHbm = _mappedMemberHelper.Write(collectionMapping);
            collectionHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            object propertyHbm = _mappedMemberHelper.Write(propertyMapping);
            propertyHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            object manyHbm = _mappedMemberHelper.Write(manyToOneMapping);
            manyHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(ComponentMapping componentMapping)
        {
            object componentHbm = _mappedMemberHelper.Write(componentMapping);
            componentHbm.AddTo(ref _hbm.Items);
        }
    }
}