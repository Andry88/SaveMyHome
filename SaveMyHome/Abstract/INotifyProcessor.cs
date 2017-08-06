using SaveMyHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaveMyHome.Abstract
{
    public interface INotifyProcessor
    {
        void ProcessNotify(Message message, IEnumerable<Apartment> apartments);
    }
}