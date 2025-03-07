﻿using DataAccess.Helpers;
using EcoSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace esust.Models
{
    public class ESutContextDB : DbContext
    {
        private string connectionstring;
        public ESutContextDB()
        {
            connectionstring = ConfigHelper.getConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.

                optionsBuilder.UseSqlServer(connectionstring);


                // optionsBuilder.UseSqlServer("Data Source=104.40.131.15;Initial Catalog=biassist_optimized; User ID=sa; password=BlueOmega456;");
            }
        }

        public virtual DbSet<PageTbl> Pages { get; set; }
        public virtual DbSet<NewsTbl> News { get; set; }
        public virtual DbSet<EventTbl> Events { get; set; }
        public virtual DbSet<ImageSliderTbl> ImageSliders { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<MenuTbl> Menus { get; set; }
    }
}
