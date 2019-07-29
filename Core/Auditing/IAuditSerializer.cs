namespace Core.Auditing
{
    public interface IAuditSerializer
    {
        string Serialize(object obj);
    }
}