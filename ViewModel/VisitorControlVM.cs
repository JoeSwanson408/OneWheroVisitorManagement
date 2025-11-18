using OnewheroVisitorManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnewheroVisitorManagement.ViewModel
{
    public class VisitorControlVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public VisitorControlVM()
        {
            _pageModel = new PageModel();
        }
    }
}
