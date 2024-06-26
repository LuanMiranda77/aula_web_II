﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App_aula_1.Models;

[Table("tbEscalaBristol_Paciente_Consulta")]
[Index("IdEscalaBristol", Name = "IX_tbEscalaBristol_Paciente_Consulta_IdEscalaBristol")]
[Index("IdHoraPacienteProfissional", Name = "IX_tbEscalaBristol_Paciente_Consulta_IdHora_Paciente_Profissional")]
[Index("IdPaciente", Name = "IX_tbEscalaBristol_Paciente_Consulta_IdPaciente")]
public partial class TbEscalaBristolPacienteConsulta
{
    [Key]
    [Column("IdEscalaBristol_Paciente_Consulta")]
    public int IdEscalaBristolPacienteConsulta { get; set; }

    public int IdEscalaBristol { get; set; }

    public int IdPaciente { get; set; }

    [Column("IdHora_Paciente_Profissional")]
    public int IdHoraPacienteProfissional { get; set; }

    [ForeignKey("IdEscalaBristol")]
    [InverseProperty("TbEscalaBristolPacienteConsulta")]
    public virtual TbEscalaBristol IdEscalaBristolNavigation { get; set; }

    [ForeignKey("IdHoraPacienteProfissional")]
    [InverseProperty("TbEscalaBristolPacienteConsulta")]
    public virtual TbHoraPacienteProfissional IdHoraPacienteProfissionalNavigation { get; set; }

    [ForeignKey("IdPaciente")]
    [InverseProperty("TbEscalaBristolPacienteConsulta")]
    public virtual TbPaciente IdPacienteNavigation { get; set; }
}