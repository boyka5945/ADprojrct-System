namespace Inventory_mvc.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StationeryModel : DbContext
    {
        public StationeryModel()
            : base("name=StationeryModel")
        {
        }

        public virtual DbSet<Adjustment_Voucher_Record> Adjustment_Voucher_Records { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Collection_Point> Collection_Points { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Inventory_Status_Record> Inventory_Status_Records { get; set; }
        public virtual DbSet<permissionInfo> permissionInfoes { get; set; }
        public virtual DbSet<Purchase_Detail> Purchase_Detail { get; set; }
        public virtual DbSet<Purchase_Order_Record> Purchase_Order_Records { get; set; }
        public virtual DbSet<Requisition_Detail> Requisition_Detail { get; set; }
        public virtual DbSet<Requisition_Record> Requisition_Records { get; set; }
        public virtual DbSet<roleInfo> roleInfoes { get; set; }
        public virtual DbSet<rolePermission> rolePermissions { get; set; }
        public virtual DbSet<Stationery> Stationeries { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Transaction_Detail> Transaction_Details { get; set; }
        public virtual DbSet<Transaction_Record> Transaction_Records { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Voucher_Detail> Voucher_Details { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adjustment_Voucher_Record>()
                .HasMany(e => e.Voucher_Details)
                .WithRequired(e => e.Adjustment_Voucher_Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Stationeries)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Collection_Point>()
                .HasMany(e => e.Departments)
                .WithRequired(e => e.Collection_Point)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Requisition_Record)
                .WithRequired(e => e.Department)
                .HasForeignKey(e => e.deptCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<permissionInfo>()
                .HasMany(e => e.rolePermissions)
                .WithRequired(e => e.permissionInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Purchase_Order_Record>()
                .HasMany(e => e.Purchase_Detail)
                .WithRequired(e => e.Purchase_Order_Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Requisition_Record>()
                .HasMany(e => e.Requisition_Detail)
                .WithRequired(e => e.Requisition_Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<roleInfo>()
                .HasMany(e => e.rolePermissions)
                .WithRequired(e => e.roleInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<roleInfo>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.roleInfo)
                .HasForeignKey(e => e.role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Inventory_Status_Record)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Purchase_Detail)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Requisition_Detail)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Transaction_Details)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Voucher_Details)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationeries)
                .WithRequired(e => e.Supplier)
                .HasForeignKey(e => e.firstSupplierCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationeries1)
                .WithOptional(e => e.Supplier1)
                .HasForeignKey(e => e.secondSupplierCode);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationeries2)
                .WithOptional(e => e.Supplier2)
                .HasForeignKey(e => e.thirdSupplierCode);

            modelBuilder.Entity<Transaction_Record>()
                .HasMany(e => e.Transaction_Details)
                .WithRequired(e => e.Transaction_Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Adjustment_Voucher_Record)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.handlingStaffID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Adjustment_Voucher_Record1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.authorisingStaffID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Purchase_Order_Record)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.clerkID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Requisition_Record)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.requesterID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Requisition_Record1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.approvingStaffID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Transaction_Record)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.clerkID);
        }
    }
}
