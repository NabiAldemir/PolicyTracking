using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IPolicyTypeService : IGenericService<PolicyType>
    {
        void TUpdate(PolicyTypeUpdateDTO t);
    }
}
