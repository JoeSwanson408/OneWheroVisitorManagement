using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnewheroVisitorManagement.Model;

namespace OnewheroVisitorManagement.ViewModel
{
    public class AboutVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public AboutVM()
        {
            _pageModel = new PageModel();
        }
    }
}
