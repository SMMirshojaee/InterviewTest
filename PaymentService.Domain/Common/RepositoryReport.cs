namespace PaymentService.Domain.Common;

public class RepositoryReport
{
    public bool IsSuccessful { get; set; } = true;
    public Exception? Exception { get; set; }
}
public class RepositoryReport<T> : RepositoryReport
{
    public T? Output { get; set; }
}