﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using QuickBootstrap.Attributes;

namespace QuickBootstrap.Entities
{
    public class SalesData
    {
        [DisplayName("记录ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("下订单时间")]
        public DateTime? GenerationTime { get; set; }

        [Required]
        [DisplayName("订单号")]
        public string O_cd { get; set; }

        [DisplayName("广告主账号")]
        public string M_id { get; set; }

        [Required]
        [DisplayName("注册者账户和真实姓名")]
        public string  Mbr_id { get; set; }


        [Required]
        [DisplayName("应得的佣金")]
        public decimal  Comm { get; set; }

        [DisplayName("联盟会员下会员账户")]
        public string U_id { get; set; }


        [Required]
        [DisplayName("商品编号")]
        public string P_cd { get; set; }


        [Required]
        [DisplayName("商品数量")]
        public int  It_cnt { get; set; }

        [Required]
        [DisplayName("商品单价")]
        public decimal  Price { get; set; }

        [DisplayName("商品分类")]
        public string  C_cd { get; set; }


        [Required]
        [DisplayName("商品分类")]
        public DateTime AddTime { get; set; }

    }
}