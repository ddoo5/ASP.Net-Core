using System;
namespace RichModelRelease.Billl.IRepositories
{
    public interface IBillRepository
    {
        void Create(int number);
        void IncludeSheetsPlus(int sum);
        void IncludeSheetsMinus(int sum);
    }
}

