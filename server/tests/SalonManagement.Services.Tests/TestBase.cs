using AutoFixture;
using AutoMapper;
using AutoMapper.Internal;
using SalonManagement.Dal;
using SalonManagement.Api.Mappings;
using NSubstitute;

namespace SalonManagement.Services.Tests;

public abstract class TestBase
{
    protected readonly IFixture Fixture;
    protected readonly ISalonDbContext DbContext;
    protected readonly IMapper Mapper;

    protected TestBase()
    {
        Fixture = new Fixture();
        Fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        DbContext = Substitute.For<ISalonDbContext>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.Internal().AllowAdditiveTypeMapCreation = true;
            cfg.AddMaps(typeof(AppointmentMappingProfile).Assembly);
        });

        config.AssertConfigurationIsValid();
        Mapper = config.CreateMapper();
    }
}
