using AutoMapper;
using EntityLayer.Concrete;
using PolicyTracking.ViewModels;

namespace PolicyTrackingWebUI.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Customer,UpdateCustomerViewModel>()
                .ForMember(dest=>dest.Addresses,opt=>opt.Ignore())
                .ForMember(dest=>dest.Cities,opt=>opt.Ignore());
        }
    }
}
