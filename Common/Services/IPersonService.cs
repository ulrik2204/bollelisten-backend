using Data;

namespace Common.Services;

public interface IPersonService
{

}

public class PersonService(AppDbContext dbContext) : IPersonService
{

}

