// See https://aka.ms/new-console-template for more information
using BeardCore.Commons.Repository;
using BeardCore.Service.Service;
using SqlSugar;

IFreeSql freeSql = new FreeSql.FreeSqlBuilder()
    .UseConnectionString(FreeSql.DataType.Dameng, @"")
    .Build();
freeSql.Aop.CurdAfter += (s, e) =>
{
    Console.WriteLine($"{e.EntityType.FullName}{e.Sql}");
};
PersonService personService = new PersonService();
var hgd = new Person()
{
    sex = "M",
    name = "胡国栋"
};

personService.Insert(hgd);


var ps = personService.GetPersons();
foreach (var p in ps)
{
    Console.WriteLine(p.name);
}

var accounts = personService.GetAccount();
foreach (var acc in accounts)
{ 
    Console.WriteLine("id: {0}, bal: {1}", acc.accountId, acc.bal);
}

int total = 0;
ps = personService.AsQueryable().ToPageList(1, 10, ref total);


foreach (var p in ps)
{
    Console.WriteLine(p.name);
}

// personService.Update(new Person() { personId = 19, name = "胡国栋2", sex = "F"});

// var modifiedPerson = personService.GetById(19);

// Console.WriteLine(modifiedPerson.name);
// var delRes = personService.DeleteById(19);
// Console.WriteLine("是否删除成功？ {0}", delRes);

Console.WriteLine("total: {0}", total);

// var entService = new Repository<BizBaseInfo>();
// var ents = entService.AsQueryable().ToList();


Console.WriteLine("执行完成");

Console.WriteLine("执行DM FreeSQL=======================");

var ents = freeSql.Select<BizBaseInfo>().Where(a=>a.ID == "0000175D173945DAAAF87675E3D35661").ToList();

foreach (var e in ents)
{
    Console.WriteLine("{0}-{1}", e.ID, e.Mc);
}

Console.WriteLine("执行DM FreeSQL=======================");
Console.ReadKey();

