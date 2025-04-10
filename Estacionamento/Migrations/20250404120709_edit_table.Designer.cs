﻿// <auto-generated />
using System;
using Estacionamento.DataAccess.ContextApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Estacionamento.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250404120709_edit_table")]
    partial class edit_table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Estacionamento.Models.RegistroEstacionamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Finalizado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("HorarioEntrada")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("HorarioSaida")
                        .HasColumnType("datetime2");

                    b.Property<int>("MetodoPagamento")
                        .HasColumnType("int");

                    b.Property<bool>("Pago")
                        .HasColumnType("bit");

                    b.Property<string>("PlacaCarro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ValorPagar")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("RegistrosEstacionamentos");
                });

            modelBuilder.Entity("Estacionamento.Models.TabelaValor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PrecoPorMeiaHora")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TempoToleranciaMinutos")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TabelaValores");
                });
#pragma warning restore 612, 618
        }
    }
}
