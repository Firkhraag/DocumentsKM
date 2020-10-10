using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DocumentsKM.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    short_name = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_positions", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    recruited_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fired_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    department_number = table.Column<int>(type: "integer", nullable: false),
                    position_code = table.Column<int>(type: "integer", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    has_canteen = table.Column<bool>(type: "boolean", nullable: false),
                    vacation_type = table.Column<int>(type: "integer", nullable: false),
                    begin_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_positions_position_code",
                        column: x => x.position_code,
                        principalTable: "positions",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    number = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false),
                    short_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_industrial = table.Column<bool>(type: "boolean", nullable: false),
                    department_head_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.number);
                    table.ForeignKey(
                        name: "fk_departments_employees_department_head_id",
                        column: x => x.department_head_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    additional_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    base_series = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    approved1_id = table.Column<int>(type: "integer", nullable: true),
                    approved2_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                    table.ForeignKey(
                        name: "fk_projects_employees_approved1_id",
                        column: x => x.approved1_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_projects_employees_approved2_id",
                        column: x => x.approved2_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    project_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    additional_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    chief_engineer_id = table.Column<int>(type: "integer", nullable: false),
                    active_node = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nodes", x => x.id);
                    table.ForeignKey(
                        name: "fk_nodes_employees_chief_engineer_id",
                        column: x => x.chief_engineer_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_nodes_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subnodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    node_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    additional_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subnodes", x => x.id);
                    table.ForeignKey(
                        name: "fk_subnodes_nodes_node_id",
                        column: x => x.node_id,
                        principalTable: "nodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "marks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subnode_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    additional_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
                    chief_specialist_id = table.Column<int>(type: "integer", nullable: true),
                    group_leader_id = table.Column<int>(type: "integer", nullable: true),
                    main_builder_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_marks", x => x.id);
                    table.ForeignKey(
                        name: "fk_marks_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_marks_employees_chief_specialist_id",
                        column: x => x.chief_specialist_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_group_leader_id",
                        column: x => x.group_leader_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_main_builder_id",
                        column: x => x.main_builder_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_marks_subnodes_subnode_id",
                        column: x => x.subnode_id,
                        principalTable: "subnodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "marks_approvals",
                columns: table => new
                {
                    mark_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_marks_approvals", x => new { x.mark_id, x.employee_id });
                    table.ForeignKey(
                        name: "fk_marks_approvals_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_marks_approvals_marks_mark_id",
                        column: x => x.mark_id,
                        principalTable: "marks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_departments_department_head_id",
                table: "departments",
                column: "department_head_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_department_number",
                table: "employees",
                column: "department_number");

            migrationBuilder.CreateIndex(
                name: "ix_employees_position_code",
                table: "employees",
                column: "position_code");

            migrationBuilder.CreateIndex(
                name: "ix_marks_chief_specialist_id",
                table: "marks",
                column: "chief_specialist_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_department_id",
                table: "marks",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_group_leader_id",
                table: "marks",
                column: "group_leader_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_main_builder_id",
                table: "marks",
                column: "main_builder_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_subnode_id",
                table: "marks",
                column: "subnode_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_approvals_employee_id",
                table: "marks_approvals",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_nodes_chief_engineer_id",
                table: "nodes",
                column: "chief_engineer_id");

            migrationBuilder.CreateIndex(
                name: "ix_nodes_project_id",
                table: "nodes",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_projects_approved1_id",
                table: "projects",
                column: "approved1_id");

            migrationBuilder.CreateIndex(
                name: "ix_projects_approved2_id",
                table: "projects",
                column: "approved2_id");

            migrationBuilder.CreateIndex(
                name: "ix_subnodes_node_id",
                table: "subnodes",
                column: "node_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_employee_id",
                table: "users",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_employees_departments_department_number",
                table: "employees",
                column: "department_number",
                principalTable: "departments",
                principalColumn: "number",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_departments_employees_department_head_id",
                table: "departments");

            migrationBuilder.DropTable(
                name: "marks_approvals");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "marks");

            migrationBuilder.DropTable(
                name: "subnodes");

            migrationBuilder.DropTable(
                name: "nodes");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "positions");
        }
    }
}
