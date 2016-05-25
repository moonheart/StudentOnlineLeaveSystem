using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LeaveSystem.Web.IDAL;

namespace LeaveSystem.Web.Models
{
    public abstract class Entity<TKey>
    {
        [Required]
        public virtual TKey Id { get; private set; }

    }

}