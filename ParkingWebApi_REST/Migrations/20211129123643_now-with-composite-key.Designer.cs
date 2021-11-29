﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi_REST.Models;

namespace WebApi_REST.Migrations
{
    [DbContext(typeof(ParkingDbContext))]
    [Migration("20211129123643_now-with-composite-key")]
    partial class nowwithcompositekey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("WebApi_REST.Models.ParkingSlot", b =>
                {
                    b.Property<int>("ParkingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SensorDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Occupied")
                        .HasColumnType("bit");

                    b.HasKey("ParkingId", "SensorDateTime");

                    b.ToTable("ParkingSlots");
                });

            modelBuilder.Entity("WebApi_REST.Models.Sensor", b =>
                {
                    b.Property<int>("SensorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ParkingId")
                        .HasColumnType("int");

                    b.HasKey("SensorId");

                    b.ToTable("Sensors");
                });
#pragma warning restore 612, 618
        }
    }
}
