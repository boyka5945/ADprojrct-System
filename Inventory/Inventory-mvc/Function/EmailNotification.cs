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
            { (int) UserRoles.RoleID.StoreClerk, new int[] { (int) UserRoles.RoleID.StoreManager} },
            { (int) UserRoles.RoleID.StoreSupervisor, new int[] { (int) UserRoles.RoleID.StoreManager} }
        };

        // TODO - UNCOMMENT ALL THE EMAIL SEND
        private static IUserService userService = new UserService();
        private static IRequisitionRecordService requisitionService = new RequisitionRecordService();
        private static IAdjustmentVoucherService adjustmentVoucherService = new AdjustmentVoucherService();
        

        public static void EmailNotificatioForNewRequisition(string requesterID)
        {
            int userRole = userService.GetRoleByID(requesterID);
            string[] emailReciever = userService.FindApprovingStaffsEmailByRequesterID(requesterID, RequisitionApprovingStaffsRoleID[userRole]);

            string content = String.Format("There is one new stationery requisition from {0} pending your approval.", userService.FindNameByID(requesterID));
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
                string title = String.Format("Your stationery requisition (no: {0}) has been approved.", requisitionNo);
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, title, content);
                //email.send();

            }
            else if (status == RequisitionStatus.REJECTED)
            {
                string title = String.Format("Your stationery requisition (no: {0}) has been rejected.", requisitionNo);
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, title, content);
                //email.send();
            }
        }


        public static void EmailNotificationForNewAdjustmentVoucher(string requesterID, decimal voucherAmount)
        {
            List<string> emailAddress = new List<string>();

            if((voucherAmount *-1) <= 250)
            {
                // send email notification to supervisor               
                foreach(var i in userService.FindUsersByRole((int) UserRoles.RoleID.StoreSupervisor))
                {
                    emailAddress.Add(i.userEmail);
                }
            }
            else
            {
                // send email notification to store manager
                foreach (var i in userService.FindUsersByRole((int)UserRoles.RoleID.StoreManager))
                {
                    emailAddress.Add(i.userEmail);
                }
            }

            string content = String.Format("There is one new adjustment voucher from {0} pending your approval.", userService.FindNameByID(requesterID));
            foreach (string e in emailAddress)
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
                string title = String.Format("Your adjustment voucher (no: {0}) has been approved.", voucherNo);
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, title, content);
                //email.send();

            }
            else if (status == AdjustmentVoucherStatus.REJECTED)
            {
                string title = String.Format("Your adjustment voucher (no: {0}) has been rejected.", voucherNo);
                string content = remarks;
                sendEmail email = new sendEmail(emailAddress, title, content);
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

            string content = String.Format("Kindly be informed that the coming stationery collection date will be on {0}.", collectionDate.ToLongDateString());

            foreach (var e in emailAddress)
            {
                sendEmail email = new sendEmail(e, "Stationery collection date", content);
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

            string content = String.Format("Kindly be informed that the collection point of Dept {0} has been changed at {1}.", userService.FindDeptCodeByID(userRepID), DateTime.Today.ToLongDateString());

            foreach (var e in emailAddress)
            {
                sendEmail email = new sendEmail(e, "Collection point changes", content);
                //email.send();
            }
        }


    }
}