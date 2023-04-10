using AutoMapper;
using PhoneBookEntityLayer.Entities;
using PhoneBookEntityLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.Mappings
{
    public class Maps:Profile
    {
        //Kim kime donussun?
        public Maps()
        {
            CreateMap<Member, MemberViewModel>(); 
            CreateMap<MemberViewModel,Member>();

            CreateMap<PhoneType, PhoneTypeViewModel>().ReverseMap();
            CreateMap<MemberPhone, MemberPhoneViewModel>().ReverseMap();


        }
    }
}
