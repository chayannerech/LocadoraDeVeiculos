using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocadoraDeVeiculos.Infra.Orm.Migrations
{
    /// <inheritdoc />
    public partial class BancoDeDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
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
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                    UserId = table.Column<int>(type: "int", nullable: false)
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
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
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
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                name: "TBAluguel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CondutorNome = table.Column<string>(type: "varchar(200)", nullable: false),
                    CondutorId = table.Column<int>(type: "int", nullable: false),
                    ClienteNome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    GrupoNome = table.Column<string>(type: "varchar(200)", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    CategoriaPlano = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VeiculoId = table.Column<int>(type: "int", nullable: false),
                    VeiculoPlaca = table.Column<string>(type: "varchar(10)", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRetornoPrevista = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRetornoReal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxasSelecionadasId = table.Column<string>(type: "varchar(10)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBAluguel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBAluguel_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", nullable: false),
                    Telefone = table.Column<string>(type: "varchar(200)", nullable: false),
                    PessoaFisica = table.Column<bool>(type: "bit", nullable: false),
                    Documento = table.Column<string>(type: "varchar(200)", nullable: false),
                    RG = table.Column<string>(type: "varchar(200)", nullable: false),
                    CNH = table.Column<string>(type: "varchar(200)", nullable: false),
                    Estado = table.Column<string>(type: "varchar(200)", nullable: false),
                    Cidade = table.Column<string>(type: "varchar(200)", nullable: false),
                    Bairro = table.Column<string>(type: "varchar(200)", nullable: false),
                    Rua = table.Column<string>(type: "varchar(200)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCliente_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBConfiguracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gasolina = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Etanol = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diesel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GNV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBConfiguracao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBConfiguracao_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBFuncionario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Login = table.Column<string>(type: "varchar(100)", nullable: false),
                    Senha = table.Column<string>(type: "varchar(64)", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBFuncionario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBFuncionario_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBGrupoDeAutomoveis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    PrecoDiaria = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoDiariaControlada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoLivre = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBGrupoDeAutomoveis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBGrupoDeAutomoveis_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBTaxa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoFixo = table.Column<bool>(type: "bit", nullable: false),
                    Seguro = table.Column<bool>(type: "bit", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBTaxa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBTaxa_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBCondutor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cliente_Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", nullable: false),
                    Telefone = table.Column<string>(type: "varchar(200)", nullable: false),
                    CPF = table.Column<string>(type: "varchar(15)", nullable: false),
                    CNH = table.Column<string>(type: "varchar(15)", nullable: false),
                    ValidadeCNH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCondutor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCondutor_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBCondutor_TBCliente_Cliente_Id",
                        column: x => x.Cliente_Id,
                        principalTable: "TBCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBPlanoDeCobranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grupo_Id = table.Column<int>(type: "int", nullable: false),
                    PrecoDiaria = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoKm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KmDisponivel = table.Column<int>(type: "int", nullable: false),
                    PrecoDiariaControlada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoExtrapolado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoLivre = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBPlanoDeCobranca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBPlanoDeCobranca_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBPlanoDeCobranca_TBGrupoDeAutomoveis_Grupo_Id",
                        column: x => x.Grupo_Id,
                        principalTable: "TBGrupoDeAutomoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBVeiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "varchar(10)", nullable: false),
                    Marca = table.Column<string>(type: "varchar(50)", nullable: false),
                    Cor = table.Column<string>(type: "varchar(30)", nullable: false),
                    Modelo = table.Column<string>(type: "varchar(50)", nullable: false),
                    TipoCombustivel = table.Column<string>(type: "varchar(50)", nullable: false),
                    CapacidadeCombustivel = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    ImagemEmBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TipoDaImagem = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Grupo_Id = table.Column<int>(type: "int", nullable: false),
                    Alugado = table.Column<bool>(type: "bit", nullable: false),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBVeiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBVeiculos_AspNetUsers_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBVeiculos_TBGrupoDeAutomoveis_Grupo_Id",
                        column: x => x.Grupo_Id,
                        principalTable: "TBGrupoDeAutomoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_TBAluguel_Usuario_Id",
                table: "TBAluguel",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBCliente_Usuario_Id",
                table: "TBCliente",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBCondutor_Cliente_Id",
                table: "TBCondutor",
                column: "Cliente_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBCondutor_Usuario_Id",
                table: "TBCondutor",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBConfiguracao_Usuario_Id",
                table: "TBConfiguracao",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBFuncionario_Usuario_Id",
                table: "TBFuncionario",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBGrupoDeAutomoveis_Usuario_Id",
                table: "TBGrupoDeAutomoveis",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBPlanoDeCobranca_Grupo_Id",
                table: "TBPlanoDeCobranca",
                column: "Grupo_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBPlanoDeCobranca_Usuario_Id",
                table: "TBPlanoDeCobranca",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBTaxa_Usuario_Id",
                table: "TBTaxa",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBVeiculos_Grupo_Id",
                table: "TBVeiculos",
                column: "Grupo_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TBVeiculos_Usuario_Id",
                table: "TBVeiculos",
                column: "Usuario_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "TBAluguel");

            migrationBuilder.DropTable(
                name: "TBCondutor");

            migrationBuilder.DropTable(
                name: "TBConfiguracao");

            migrationBuilder.DropTable(
                name: "TBFuncionario");

            migrationBuilder.DropTable(
                name: "TBPlanoDeCobranca");

            migrationBuilder.DropTable(
                name: "TBTaxa");

            migrationBuilder.DropTable(
                name: "TBVeiculos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TBCliente");

            migrationBuilder.DropTable(
                name: "TBGrupoDeAutomoveis");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
