﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App_aula_1.Models;

[Table("tbPais")]
public partial class TbPais
{
    [Key]
    public int IdPais { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string Sigla { get; set; }

    [InverseProperty("IdPaisNavigation")]
    public virtual ICollection<TbEstado> TbEstado { get; set; } = new List<TbEstado>();
}