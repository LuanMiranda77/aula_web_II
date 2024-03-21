﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App_aula_1.Models;

[Table("tbRastreamentoMetabolico")]
[Index("IdHoraPacienteProfissional", Name = "IX_tbRastreamentoMetabolico_IdHoraPaciente_Profissional")]
[Index("IdRastreamentoResposta", Name = "IX_tbRastreamentoMetabolico_IdRastreamentoResposta")]
public partial class TbRastreamentoMetabolico
{
    [Key]
    public int IdRastreamentoMetabolico { get; set; }

    public int IdRastreamentoResposta { get; set; }

    [Column("IdHoraPaciente_Profissional")]
    public int IdHoraPacienteProfissional { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string ObsGeral { get; set; }

    public int? Total { get; set; }

    [ForeignKey("IdHoraPacienteProfissional")]
    [InverseProperty("TbRastreamentoMetabolico")]
    public virtual TbHoraPacienteProfissional IdHoraPacienteProfissionalNavigation { get; set; }

    [ForeignKey("IdRastreamentoResposta")]
    [InverseProperty("TbRastreamentoMetabolico")]
    public virtual TbRastreamentoResposta IdRastreamentoRespostaNavigation { get; set; }
}