using RichModelRelease.Billl.IRepositories;
using RichModelRelease.Billl.Models;
using RichModelRelease.Billl.Repositories;



//for testing

Bill bill = new(1, 154);
BillRepository _repo = new(bill);

_repo.IncludeSheetsPlus(2599);
Console.WriteLine($"{bill.BillNumber} {bill.Sum}");

_repo.IncludeSheetsMinus(799);
Console.WriteLine($"{bill.BillNumber} {bill.Sum}");

Console.ReadKey();
