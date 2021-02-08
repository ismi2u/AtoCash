using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoCash.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvanceOrReimburseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ClaimTypeDesc = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvanceOrReimburseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalGroupCode = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    ApprovalGroupDesc = table.Column<string>(type: "nvarchar(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCentres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostCentreCode = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    CostCentreDesc = table.Column<string>(type: "nvarchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCentres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpJobTypeCode = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    EmpJobTypeDesc = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseTypeName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ExpenseTypeDesc = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleCode = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    MaxPettyCashAllowed = table.Column<decimal>(type: "decimal(7,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeptCode = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    DeptName = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    CostCentreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_CostCentres_CostCentreId",
                        column: x => x.CostCentreId,
                        principalTable: "CostCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CostCentreId = table.Column<int>(type: "int", nullable: false),
                    ProjectDesc = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_CostCentres_CostCentreId",
                        column: x => x.CostCentreId,
                        principalTable: "CostCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalRoleMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalGroupId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ApprovalLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalRoleMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalRoleMaps_ApprovalGroups_ApprovalGroupId",
                        column: x => x.ApprovalGroupId,
                        principalTable: "ApprovalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalRoleMaps_JobRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EmpCode = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    BankCardNo = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NationalID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOJ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    EmploymentTypeId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ApprovalGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_ApprovalGroups_ApprovalGroupId",
                        column: x => x.ApprovalGroupId,
                        principalTable: "ApprovalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_EmploymentTypes_EmploymentTypeId",
                        column: x => x.EmploymentTypeId,
                        principalTable: "EmploymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_JobRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SubProjectName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    SubProjectDesc = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpCurrentPettyCashBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CurBalance = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpCurrentPettyCashBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpCurrentPettyCashBalances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectManagements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectManagements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectManagements_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectManagements_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "WorkTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubProjectId = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    TaskDesc = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTasks_SubProjects_SubProjectId",
                        column: x => x.SubProjectId,
                        principalTable: "SubProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseReimburseRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ExpenseReimbClaimAmount = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Documents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpReimReqDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    SubProjectId = table.Column<int>(type: "int", nullable: true),
                    WorkTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReimburseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseReimburseRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseReimburseRequests_ExpenseTypes_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseReimburseRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseReimburseRequests_SubProjects_SubProjectId",
                        column: x => x.SubProjectId,
                        principalTable: "SubProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseReimburseRequests_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PettyCashRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PettyClaimAmount = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    PettyClaimRequestDesc = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    CashReqDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    SubProjectId = table.Column<int>(type: "int", nullable: true),
                    WorkTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PettyCashRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PettyCashRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PettyCashRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PettyCashRequests_SubProjects_SubProjectId",
                        column: x => x.SubProjectId,
                        principalTable: "SubProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PettyCashRequests_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TravelApprovalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    TravelStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TravelEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    SubProjectId = table.Column<int>(type: "int", nullable: true),
                    WorkTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelApprovalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelApprovalRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelApprovalRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelApprovalRequests_SubProjects_SubProjectId",
                        column: x => x.SubProjectId,
                        principalTable: "SubProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelApprovalRequests_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimApprovalStatusTrackers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PettyCashRequestId = table.Column<int>(type: "int", nullable: false),
                    ExpenseReimburseRequestId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ReqDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalStatusTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimApprovalStatusTrackers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimApprovalStatusTrackers_ApprovalStatusTypes_ApprovalStatusTypeId",
                        column: x => x.ApprovalStatusTypeId,
                        principalTable: "ApprovalStatusTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimApprovalStatusTrackers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimApprovalStatusTrackers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClaimApprovalStatusTrackers_ExpenseReimburseRequests_ExpenseReimburseRequestId",
                        column: x => x.ExpenseReimburseRequestId,
                        principalTable: "ExpenseReimburseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClaimApprovalStatusTrackers_JobRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimApprovalStatusTrackers_PettyCashRequests_PettyCashRequestId",
                        column: x => x.PettyCashRequestId,
                        principalTable: "PettyCashRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClaimApprovalStatusTrackers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementsAndClaimsMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PettyCashRequestId = table.Column<int>(type: "int", nullable: true),
                    ExpenseReimburseReqId = table.Column<int>(type: "int", nullable: true),
                    AdvanceOrReimburseId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    SubProjectId = table.Column<int>(type: "int", nullable: true),
                    WorkTaskId = table.Column<int>(type: "int", nullable: true),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    CostCentreId = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementsAndClaimsMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_AdvanceOrReimburseTypes_AdvanceOrReimburseId",
                        column: x => x.AdvanceOrReimburseId,
                        principalTable: "AdvanceOrReimburseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_ApprovalStatusTypes_ApprovalStatusId",
                        column: x => x.ApprovalStatusId,
                        principalTable: "ApprovalStatusTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_CostCentres_CostCentreId",
                        column: x => x.CostCentreId,
                        principalTable: "CostCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_ExpenseReimburseRequests_ExpenseReimburseReqId",
                        column: x => x.ExpenseReimburseReqId,
                        principalTable: "ExpenseReimburseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_PettyCashRequests_PettyCashRequestId",
                        column: x => x.PettyCashRequestId,
                        principalTable: "PettyCashRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_SubProjects_SubProjectId",
                        column: x => x.SubProjectId,
                        principalTable: "SubProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementsAndClaimsMasters_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TravelApprovalStatusTrackers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    TravelApprovalRequestId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ReqDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalStatusTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelApprovalStatusTrackers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelApprovalStatusTrackers_ApprovalStatusTypes_ApprovalStatusTypeId",
                        column: x => x.ApprovalStatusTypeId,
                        principalTable: "ApprovalStatusTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelApprovalStatusTrackers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelApprovalStatusTrackers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TravelApprovalStatusTrackers_JobRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelApprovalStatusTrackers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TravelApprovalStatusTrackers_TravelApprovalRequests_TravelApprovalRequestId",
                        column: x => x.TravelApprovalRequestId,
                        principalTable: "TravelApprovalRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRoleMaps_ApprovalGroupId",
                table: "ApprovalRoleMaps",
                column: "ApprovalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRoleMaps_RoleId",
                table: "ApprovalRoleMaps",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimApprovalStatusTrackers_ApprovalStatusTypeId",
                table: "ClaimApprovalStatusTrackers",
                column: "ApprovalStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimApprovalStatusTrackers_DepartmentId",
                table: "ClaimApprovalStatusTrackers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimApprovalStatusTrackers_EmployeeId",
                table: "ClaimApprovalStatusTrackers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimApprovalStatusTrackers_ExpenseReimburseRequestId",
                table: "ClaimApprovalStatusTrackers",
                column: "ExpenseReimburseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimApprovalStatusTrackers_PettyCashRequestId",
                table: "ClaimApprovalStatusTrackers",
                column: "PettyCashRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimApprovalStatusTrackers_ProjectId",
                table: "ClaimApprovalStatusTrackers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimApprovalStatusTrackers_RoleId",
                table: "ClaimApprovalStatusTrackers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CostCentreId",
                table: "Departments",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_AdvanceOrReimburseId",
                table: "DisbursementsAndClaimsMasters",
                column: "AdvanceOrReimburseId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_ApprovalStatusId",
                table: "DisbursementsAndClaimsMasters",
                column: "ApprovalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_CostCentreId",
                table: "DisbursementsAndClaimsMasters",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_EmployeeId",
                table: "DisbursementsAndClaimsMasters",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_ExpenseReimburseReqId",
                table: "DisbursementsAndClaimsMasters",
                column: "ExpenseReimburseReqId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_PettyCashRequestId",
                table: "DisbursementsAndClaimsMasters",
                column: "PettyCashRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_ProjectId",
                table: "DisbursementsAndClaimsMasters",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_SubProjectId",
                table: "DisbursementsAndClaimsMasters",
                column: "SubProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementsAndClaimsMasters_WorkTaskId",
                table: "DisbursementsAndClaimsMasters",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpCurrentPettyCashBalances_EmployeeId",
                table: "EmpCurrentPettyCashBalances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApprovalGroupId",
                table: "Employees",
                column: "ApprovalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmploymentTypeId",
                table: "Employees",
                column: "EmploymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReimburseRequests_EmployeeId",
                table: "ExpenseReimburseRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReimburseRequests_ExpenseTypeId",
                table: "ExpenseReimburseRequests",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReimburseRequests_ProjectId",
                table: "ExpenseReimburseRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReimburseRequests_SubProjectId",
                table: "ExpenseReimburseRequests",
                column: "SubProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReimburseRequests_WorkTaskId",
                table: "ExpenseReimburseRequests",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_PettyCashRequests_EmployeeId",
                table: "PettyCashRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PettyCashRequests_ProjectId",
                table: "PettyCashRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PettyCashRequests_SubProjectId",
                table: "PettyCashRequests",
                column: "SubProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PettyCashRequests_WorkTaskId",
                table: "PettyCashRequests",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagements_EmployeeId",
                table: "ProjectManagements",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagements_ProjectId",
                table: "ProjectManagements",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CostCentreId",
                table: "Projects",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProjects_ProjectId",
                table: "SubProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalRequests_EmployeeId",
                table: "TravelApprovalRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalRequests_ProjectId",
                table: "TravelApprovalRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalRequests_SubProjectId",
                table: "TravelApprovalRequests",
                column: "SubProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalRequests_WorkTaskId",
                table: "TravelApprovalRequests",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalStatusTrackers_ApprovalStatusTypeId",
                table: "TravelApprovalStatusTrackers",
                column: "ApprovalStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalStatusTrackers_DepartmentId",
                table: "TravelApprovalStatusTrackers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalStatusTrackers_EmployeeId",
                table: "TravelApprovalStatusTrackers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalStatusTrackers_ProjectId",
                table: "TravelApprovalStatusTrackers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalStatusTrackers_RoleId",
                table: "TravelApprovalStatusTrackers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelApprovalStatusTrackers_TravelApprovalRequestId",
                table: "TravelApprovalStatusTrackers",
                column: "TravelApprovalRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_SubProjectId",
                table: "WorkTasks",
                column: "SubProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalRoleMaps");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClaimApprovalStatusTrackers");

            migrationBuilder.DropTable(
                name: "DisbursementsAndClaimsMasters");

            migrationBuilder.DropTable(
                name: "EmpCurrentPettyCashBalances");

            migrationBuilder.DropTable(
                name: "ProjectManagements");

            migrationBuilder.DropTable(
                name: "TravelApprovalStatusTrackers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AdvanceOrReimburseTypes");

            migrationBuilder.DropTable(
                name: "ExpenseReimburseRequests");

            migrationBuilder.DropTable(
                name: "PettyCashRequests");

            migrationBuilder.DropTable(
                name: "ApprovalStatusTypes");

            migrationBuilder.DropTable(
                name: "TravelApprovalRequests");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "WorkTasks");

            migrationBuilder.DropTable(
                name: "ApprovalGroups");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "EmploymentTypes");

            migrationBuilder.DropTable(
                name: "JobRoles");

            migrationBuilder.DropTable(
                name: "SubProjects");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "CostCentres");
        }
    }
}
