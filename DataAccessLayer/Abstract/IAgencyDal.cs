using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using EntityLayer.Models;

namespace DataAccessLayer.Abstract
{
    public interface IAgencyDal:IGenericDal<Agency>
    {
        public void AddAgencyWithAddress(AddAgencyWithAddress model);
        public void UpdateAgencyWithAddress(UpdateAgencyWithAddress model);
    }
}
