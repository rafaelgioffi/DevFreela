namespace DevFreela.Core.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreateAt = DateTime.Now;
            IsDeleted = false;
        }
        public int Id { get; private set; }
        public DateTime CreateAt { get; private set; }
        public bool IsDeleted { get; private set; }

        public void SetAsDeleted() 
        { 
            IsDeleted = true;
        }
    }
}
