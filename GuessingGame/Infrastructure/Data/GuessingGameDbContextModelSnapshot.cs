﻿// <auto-generated />
using System;
using GuessingGame.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GuessingGame.Infrastructure.Data
{
    [DbContext(typeof(GuessingGameDbContext))]
    partial class GuessingGameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("GuessingGame.Core.Domain.History.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GameStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UncoveredSegments")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("History");
                });

            modelBuilder.Entity("GuessingGame.Core.Domain.Images.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PriorityScoreForBetterSegment")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Segment")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("SegmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("GuessingGame.Core.Domain.Oracle.BetterSegments", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("GameStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SegmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameStateId");

                    b.ToTable("BetterSegments");
                });

            modelBuilder.Entity("GuessingGame.Core.Domain.Oracle.ChoosenSegment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("GameStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SegmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameStateId");

                    b.ToTable("ChoosenSegment");
                });

            modelBuilder.Entity("GuessingGame.Core.Domain.Oracle.GameState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CorrectGuess")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("GameFinished")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("GameWon")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShownSegmentsNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsedGuesses")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("UserCanGuess")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("TEXT");

                    b.Property<int>("segmentsInImage")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("GuessingGame.Core.Domain.Oracle.BetterSegments", b =>
                {
                    b.HasOne("GuessingGame.Core.Domain.Oracle.GameState", null)
                        .WithMany("ProposedSegmentIds")
                        .HasForeignKey("GameStateId");
                });

            modelBuilder.Entity("GuessingGame.Core.Domain.Oracle.ChoosenSegment", b =>
                {
                    b.HasOne("GuessingGame.Core.Domain.Oracle.GameState", null)
                        .WithMany("ChoosenSegments")
                        .HasForeignKey("GameStateId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GuessingGame.Core.Domain.Oracle.GameState", b =>
                {
                    b.Navigation("ChoosenSegments");

                    b.Navigation("ProposedSegmentIds");
                });
#pragma warning restore 612, 618
        }
    }
}
