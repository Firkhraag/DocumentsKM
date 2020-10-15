﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DocumentsKM.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "document_types",
                columns: table => new
                {
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    code = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_document_types", x => x.type);
                });

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
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
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
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
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
                    main_builder_id = table.Column<int>(type: "integer", nullable: false),
                    current_specification_id = table.Column<int>(type: "integer", nullable: true),
                    approval_specialist1_id = table.Column<int>(type: "integer", nullable: true),
                    approval_specialist2_id = table.Column<int>(type: "integer", nullable: true),
                    approval_specialist3_id = table.Column<int>(type: "integer", nullable: true),
                    approval_specialist4_id = table.Column<int>(type: "integer", nullable: true),
                    approval_specialist5_id = table.Column<int>(type: "integer", nullable: true),
                    approval_specialist6_id = table.Column<int>(type: "integer", nullable: true),
                    approval_specialist7_id = table.Column<int>(type: "integer", nullable: true),
                    edited = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
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
                        name: "fk_marks_employees_approval_specialist1_id",
                        column: x => x.approval_specialist1_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_approval_specialist2_id",
                        column: x => x.approval_specialist2_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_approval_specialist3_id",
                        column: x => x.approval_specialist3_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_approval_specialist4_id",
                        column: x => x.approval_specialist4_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_approval_specialist5_id",
                        column: x => x.approval_specialist5_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_approval_specialist6_id",
                        column: x => x.approval_specialist6_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_marks_employees_approval_specialist7_id",
                        column: x => x.approval_specialist7_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "sheets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mark_id = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    document_type_id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    format = table.Column<float>(type: "real", nullable: false),
                    developer_id = table.Column<int>(type: "integer", nullable: false),
                    inspector_id = table.Column<int>(type: "integer", nullable: true),
                    norm_controller_id = table.Column<int>(type: "integer", nullable: true),
                    release = table.Column<byte>(type: "smallint", nullable: false),
                    number_of_pages = table.Column<byte>(type: "smallint", nullable: false),
                    note = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sheets", x => x.id);
                    table.ForeignKey(
                        name: "fk_sheets_document_types_document_type_id",
                        column: x => x.document_type_id,
                        principalTable: "document_types",
                        principalColumn: "type",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sheets_employees_developer_id",
                        column: x => x.developer_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sheets_employees_inspector_id",
                        column: x => x.inspector_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sheets_employees_norm_controller_id",
                        column: x => x.norm_controller_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sheets_marks_mark_id",
                        column: x => x.mark_id,
                        principalTable: "marks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specifications",
                columns: table => new
                {
                    position = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mark_id = table.Column<int>(type: "integer", nullable: false),
                    release_number = table.Column<byte>(type: "smallint", nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specifications", x => x.position);
                    table.ForeignKey(
                        name: "fk_specifications_marks_mark_id",
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
                name: "ix_marks_approval_specialist1_id",
                table: "marks",
                column: "approval_specialist1_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_approval_specialist2_id",
                table: "marks",
                column: "approval_specialist2_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_approval_specialist3_id",
                table: "marks",
                column: "approval_specialist3_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_approval_specialist4_id",
                table: "marks",
                column: "approval_specialist4_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_approval_specialist5_id",
                table: "marks",
                column: "approval_specialist5_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_approval_specialist6_id",
                table: "marks",
                column: "approval_specialist6_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_approval_specialist7_id",
                table: "marks",
                column: "approval_specialist7_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_chief_specialist_id",
                table: "marks",
                column: "chief_specialist_id");

            migrationBuilder.CreateIndex(
                name: "ix_marks_code",
                table: "marks",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_marks_current_specification_id",
                table: "marks",
                column: "current_specification_id",
                unique: true);

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
                name: "ix_marks_subnode_id_code",
                table: "marks",
                columns: new[] { "subnode_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_nodes_chief_engineer_id",
                table: "nodes",
                column: "chief_engineer_id");

            migrationBuilder.CreateIndex(
                name: "ix_nodes_code",
                table: "nodes",
                column: "code",
                unique: true);

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
                name: "ix_projects_base_series",
                table: "projects",
                column: "base_series",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sheets_developer_id",
                table: "sheets",
                column: "developer_id");

            migrationBuilder.CreateIndex(
                name: "ix_sheets_document_type_id",
                table: "sheets",
                column: "document_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_sheets_inspector_id",
                table: "sheets",
                column: "inspector_id");

            migrationBuilder.CreateIndex(
                name: "ix_sheets_mark_id_number_document_type_id",
                table: "sheets",
                columns: new[] { "mark_id", "number", "document_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sheets_norm_controller_id",
                table: "sheets",
                column: "norm_controller_id");

            migrationBuilder.CreateIndex(
                name: "ix_specifications_mark_id_release_number",
                table: "specifications",
                columns: new[] { "mark_id", "release_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subnodes_code",
                table: "subnodes",
                column: "code",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "fk_marks_specifications_current_specification_id",
                table: "marks",
                column: "current_specification_id",
                principalTable: "specifications",
                principalColumn: "position",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_departments_employees_department_head_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist1_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist2_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist3_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist4_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist5_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist6_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist7_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_chief_specialist_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_group_leader_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_main_builder_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist1_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist2_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist3_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist4_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist5_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist6_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist7_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_chief_specialist_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_group_leader_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_main_builder_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist1_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist2_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist3_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist4_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist5_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist6_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_approval_specialist7_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_chief_specialist_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_group_leader_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_employees_main_builder_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_departments_department_id",
                table: "marks");

            migrationBuilder.DropForeignKey(
                name: "fk_marks_specifications_current_specification_id",
                table: "marks");

            migrationBuilder.DropTable(
                name: "sheets");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "document_types");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "positions");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "specifications");

            migrationBuilder.DropTable(
                name: "marks");

            migrationBuilder.DropTable(
                name: "subnodes");

            migrationBuilder.DropTable(
                name: "nodes");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}