using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Client.Net.Constance
{
    public enum ApplicationTypes
    {
        AppTypeMaster,
        AppTypeRecording,
        AppTypeDriver,
        AppTypeClient,
        AppTypeManager,
        AppTypeReplayer,
        AppTypeSDK,
        AppTypeCommon = 10,
        AppTypeFederation = 11,
    }
}
