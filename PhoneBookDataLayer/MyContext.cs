using Microsoft.EntityFrameworkCore;
using PhoneBookEntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookDataLayer
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {
            //ctor a parametre verdik 
            //Generic bir parametre verdik
            //Böylece Conncetionstring özelliğimizi Opsiyon olarak program.cs üzerinden ayarlayacagiz.

            
        }//ctor bitti


        //tablolarin Dbset propertylerini yazmamiz gerekiyor.

        public DbSet<Member> MemberTable { get; set; }
        public DbSet<MemberPhone> PhoneTable { get; set; }
        public DbSet<PhoneType> PhoneType { get; set; }

    }
}
