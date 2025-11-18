using OnewheroVisitorManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnewheroVisitorManagement.ViewModel
{
    public class AdminEventsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public AdminEventsVM()
        {
            _pageModel = new PageModel();
        }
    }
}
