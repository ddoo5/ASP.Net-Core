using System;
using RichModelRelease.Billl.IRepositories;
using RichModelRelease.Billl.Models;

namespace RichModelRelease.Billl.Repositories
{
    public class BillRepository : IBillRepository
    {
        private Bill _bill;



        public BillRepository(Bill bill2)
        {
            this._bill = bill2;
        }



        public void Create(int number)
        {
            _bill = new(number, 0);
        }


        public void IncludeSheetsPlus(int sum)
        {
            _bill.PlusSum(sum);
        }


        public void IncludeSheetsMinus(int sum)
        {
            _bill.MinusSum(sum);
        }
    }
}

