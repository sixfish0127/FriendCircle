using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 宏碁班專案_社交媒體平台MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    userimage = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    birthday = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    phone = table.Column<int>(type: "int", nullable: true),
                    address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    initDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.UniqueConstraint("AK_Posts_PostId", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_userInfo",
                        column: x => x.UserId,
                        principalTable: "userInfo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ComentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ParenCommentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ComentId);
                    table.ForeignKey(
                        name: "FK_Comments_Comments",
                        column: x => x.ParenCommentId,
                        principalTable: "Comments",
                        principalColumn: "ComentId");
                    table.ForeignKey(
                        name: "FK_Comments_Posts",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_Comments_userInfo",
                        column: x => x.UserId,
                        principalTable: "userInfo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CommentFavorites",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentFavorites", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentFavorites_Comments",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "ComentId");
                    table.ForeignKey(
                        name: "FK_CommentFavorites_userInfo",
                        column: x => x.UserId,
                        principalTable: "userInfo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CommentShares",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentShares", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentShares_Comments",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "ComentId");
                    table.ForeignKey(
                        name: "FK_CommentShares_userInfo",
                        column: x => x.UserId,
                        principalTable: "userInfo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ReactionType",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReactionType = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionType", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ReactionType_Comments",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "ComentId");
                    table.ForeignKey(
                        name: "FK_ReactionType_userInfo",
                        column: x => x.UserId,
                        principalTable: "userInfo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentFavorites_UserId",
                table: "CommentFavorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParenCommentId",
                table: "Comments",
                column: "ParenCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentShares_UserId",
                table: "CommentShares",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts",
                table: "Posts",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionType_UserId",
                table: "ReactionType",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_account",
                table: "userInfo",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentFavorites");

            migrationBuilder.DropTable(
                name: "CommentShares");

            migrationBuilder.DropTable(
                name: "ReactionType");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "userInfo");
        }
    }
}
