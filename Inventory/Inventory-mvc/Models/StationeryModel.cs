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

        public virtual DbSet<Adjustment_Voucher_Record> Adjustment_Voucher_Record { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Collection_Point> Collection_Point { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Inventory_Status_Record> Inventory_Status_Record { get; set; }
        public virtual DbSet<permissionInfo> permissionInfo { get; set; }
        public virtual DbSet<Purchase_Details> Purchase_Details { get; set; }
        public virtual DbSet<Purchase_Order_Record> Purchase_Order_Record { get; set; }
        public virtual DbSet<Requisition_Details> Requisition_Details { get; set; }
        public virtual DbSet<Requisition_Record> Requisition_Record { get; set; }
        public virtual DbSet<roleInfo> roleInfo { get; set; }
        public virtual DbSet<rolePermission> rolePermission { get; set; }
        public virtual DbSet<Stationery> Stationery { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<Transaction_Details> Transaction_Details { get; set; }
        public virtual DbSet<Transaction_Record> Transaction_Record { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Voucher_Details> Voucher_Details { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adjustment_Voucher_Record>()
                .HasMany(e => e.Voucher_Details)
                .WithRequired(e => e.Adjustment_Voucher_Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Stationery)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Collection_Point>()
                .HasMany(e => e.Department)
                .WithRequired(e => e.Collection_Point)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Requisition_Record)
                .WithRequired(e => e.Department)
                .HasForeignKey(e => e.deptCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.User2)
                .WithRequired(e => e.Department2)
                .HasForeignKey(e => e.departmentCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<permissionInfo>()
                .HasMany(e => e.rolePermission)
                .WithRequired(e => e.permissionInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Purchase_Order_Record>()
                .HasMany(e => e.Purchase_Details)
                .WithRequired(e => e.Purchase_Order_Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Requisition_Record>()
                .HasMany(e => e.Requisition_Details)
                .WithRequired(e => e.Requisition_Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<roleInfo>()
                .HasMany(e => e.rolePermission)
                .WithRequired(e => e.roleInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Inventory_Status_Record)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Purchase_Details)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.Requisition_Details)
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
                .HasMany(e => e.Stationery)
                .WithRequired(e => e.Supplier)
                .HasForeignKey(e => e.firstSupplierCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationery1)
                .WithOptional(e => e.Supplier1)
                .HasForeignKey(e => e.secondSupplierCode);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationery2)
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
                .HasMany(e => e.Department)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.departmentHeadID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Department1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.representativeID)
                .WillCascadeOnDelete(false);

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
