using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using EntityLayer.Models;

namespace BusinessLayer.Abstract
{
    public interface IAgencyService:IGenericService<Agency>
    {
        public void TUpdate(AgencyUpdateDTO t);
        public void AddAgencyWithAddress(AddAgencyWithAddress model);
        public void UpdateAgencyWithAddress(UpdateAgencyWithAddress model);
    }
}
