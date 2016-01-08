using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace QuickBootstrap.Entities
{
    public class SalesData
    {
        [DisplayName("记录ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Yyyymmdd", TypeName = "int")]
        public int Yyyymmdd { get; set; }

        [Column("Hhmiss", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string  Hhmiss { get; set; }

        [DisplayName("完整订单时间")]
        public DateTime? GenerationTime { get; set; }

        [Required]
        [DisplayName("订单号")]
        [Column("O_cd", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string O_cd { get; set; }

        [DisplayName("广告主账号")]
        [Column("M_id", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string M_id { get; set; }

        [DisplayName("注册者账户和真实姓名")]
        [Column("Mbr_id", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string  Mbr_id { get; set; }


        [Required]
        [DisplayName("应得的佣金")]
        public decimal  Comm { get; set; }

        [DisplayName("联盟会员下会员账户")]
        [Column("U_id", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string U_id { get; set; }


        [Required]
        [DisplayName("商品编号")]
        [Column("P_cd", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string P_cd { get; set; }


        [Required]
        [DisplayName("商品数量")]
        public int  It_cnt { get; set; }

        [Required]
        [DisplayName("商品单价")]
        [Column("Price", TypeName = "Money")]
        public decimal Price { get; set; }

        [DisplayName("商品分类")]
        [Column("C_cd", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string  C_cd { get; set; }

        [DisplayName("添加时间")]
        public DateTime? AddTime { get; set; }

        // 销售量 (额)
        [Column("Sales", TypeName = "Money")]
        public decimal Sales { get; set; }

        // 佣金
        [Column("Commission", TypeName = "Money")]
        public decimal Commission { get; set; }

        [DisplayName("业绩状态状态")]
        [DefaultValue(0)]
        public int? Stat_code { get; set; }

        [Column("Stat_desc", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Stat_desc { get; set; }

        // 特定网站ID
        [Column("Affiliate_id", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string Affiliate_id { get; set; }

        public string Cancel_comment { get; set; }

        // 结算日期
        public string   Bill_yyyymmdd { get; set; }

        [DisplayName("更新时间")]
        public DateTime? UpdateTime { get; set; }

    }
}