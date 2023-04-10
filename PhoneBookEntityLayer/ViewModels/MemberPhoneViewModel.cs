using PhoneBookEntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.ViewModels
{
    public class MemberPhoneViewModel// MemberPhoneDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRemoved { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string FriendNameSurname { get; set; }
        public byte PhoneTypeId { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string Phone { get; set; } //+905396796650
        public string MemberId { get; set; }
        public PhoneType? PhoneType { get; set; }
        public Member? Member { get; set; }

    }
}
