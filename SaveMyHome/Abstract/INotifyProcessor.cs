using SaveMyHome.Models;
using System.Collections.Generic;

namespace SaveMyHome.Abstract
{
    public interface INotifyProcessor
    {
        void ProcessNotify(Message message, IEnumerable<Apartment> apartments);
    }
}