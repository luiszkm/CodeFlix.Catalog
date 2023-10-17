
using Bogus;

namespace CodeFlix.Catalog.UnitTest.Common;
public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    public BaseFixture()
    => Faker = new Faker("pt_BR");

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;


}
