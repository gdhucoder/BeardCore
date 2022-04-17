using BeardCore.Commons.Repository;
using SqlSugar;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace BeardCore.Service.Service
{
    public class Service
    {
    }

    public class PersonService : Repository<Person>
    {

        // 查询所有人
        public List<Person> GetPersons()
        {
            return base.GetList(); //使用自已的仓储方法
        }

        // 查询账户
        public List<Account> GetAccount()
        {
            var accountDb = base.Change<Account>();//切换仓仓（新功能）
            return accountDb.GetList();
        }

        //分页
        public List<Person> GetOrderPage(Expression<Func<Person, bool>> where, int pagesize, int pageindex)
        {
            return base.GetPageList(where, new SqlSugar.PageModel() { PageIndex = pageindex, PageSize = pagesize }); //使用自已的仓储方法
        }

    }

    public class Order
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OrderItem
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string ItemName { get; set; }
    }

    [SugarTable("PERSON.PERSON")]
    public class Person
    {          
        // IsIdentity 自增列，插入时insert语句
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int personId { get; set; }
        public string sex { get; set; }
        public string name { get; set; }

        public string email { get; set; }
        public string phone { get; set; }
    }

    [SugarTable("OTHER.ACCOUNT")]
    public class Account
    {
        [SugarColumn(ColumnName = "ACCOUNT_ID")]
        public int accountId { get; set; }

        public decimal bal { get; set; }
    }

    [SugarTable("\"EnterpriseDB\".BIZ_BASE_INFO")]
    [Table(Name = "EnterpriseDB.BIZ_BASE_INFO")]
    public class BizBaseInfo
    {
        [Column(IsPrimary = true)]
        public string ID { get; set; }
        public string Mc { get; set; }

        public DateTime? LASTDATE { get; set; }
    }
}
