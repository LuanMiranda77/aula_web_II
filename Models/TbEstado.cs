﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App_aula_1.Models;

[Table("tbEstado")]
[Index("IdPais", Name = "IX_tbEstado_IdPais")]
public partial class TbEstado
{
    [Key]
    public int IdEstado { get; set; }

    public int IdPais { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; }

    [Column("UF")]
    [StringLength(2)]
    [Unicode(false)]
    public string Uf { get; set; }

    [ForeignKey("IdPais")]
    [InverseProperty("TbEstado")]
    public virtual TbPais IdPaisNavigation { get; set; }

    [InverseProperty("IdEstadoNavigation")]
    public virtual ICollection<TbCidade> TbCidade { get; set; } = new List<TbCidade>();
}