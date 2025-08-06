using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using EntityLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework
{
    public class EfAgencyRepository : GenericRepository<Agency, Context>, IAgencyDal
    {
        private readonly IMapper _mapper;
        public EfAgencyRepository(Context context, IMapper mapper)
        : base(context)
        {
            _mapper = mapper;
        }

        public void AddAgencyWithAddress(AddAgencyWithAddress model)
        {
            using var transaction = c.Database.BeginTransaction();

            try
            {
                //Acente Ekle
                var agency = _mapper.Map<Agency>(model);
                c.Agencies.Add(agency);
                agency.Status = true;
                c.SaveChanges();

                //Adres Ekle
                var address = _mapper.Map<Address>(model);
                address.AgencyId = agency.Id;
                address.Status = true;
                c.Addresses.Add(address);
                c.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            

        }

        public void UpdateAgencyWithAddress(UpdateAgencyWithAddress model)
        {
            using var transaction = c.Database.BeginTransaction();

            try
            {
                var agency = c.Agencies.Include(x=>x.Address).FirstOrDefault(x => x.Id == model.Id);

                if (agency is null)
                    throw new Exception("Acente Bulunamadı!");

                _mapper.Map(model, agency);
                c.Agencies.Update(agency);

                var address = c.Addresses.FirstOrDefault(x => x.AgencyId == model.Id);

                _mapper.Map(model, address);
                c.Addresses.Update(address);

                c.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            
            
        }
    }
}
