using System.ComponentModel.DataAnnotations;
using EntityLayer.Concrete;

namespace PolicyTracking.ViewModels
{
    public class PolicyTypeViewModel
    {
       public List<PolicyType> PolicyTypes {  get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = null!;

    }
}
