using AutoMapper;
using EntityLayer.Concrete;
using EntityLayer.Models;
using PolicyTracking.Models;

namespace EntityLayer.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            // sourc, dest ()
            CreateMap<PolicyTypeUpdateDTO, PolicyType>();

            CreateMap<UserUpdateDTO, AppUser>()
           .ForMember(dest => dest.Id, opt => opt.Ignore()) // 🔴 Id kesinlikle ignore edilmeli!
           .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // şifre elle set edilecek
           .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
           .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<CustomerUpdateDTO, Customer>();

            CreateMap<VehicleUpdateDTO, Vehicle>();

            CreateMap<HousingUpdateDTO, Housing>();

            CreateMap<AgencyUpdateDTO, Agency>();

            CreateMap<AddressUpdateDTO, Address>()
                .ReverseMap(); // AddressUpdateDTO'dan Address'e ve tam tersi dönüşüm

            CreateMap<UpdatePolicyModel, Policy>()

            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id güncellenmesin

            CreateMap<UpdatePolicyModel, Vehicle>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdatePolicyModel, Housing>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserSignUpViewModel, AppUser>();

            CreateMap<UpdateCustomerModel, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore());


            CreateMap<CreateCustomerModel, Customer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Addresses))
                .ForMember(dest => dest.Address, opt => opt.Ignore());


            CreateMap<UpdateCustomerModel, Address>();
            CreateMap<CreateCustomerModel, Address>();

            CreateMap<AddAgencyWithAddress, Agency>()
                .ForMember(dest => dest.Address, opt => opt.Ignore());


            CreateMap<AddAgencyWithAddress, Address>()
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<UpdateAgencyWithAddress, Agency>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<UpdateAgencyWithAddress, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<CreatePolicyModel, Vehicle>()
                .ForMember(dest => dest.Description, opt => opt.Ignore());

            CreateMap<CreatePolicyModel, Housing>()
                .ForMember(dest => dest.Description, opt => opt.Ignore());

            CreateMap<CreatePolicyModel, Policy>();

            CreateMap<UpdatePolicyModel, Vehicle>()
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<UpdatePolicyModel, Housing>()
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<UpdatePolicyModel, Policy>()
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore())
                .ForMember(dest => dest.HousingId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());






        }
    }
}
