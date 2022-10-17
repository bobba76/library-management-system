using AutoMapper;
using Library.Api;
using Library.Api.Mapping;
using Xunit;

namespace Library.Domain.Tests.Mappings;

public class MappingTests
{
    private readonly MapperConfiguration _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    /// <summary>
    /// Detta test ser till att alla properties på vymodeller har en mappning om man använder IMapFrom-interfacet
    /// </summary>
    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }
}