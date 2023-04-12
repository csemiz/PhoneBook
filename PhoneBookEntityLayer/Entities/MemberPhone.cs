using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.Entities
{
    [Table("MemberPhones")]
    public class MemberPhone:Base<int>
    {
        [Required]
        [StringLength(100,MinimumLength = 5)]
        public string FriendNameSurname { get; set; }
        
        public byte PhoneTypeId { get; set; }//Foreign Key

        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string Phone { get; set; }//+905345442053
        
        public string MemberId { get; set; }//Foreign Key


        [ForeignKey("PhoneTypeId")]
        public virtual PhoneType PhoneType { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
    }
}
