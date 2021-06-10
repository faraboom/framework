namespace Faraboom.Framework.Mapping
{
    public class Mapper : MapsterMapper.Mapper, IMapper
    {
        public Mapper()
            : base()
        {
        }

        public Mapper(TypeAdapterConfig config)
            : base(config)
        {
        }
    }
}
