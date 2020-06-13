﻿// <auto-generated />
using System;
using IPayYouPrint.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IPayYouPrint.Migrations
{
    [DbContext(typeof(IPayYouPrintContext))]
    partial class IPayYouPrintContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("IPayYouPrint.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("address");

                    b.Property<string>("email");

                    b.Property<string>("name");

                    b.Property<string>("password");

                    b.Property<bool>("supplier");

                    b.Property<string>("surname");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("IPayYouPrint.Models._3dModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("file_location");

                    b.Property<DateTime>("upload_date");

                    b.HasKey("id");

                    b.ToTable("_3dModel");
                });
#pragma warning restore 612, 618
        }
    }
}
