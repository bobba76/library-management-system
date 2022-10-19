using AutoMapper;

namespace Library.Application.Common;

public interface IMapFrom<T>
{
    /// <summary>
    ///     Create a Map from one type to another with AutoMapper
    /// </summary>
    /// <param name="profile"></param>
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}