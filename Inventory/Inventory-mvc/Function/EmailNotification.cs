using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Service;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Function
{
    public class EmailNotification
    {
        private static IUserService userService = new UserService();

        public static void EmailNotificatioForNewRequisition(string requesterID)
        {
            // TODO : WRITE EMAIL CONTENT TEMPLATE
            string content = "Some message";

            string[] emailReciever = userService.FindApprovingStaffsEmailByRequesterID(requesterID);

            foreach (var emailaddress in emailReciever)
            {
                sendEmail email = new sendEmail(emailaddress, "New Stationery Requisition", content);
                //email.send();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisitionNo"></param>
        /// <param name="status">
        /// RequisitionStatus => Approved and Processing | Rejected
        /// </param>
        public static void EmailNotificatioForRequisitionApprovalStatus(int requisitionNo, string status)
        {
            if(status == RequisitionStatus.APPROVED_PROCESSING)
            {

            }
            else if (status == RequisitionStatus.REJECTED)
            {

            }

        }


        public static void EmailNotificationForNewAdjustmentVoucher(string requesterID, decimal voucherAmount)
        {
            if(voucherAmount <= 250)
            {
                // supervisor
            }
            else
            {
                // manager
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="status">
        /// AdjustmentVoucherStatus => Approved | Rejected
        /// </param>
        /// <param name="remarks"></param>
        public static void EmailNotificatioForAdjustmentVoucherApprovalStatus(int voucherNo, string status, string remarks)
        {
            if (status == AdjustmentVoucherStatus.APPROVED)
            {

            }
            else if (status == AdjustmentVoucherStatus.REJECTED)
            {

            }
        }


        public static void EmailNotificationForItemCollection(DateTime collectionDate)
        {

        }

        public static void EmailNotificationForCollectionPointChange(string userRepID)
        {

        }


    }
}