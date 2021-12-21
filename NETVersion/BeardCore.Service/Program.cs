// See https://aka.ms/new-console-template for more information
using BeardCore.Service.Service;
using SqlSugar;

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

Console.WriteLine("total: {0}", total);

Console.WriteLine("执行完成");
Console.ReadKey();

