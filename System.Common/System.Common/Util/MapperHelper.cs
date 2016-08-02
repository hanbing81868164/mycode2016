using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class MapperHelper
    {


        public static void AddGlobalIgnore(string startingwith)
        {
            Mapper.AddGlobalIgnore(startingwith);
        }


        public static void AddProfile<TProfile>() where TProfile : Profile, new()
        {
            Mapper.AddProfile<TProfile>();
        }


        public static void AddProfile(Profile profile)
        {
            Mapper.AddProfile(profile);
        }


        public static void AssertConfigurationIsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }


        public static void AssertConfigurationIsValid<TProfile>() where TProfile : Profile, new()
        {
            Mapper.AssertConfigurationIsValid<TProfile>();
        }


        public static void AssertConfigurationIsValid(string profileName)
        {
            Mapper.AssertConfigurationIsValid(profileName);
        }


        public static void AssertConfigurationIsValid(TypeMap typeMap)
        {
            Mapper.AssertConfigurationIsValid(typeMap);
        }


        public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return Mapper.CreateMap<TSource, TDestination>();
        }


        public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(MemberList memberList)
        {
            return Mapper.CreateMap<TSource, TDestination>(memberList);
        }


        public static IMappingExpression CreateMap(Type sourceType, Type destinationType)
        {
            return Mapper.CreateMap(sourceType, destinationType);
        }


        public static IMappingExpression CreateMap(Type sourceType, Type destinationType, MemberList memberList)
        {
            return Mapper.CreateMap(sourceType, destinationType, memberList);
        }


        public static IProfileExpression CreateProfile(string profileName)
        {
            return Mapper.CreateProfile(profileName);
        }


        public static void CreateProfile(string profileName, Action<IProfileExpression> profileConfiguration)
        {
            Mapper.CreateProfile(profileName, profileConfiguration);
        }


        public static TDestination DynamicMap<TDestination>(object source)
        {
            return Mapper.DynamicMap<TDestination>(source);
        }


        public static TDestination DynamicMap<TSource, TDestination>(TSource source)
        {
            return Mapper.DynamicMap<TSource, TDestination>(source);
        }


        public static void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper.DynamicMap<TSource, TDestination>(source, destination);
        }


        public static object DynamicMap(object source, Type sourceType, Type destinationType)
        {
            return Mapper.DynamicMap(source, sourceType, destinationType);
        }


        public static void DynamicMap(object source, object destination, Type sourceType, Type destinationType)
        {
            Mapper.DynamicMap(source, destination, sourceType, destinationType);
        }


        public static TypeMap FindTypeMapFor<TSource, TDestination>()
        {
            return Mapper.FindTypeMapFor<TSource, TDestination>();
        }


        public static TypeMap FindTypeMapFor(Type sourceType, Type destinationType)
        {
            return Mapper.FindTypeMapFor(sourceType, destinationType);
        }


        public static TypeMap[] GetAllTypeMaps()
        {
            return Mapper.GetAllTypeMaps();
        }



        public static void Initialize(Action<IConfiguration> action)
        {
            Mapper.Initialize(action);
        }



        public static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }


        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }


        public static TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts)
        {
            return Mapper.Map<TDestination>(source, opts);
        }


        public static TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return Mapper.Map<TSource, TDestination>(source, opts);
        }


        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Mapper.Map<TSource, TDestination>(source, destination);
        }


        public static object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }


        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return Mapper.Map<TSource, TDestination>(source, destination, opts);
        }


        public static object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, destination, sourceType, destinationType);
        }


        public static object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            return Mapper.Map(source, sourceType, destinationType, opts);
        }


        public static object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            return Mapper.Map(source, destination, sourceType, destinationType, opts);
        }


        public static void Reset()
        {
            Mapper.Reset();
        }
    }
}
