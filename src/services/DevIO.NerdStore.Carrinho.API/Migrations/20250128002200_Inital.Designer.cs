﻿// <auto-generated />
using System;
using DevIO.NerdStore.Carrinho.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevIO.NerdStore.Carrinho.API.Migrations
{
    [DbContext(typeof(CarrinhoContext))]
    [Migration("20250128002200_Inital")]
    partial class Inital
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevIO.NerdStore.Carrinho.API.Model.CarrinhoCliente", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClienteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ClienteId" }, "IDX_Cliente");

                    b.ToTable("CarrinhoCliente");
                });

            modelBuilder.Entity("DevIO.NerdStore.Carrinho.API.Model.CarrinhoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CarrinhoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Imagem")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CarrinhoId");

                    b.ToTable("CarrinhoItens");
                });

            modelBuilder.Entity("DevIO.NerdStore.Carrinho.API.Model.CarrinhoItem", b =>
                {
                    b.HasOne("DevIO.NerdStore.Carrinho.API.Model.CarrinhoCliente", "CarrinhoCliente")
                        .WithMany("Itens")
                        .HasForeignKey("CarrinhoId")
                        .IsRequired();

                    b.Navigation("CarrinhoCliente");
                });

            modelBuilder.Entity("DevIO.NerdStore.Carrinho.API.Model.CarrinhoCliente", b =>
                {
                    b.Navigation("Itens");
                });
#pragma warning restore 612, 618
        }
    }
}
