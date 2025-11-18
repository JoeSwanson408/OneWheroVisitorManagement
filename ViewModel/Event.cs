using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OnewheroVisitorManagement.ViewModel
{
    public class Event
    {
        private int _eventID;
        public int EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }
        private string _eventName;
        public string EventName
        {
            get { return _eventName; }
            set { _eventName = value; }
        }
        private string _eventDates;
        public string EventDates
        {
            get { return _eventDates; }
            set { _eventDates = value; }
        }

        private string _eventDesc;
        public string EventDesc
        {
            get { return _eventDesc; }
            set { _eventDesc = value; }
        }

        private double _eventCost;
        public double EventCost
        {
            get { return _eventCost; }
            set { _eventCost = value; }
        }

        private string _eventImgSrc;
        public string EventImgSrc
        {
            get { return _eventImgSrc; }
            set { _eventImgSrc = value; }
        }
        public Event()
        {
        }
    }
}
