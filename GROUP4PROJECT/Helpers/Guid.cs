using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROUP4PROJECT.Helpers
{
    public class GuidHelpers
    {
        public static bool isGuid(object obj)
        {
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return false;
            }

            var isGuid = Guid.TryParse(obj.ToString(), out var guid);

            if (!isGuid)
            {
                return false;
            }

            return true;
        }
    }
}