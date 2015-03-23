using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NiceGuid.Web.Configuration
{
    public interface IAppSettings
    {
        string TrackingCode { get; }
    }
}