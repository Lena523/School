namespace Sales.Data.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T> where T : struct
    {
        
        public T Id { get; set; }
    }
}
