using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Service;

namespace Inventory_mvc.Function
{
    public class EmailNotification
    {
        enum ApprovalStatus
        {
            Approved, Rejected
        }

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
        /// 0: Approved | 1: Rejected
        /// </param>
        public static void EmailNotificatioForRequisitionApprovalStatus(int requisitionNo, int status)
        {
            if(status.ToString() == ApprovalStatus.Approved.ToString())
            {

            }
            else if (status.ToString() == ApprovalStatus.Rejected.ToString())
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


        public static void EmailNotificatioForAdjustmentVoucherApprovalStatus(int voucherNo, int status)
        {
            if (status.ToString() == ApprovalStatus.Approved.ToString())
            {

            }
            else if (status.ToString() == ApprovalStatus.Rejected.ToString())
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