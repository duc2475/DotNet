using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ecommerce.Models
{
    public partial class ecommerceContext : DbContext
    {
        public ecommerceContext()
        {
        }

        public ecommerceContext(DbContextOptions<ecommerceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCart> TblCarts { get; set; } = null!;
        public virtual DbSet<TblCartDetail> TblCartDetails { get; set; } = null!;
        public virtual DbSet<TblColor> TblColors { get; set; } = null!;
        public virtual DbSet<TblCustMessage> TblCustMessages { get; set; } = null!;
        public virtual DbSet<TblCustomer> TblCustomers { get; set; } = null!;
        public virtual DbSet<TblEndCategory> TblEndCategories { get; set; } = null!;
        public virtual DbSet<TblFaq> TblFaqs { get; set; } = null!;
        public virtual DbSet<TblMidCategory> TblMidCategories { get; set; } = null!;
        public virtual DbSet<TblOrder> TblOrders { get; set; } = null!;
        public virtual DbSet<TblPoster> TblPosters { get; set; } = null!;
        public virtual DbSet<TblProduct> TblProducts { get; set; } = null!;
        public virtual DbSet<TblProductPic> TblProductPics { get; set; } = null!;
        public virtual DbSet<TblProductsPromotion> TblProductsPromotions { get; set; } = null!;
        public virtual DbSet<TblPromotion> TblPromotions { get; set; } = null!;
        public virtual DbSet<TblTopCategory> TblTopCategories { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
        public virtual DbSet<TblUserCustomer> TblUserCustomers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ADMIN-PC;Database=ecommerce;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCart>(entity =>
            {
                entity.HasKey(e => e.CartId)
                    .HasName("PK__tbl_cart__2EF52A2725F1212E");

                entity.ToTable("tbl_cart");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.CartDate)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cart_date");

                entity.Property(e => e.CartStatus).HasColumnName("cart_status");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.TotalPrice)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("total_price");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.TblCarts)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cust_id");
            });

            modelBuilder.Entity<TblCartDetail>(entity =>
            {
                entity.HasKey(e => e.CartDetailId)
                    .HasName("PK__tbl_cart__0F08F5295825D6F6");

                entity.ToTable("tbl_cart_detail");

                entity.Property(e => e.CartDetailId).HasColumnName("cart_detail_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.Color)
                    .HasMaxLength(100)
                    .HasColumnName("color");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .HasColumnName("product_name");

                entity.Property(e => e.ProductPic)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("product_pic");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.TblCartDetails)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cart_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblCartDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_productC_id");
            });

            modelBuilder.Entity<TblColor>(entity =>
            {
                entity.HasKey(e => e.ColorId)
                    .HasName("PK__tbl_colo__1143CECBFDD96443");

                entity.ToTable("tbl_color");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.Property(e => e.ColorName)
                    .HasMaxLength(255)
                    .HasColumnName("color_name");
            });

            modelBuilder.Entity<TblCustMessage>(entity =>
            {
                entity.HasKey(e => e.CustMsgId)
                    .HasName("PK__tbl_cust__543D3148375D3440");

                entity.ToTable("tbl_cust_message");

                entity.Property(e => e.CustMsgId).HasColumnName("cust_msg_id");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.Msg)
                    .HasColumnType("text")
                    .HasColumnName("msg");

                entity.Property(e => e.SubjectMsg)
                    .HasMaxLength(255)
                    .HasColumnName("subject_msg");
            });

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.HasKey(e => e.CustId)
                    .HasName("PK__tbl_cust__A1B71F90E6492F75");

                entity.ToTable("tbl_customer");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.CustAddress)
                    .HasColumnType("text")
                    .HasColumnName("cust_address");

                entity.Property(e => e.CustCity)
                    .HasMaxLength(100)
                    .HasColumnName("cust_city");

                entity.Property(e => e.CustDatetime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cust_datetime");

                entity.Property(e => e.CustEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cust_email");

                entity.Property(e => e.CustLname)
                    .HasMaxLength(255)
                    .HasColumnName("cust_lname");

                entity.Property(e => e.CustName)
                    .HasMaxLength(100)
                    .HasColumnName("cust_name");

                entity.Property(e => e.CustPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cust_phone");

                entity.Property(e => e.CustSate)
                    .HasMaxLength(100)
                    .HasColumnName("cust_sate");

                entity.Property(e => e.CustStatus).HasColumnName("cust_status");

                entity.Property(e => e.CustZip)
                    .HasMaxLength(30)
                    .HasColumnName("cust_zip");
            });

            modelBuilder.Entity<TblEndCategory>(entity =>
            {
                entity.HasKey(e => e.EcatId)
                    .HasName("PK__tbl_end___BD402AE29BAFE28A");

                entity.ToTable("tbl_end_category");

                entity.Property(e => e.EcatId).HasColumnName("ecat_id");

                entity.Property(e => e.EcatName)
                    .HasMaxLength(255)
                    .HasColumnName("ecat_name");

                entity.Property(e => e.McatId).HasColumnName("mcat_id");

                entity.HasOne(d => d.Mcat)
                    .WithMany(p => p.TblEndCategories)
                    .HasForeignKey(d => d.McatId)
                    .HasConstraintName("fk_mcat_id");
            });

            modelBuilder.Entity<TblFaq>(entity =>
            {
                entity.HasKey(e => e.FaqId)
                    .HasName("PK__tbl_faq__66734BAF427A0BDB");

                entity.ToTable("tbl_faq");

                entity.Property(e => e.FaqId).HasColumnName("faq_id");

                entity.Property(e => e.FaqContent)
                    .HasColumnType("text")
                    .HasColumnName("faq_content");

                entity.Property(e => e.FaqTitle)
                    .HasMaxLength(255)
                    .HasColumnName("faq_title");
            });

            modelBuilder.Entity<TblMidCategory>(entity =>
            {
                entity.HasKey(e => e.McatId)
                    .HasName("PK__tbl_mid___AADAB73F17A158E2");

                entity.ToTable("tbl_mid_category");

                entity.Property(e => e.McatId).HasColumnName("mcat_id");

                entity.Property(e => e.McatName)
                    .HasMaxLength(255)
                    .HasColumnName("mcat_name");

                entity.Property(e => e.TcatId).HasColumnName("tcat_id");

                entity.HasOne(d => d.Tcat)
                    .WithMany(p => p.TblMidCategories)
                    .HasForeignKey(d => d.TcatId)
                    .HasConstraintName("fk_tcat_id");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__tbl_orde__46596229CB5AFEBD");

                entity.ToTable("tbl_order");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.CustName)
                    .HasMaxLength(255)
                    .HasColumnName("cust_name");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(255)
                    .HasColumnName("order_status");

                entity.Property(e => e.ShipingAddress)
                    .HasMaxLength(255)
                    .HasColumnName("shiping_address");

                entity.Property(e => e.ShipingCity)
                    .HasMaxLength(100)
                    .HasColumnName("shiping_city");

                entity.Property(e => e.TotalPrice)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("total_price");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cartO_id");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_custO_id");
            });

            modelBuilder.Entity<TblPoster>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PK__tmp_ms_x__3ED78766A5FDF7A8");

                entity.ToTable("tbl_poster");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.PostName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("post_name");

                entity.Property(e => e.PostTile)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("post_tile");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__tbl_prod__47027DF590A923BB");

                entity.ToTable("tbl_product");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Color)
                    .HasMaxLength(100)
                    .HasColumnName("color");

                entity.Property(e => e.EcatId).HasColumnName("ecat_id");

                entity.Property(e => e.ProductDes).HasColumnName("product_des");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .HasColumnName("product_name");

                entity.Property(e => e.ProductPic)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("product_pic");

                entity.Property(e => e.ProductPrice).HasColumnName("product_price");

                entity.Property(e => e.ProductSeo)
                    .HasMaxLength(255)
                    .HasColumnName("product_seo");

                entity.Property(e => e.ProductStatus)
                    .HasMaxLength(100)
                    .HasColumnName("product_status");

                entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");

                entity.HasOne(d => d.Ecat)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.EcatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ecat_id");
            });

            modelBuilder.Entity<TblProductPic>(entity =>
            {
                entity.HasKey(e => e.PicId)
                    .HasName("PK__tbl_prod__5A0338178D39E603");

                entity.ToTable("tbl_product_pic");

                entity.Property(e => e.PicId).HasColumnName("pic_id");

                entity.Property(e => e.PicName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pic_name");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblProductPics)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_product_id");
            });

            modelBuilder.Entity<TblProductsPromotion>(entity =>
            {
                entity.HasKey(e => e.PpId)
                    .HasName("PK__tbl_prod__2FCD841FBBD88E1A");

                entity.ToTable("tbl_products_promotion");

                entity.Property(e => e.PpId).HasColumnName("pp_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.PromoId).HasColumnName("promo_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblProductsPromotions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pro2_id");

                entity.HasOne(d => d.Promo)
                    .WithMany(p => p.TblProductsPromotions)
                    .HasForeignKey(d => d.PromoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_promo_id");
            });

            modelBuilder.Entity<TblPromotion>(entity =>
            {
                entity.HasKey(e => e.PromoId)
                    .HasName("PK__tbl_prom__84EB4CA594F3E1E3");

                entity.ToTable("tbl_promotion");

                entity.Property(e => e.PromoId).HasColumnName("promo_id");

                entity.Property(e => e.PromoDiscount).HasColumnName("promo_discount");

                entity.Property(e => e.PromoEdate)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("promo_edate");

                entity.Property(e => e.PromoName)
                    .HasMaxLength(255)
                    .HasColumnName("promo_name");

                entity.Property(e => e.PromoSdate)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("promo_sdate");
            });

            modelBuilder.Entity<TblTopCategory>(entity =>
            {
                entity.HasKey(e => e.TcatId)
                    .HasName("PK__tbl_top___009FB96DF41F4C98");

                entity.ToTable("tbl_top_category");

                entity.Property(e => e.TcatId).HasColumnName("tcat_id");

                entity.Property(e => e.ShowOnMenu).HasColumnName("show_on_menu");

                entity.Property(e => e.TcatName)
                    .HasMaxLength(255)
                    .HasColumnName("tcat_name");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__tbl_user__B9BE370F762C267C");

                entity.ToTable("tbl_user");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPass)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_pass");

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_phone");

                entity.Property(e => e.UserPhoto)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_photo");

                entity.Property(e => e.UserRole)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("user_role");

                entity.Property(e => e.UserStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("user_status");
            });

            modelBuilder.Entity<TblUserCustomer>(entity =>
            {
                entity.ToTable("tbl_user_customer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.TblUserCustomers)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cust_user");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserCustomers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_cust");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
