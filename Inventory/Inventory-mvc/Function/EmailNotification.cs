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
        // Employee = 3 | UR = 4 ==> DeptHead = 2, ActingDeptHead = 8
        // StoreClerk = 7 ==> Manager = 5 
        private static Dictionary<int, int[]> RequisitionApprovingStaffsRoleID = new Dictionary<int, int[]>()
        {
            { (int) UserRoles.RoleID.Employee, new int[] { (int) UserRoles.RoleID.DepartmentHead, (int) UserRoles.RoleID.ActingDepartmentHead} },
            { (int) UserRoles.RoleID.UserRepresentative, new int[] { (int) UserRoles.RoleID.DepartmentHead, (int) UserRoles.RoleID.ActingDepartmentHead, (int) UserRoles.RoleID.StoreManager } },
            { (int) UserRoles.RoleID.StoreClerk, new int[] { (int) UserRoles.RoleID.StoreManager} }
        };


        private static IUserService userService = new UserService();
        private static IRequisitionRecordService requisitionService = new RequisitionRecordService();
        private static IAdjustmentVoucherService adjustmentVoucherService = new AdjustmentVoucherService();
        

        public static void EmailNotificatioForNewRequisition(string requesterID)
        {
            int userRole = userService.GetRoleByID(requesterID);
            string[] emailReciever = userService.FindApprovingStaffsEmailByRequesterID(requesterID, RequisitionApprovingStaffsRoleID[userRole]);

            // TODO : WRITE EMAIL CONTENT TEMPLATE
            string content = "Some message";
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
        public static void EmailNotificatioForRequisitionApprovalStatus(int requisitionNo, string status, string remarks)
        {
            string requesterID = requisitionService.GetRequisitionByID(requisitionNo).requesterID;
            string emailAddress = userService.FindByUserID(requesterID).userEmail;

            if(status == RequisitionStatus.APPROVED_PROCESSING)
            {
                // TODO : WRITE EMAIL CONTENT TEMPLATE
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, "Stationery requisition has been approved", content);
                //email.send();

            }
            else if (status == RequisitionStatus.REJECTED)
            {
                // TODO : WRITE EMAIL CONTENT TEMPLATE
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, "Stationery requisition has been rejected", content);
                //email.send();
            }
        }


        public static void EmailNotificationForNewAdjustmentVoucher(string requesterID, decimal voucherAmount)
        {
            List<string> emailAddress = new List<string>();

            if((voucherAmount *-1) <= 250)
            {
                // supervisor               
                foreach(var i in userService.FindUsersByRole((int) UserRoles.RoleID.StoreSupervisor))
                {
                    emailAddress.Add(i.userEmail);
                }
            }
            else
            {
                // manager
                foreach (var i in userService.FindUsersByRole((int)UserRoles.RoleID.StoreManager))
                {
                    emailAddress.Add(i.userEmail);
                }
            }

            // TODO : WRITE EMAIL CONTENT TEMPLATE
            string content = "Some content";
            foreach(var e in emailAddress)
            {
                sendEmail email = new sendEmail(e, "New adjustment voucher pending approval", content);
                //email.send();
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
            string requesterID = adjustmentVoucherService.FindVoucherRecordByVoucherNo(voucherNo).handlingStaffID;
            string emailAddress = userService.FindByUserID(requesterID).userEmail;

            if (status == AdjustmentVoucherStatus.APPROVED)
            {
                // TODO : WRITE EMAIL CONTENT TEMPLATE
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, "Adjustment voucher has been approved", content);
                //email.send();

            }
            else if (status == AdjustmentVoucherStatus.REJECTED)
            {
                // TODO : WRITE EMAIL CONTENT TEMPLATE
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, "Adjustment voucher has been rejected", content);
                //email.send();
            }
        }


        public static void EmailNotificationForItemCollection(DateTime collectionDate)
        {
            List<string> emailAddress = new List<string>();

            foreach (var i in userService.FindUsersByRole((int)UserRoles.RoleID.UserRepresentative))
            {
                emailAddress.Add(i.userEmail);
            }

            // TODO : WRITE EMAIL CONTENT TEMPLATE
            string content = "Some message";

            foreach (var e in emailAddress)
            {
                sendEmail email = new sendEmail(e, "Stationery pending collection", content);
                //email.send();
            }
        }

        public static void EmailNotificationForCollectionPointChange(string userRepID)
        {
            List<string> emailAddress = new List<string>();

            foreach (var i in userService.FindUsersByRole((int)UserRoles.RoleID.StoreClerk))
            {
                emailAddress.Add(i.userEmail);
            }

            // TODO : WRITE EMAIL CONTENT TEMPLATE
            string content = "Some message";

            foreach (var e in emailAddress)
            {
                sendEmail email = new sendEmail(e, "Collection point changes", content);
                //email.send();
            }
        }
    }
}