using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmManyToOneWriter : NullMappingModelVisitor, IHbmWriter<ManyToOneMapping>
    {
        private HbmManyToOne _hbm;

        public object Write(ManyToOneMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            _hbm = new HbmManyToOne();
            _hbm.name = manyToOneMapping.Name;

            if(manyToOneMapping.Attributes.IsSpecified(x => x.IsNotNullable))
            {
                _hbm.SetNotNull(manyToOneMapping.IsNotNullable);
            }
        }
    }
}