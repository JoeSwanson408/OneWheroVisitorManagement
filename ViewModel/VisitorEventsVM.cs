using OnewheroVisitorManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnewheroVisitorManagement.ViewModel
{
    public class VisitorEventsVM : Utilities.ViewModelBase
    {
        
        private readonly PageModel _pageModel;
        public ObservableCollection<Event> EventsCollection { get; set; }
        public VisitorEventsVM()
        {
            _pageModel = new PageModel();
            
            EventsCollection = new ObservableCollection<Event>(); // Initialize before the loop
            
            foreach (Event ev in MainWindow.EventsList)
            {
                Event newEvent = new Event()
                {
                    EventID = ev.EventID,
                    EventName = ev.EventName,
                    EventDates = ev.EventDates,
                    EventDesc = ev.EventDesc,
                    EventCost = ev.EventCost,
                    EventImgSrc = ev.EventImgSrc
                };

                EventsCollection.Add(newEvent); // Add to the existing collection
            }
        }
    }
}