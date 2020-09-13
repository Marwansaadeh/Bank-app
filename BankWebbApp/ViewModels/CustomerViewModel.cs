using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankWebbApp.ViewModels
{
    public class CustomerViewModel
    {
       
        public List<Items> Items { get; set; }
        public PagingViewModel PagingViewModel { get; set; } = new PagingViewModel();
    }

    public class Items
    {
        public int CustomerId { get; set; }
        public string NationalId { get; set; }

        public string Givenname { get; set; }

        public string Streetaddress { get; set; }

        public string City { get; set; }
    }

    public class PagingViewModel
    {

        public IEnumerable<string> GetPages
        {
            get
            {
                int delta = 2;
                int left = CurrentPage - delta;
                int right = CurrentPage + delta + 1;

                var range = new List<string>();
                for (int i = 1; i <= MaxPages; i++)
                    if (i == 1 || i == MaxPages || (i >= left && i < right))
                        range.Add(i.ToString());

                var rangeIncludingDots = new List<string>();
                int l = 0;
                foreach (var i in range.Select(r => Convert.ToInt32(r)))
                {
                    if (l > 0)
                    {
                        if (i - l == 2)
                            rangeIncludingDots.Add((l + 1).ToString());
                        else if (i - l != 1)
                            rangeIncludingDots.Add("...");
                    }

                    rangeIncludingDots.Add(i.ToString());
                    l = i;
                }

                return rangeIncludingDots;
            }
        }

        public const int PageSize = 50;
       
        public int CurrentPage { get; set; }
        public int MaxPages { get; set; }

        public bool ShowPrevButton
        {
            get { return CurrentPage > 1; }
        }

      
        public bool ShowNextButton
        {
            get { return CurrentPage < MaxPages; }
        }
    }
}
