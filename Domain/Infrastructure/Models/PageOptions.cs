using System;

namespace AspNetFlex.Domain.Infrastructure.Models
{
    public class PageOptions
    {
        public const int Unlimited = int.MaxValue;
        
        private int _itemsPerPage = Unlimited;
        public int PageNumber { get; set; } = 1;

        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set => _itemsPerPage = Math.Max(1, value);
        }
    }
}