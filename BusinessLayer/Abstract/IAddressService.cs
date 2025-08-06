using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IAddressService:IGenericService<Address>
    {
        public List<Address> GetAllAddresses();
        public void TUpdate(AddressUpdateDTO t);
    }
}
