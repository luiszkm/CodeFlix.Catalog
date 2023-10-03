using Bogus;

namespace CodeFlix.Catalog.IntegrationTests.Base;
public class BaseFixture
{
    public BaseFixture()
        => Faker = new Faker("pt_BR");


    protected Faker Faker { get; set; }

}
