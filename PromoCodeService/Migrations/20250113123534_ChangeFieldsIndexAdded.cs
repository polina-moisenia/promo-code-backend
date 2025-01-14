﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldsIndexAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "PromoCodes");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "PromoCodes",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PromoCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PromoCodes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_Code",
                table: "PromoCodes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_Code",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PromoCodes");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PromoCodes",
                newName: "ExpiryDate");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PromoCodes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "PromoCodes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RequestId",
                table: "PromoCodes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
