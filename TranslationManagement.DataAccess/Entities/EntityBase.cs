namespace DataAccess.Entities
{
    public class EntityBase<T> 
    {
        public T Id { get; set; }
    }
    
    public class EntityBase:EntityBase<int> {
    }
}
