namespace Api.Service.Repository.DbEntities
{
    public abstract class BaseEntity : IEntity
    {
        public string Id { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
    public interface IEntity
    {
        string Id { get; }
    }
}
