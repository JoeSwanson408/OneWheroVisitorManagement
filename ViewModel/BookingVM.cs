using OnewheroVisitorManagement.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OnewheroVisitorManagement.ViewModel
{
    public class BookingVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public BookingVM(string EventID)
        {
            _pageModel = new PageModel();
        }

    }
}
