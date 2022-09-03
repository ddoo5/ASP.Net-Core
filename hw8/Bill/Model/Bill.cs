using System;
namespace RichModelRelease.Billl.Models
{
    public sealed class Bill
    {
        public Bill(int number, int sum)
        {
            BillNumber = number;
            Sum = sum;

            Validate();
        }


        public int BillNumber { get; private set; }

        public int Sum { get; private set; }

        // я так понял, что записи - это операции со счетом, поэтому эти методы будут
        //  IncludeSheets
        public void PlusSum(int sum) => Sum += sum;

        public void MinusSum(int minus) => Sum -= minus;


        public void Validate()
        {
            if (BillNumber < 0)
                throw new Exception("Bill number can't be less than zero");

            if (BillNumber == 0)
                throw new Exception("Bill number can't be zero");

            if (Sum < 0)
                throw new Exception("Sum can't be less than zero");
        }
    }
}

