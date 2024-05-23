namespace Sales.Data.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Entity<T> : IEntity<T> where T : struct
    {
       
        public T Id { get; set; }
    }
}
