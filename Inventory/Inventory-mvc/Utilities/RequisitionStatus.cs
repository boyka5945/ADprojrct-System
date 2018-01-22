using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Utilities
{
    public static class RequisitionStatus
    {
        public const string PENDING_APPROVAL = "Pending Approval";
        public const string APPROVED_PROCESSING = "Approved and Processing";
        public const string PARTIALLY_FULFILLED = "Partially fulfilled";
        public const string COLLECTED = "Collected";
        public const string REJECTED = "Rejected";
    }
}