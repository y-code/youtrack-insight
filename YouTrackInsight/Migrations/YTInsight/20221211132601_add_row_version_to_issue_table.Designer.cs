﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YouTrackInsight.Entity;

#nullable disable

namespace YouTrackInsight.Migrations.YTInsight
{
    [DbContext(typeof(YTInsightDbContext))]
    [Migration("20221211132601_add_row_version_to_issue_table")]
    partial class addrowversiontoissuetable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("YouTrackInsight.Entity.YTIssueLinkModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Source")
                        .HasColumnType("text")
                        .HasColumnName("source");

                    b.Property<string>("Target")
                        .HasColumnType("text")
                        .HasColumnName("target");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("YTIssueModelId")
                        .HasColumnType("text");

                    b.Property<string>("YTIssueModelProjectId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("YTIssueModelId", "YTIssueModelProjectId");

                    b.ToTable("yt_issue_link");
                });

            modelBuilder.Entity("YouTrackInsight.Entity.YTIssueModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("ProjectId")
                        .HasColumnType("text")
                        .HasColumnName("project_id");

                    b.Property<string>("Summary")
                        .HasColumnType("text")
                        .HasColumnName("summary");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.HasKey("Id", "ProjectId");

                    b.ToTable("yt_issue");
                });

            modelBuilder.Entity("YouTrackInsight.Entity.YTIssueLinkModel", b =>
                {
                    b.HasOne("YouTrackInsight.Entity.YTIssueModel", null)
                        .WithMany("Links")
                        .HasForeignKey("YTIssueModelId", "YTIssueModelProjectId");
                });

            modelBuilder.Entity("YouTrackInsight.Entity.YTIssueModel", b =>
                {
                    b.Navigation("Links");
                });
#pragma warning restore 612, 618
        }
    }
}
