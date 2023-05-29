using AutoMapper;
using DSharpPlus.Entities;
using MySqlX.XDevAPI;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using TerritorialHQ.Models;

namespace TerritorialHQ.Mapping
{
    public class ContentPageProfile : Profile
    {
        public ContentPageProfile() : this("ContentPageProfile")
        {
        }

        protected ContentPageProfile(string profileName)
        : base(profileName)
        {
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Clans.CreateModel, Clan>().ReverseMap();
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Clans.EditModel, Clan>().ReverseMap();          
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Navigation.CreateModel, NavigationEntry>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Navigation.EditModel, NavigationEntry>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.ContentPages.CreateModel, ContentPage>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.ContentPages.EditModel, ContentPage>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Journal.CreateModel, JournalArticle>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Journal.EditModel, JournalArticle>().ReverseMap();                  
        }
    }
}
