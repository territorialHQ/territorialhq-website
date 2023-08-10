using AutoMapper;
using DSharpPlus.Entities;
using MySqlX.XDevAPI;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using TerritorialHQ.Models;
using TerritorialHQ_Library.Entities;
using TerritorialHQ_Library.DTO;

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
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Clans.CreateModel, DTOClan>().ReverseMap();
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Clans.EditModel, DTOClan>().ReverseMap();          
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Navigation.CreateModel, DTONavigationEntry>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Navigation.EditModel, DTONavigationEntry>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.ContentPages.CreateModel, DTOContentPage>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.ContentPages.EditModel, DTOContentPage>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Journal.CreateModel, DTOJournalArticle>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.Journal.EditModel, DTOJournalArticle>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.ContentCreators.CreateModel, DTOContentCreator>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.ContentCreators.EditModel, DTOContentCreator>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.CommunityEvents.CreateModel, DTOCommunityEvent>().ReverseMap();                  
            CreateMap<TerritorialHQ.Areas.Administration.Pages.CommunityEvents.EditModel, DTOCommunityEvent>().ReverseMap();                  
        }
    }
}
